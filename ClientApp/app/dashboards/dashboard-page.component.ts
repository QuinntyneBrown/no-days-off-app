import {Component, NgZone, Injector} from "@angular/core";
import {DashboardsService} from "./dashboards.service";
import {Dashboard} from "./dashboard.model";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";
import {EventHub} from "../shared/services/event-hub";
import {Router,ActivatedRoute} from "@angular/router";
import {ModalService} from "../shared/services/modal.service";
import {TileCatalogModalWindowComponent} from "../tiles/tile-catalog-modal-window.component";
import {Observable} from "rxjs/Observable";
import {pluckOut} from "../shared/utilities/pluck-out";
import {TILE_SELECTED} from "../tiles/tiles.actions";
import {DashboardTilesService} from "../dashboard-tiles/dashboard-tiles.service";
import {
    CONFIGURE_DASHBOARD_TILE,
    REMOVE_DASHBOARD_TILE,
    SAVE_DASHBOARD_TILE,
    CONFIGURE_DASHBOARD_TILE_SAVE_CLICK,
    ADDED_OR_UDPATED as DASHBOARD_TILE_ADDED_OR_UPDATED
} from "../dashboard-tiles/dashboard-tiles.actions";
import {addOrUpdate} from "../shared/utilities/add-or-update";
import {ConfigureDashboardTileModalWindowComponent} from "../dashboard-tiles/configure-dashboard-tile-modal-window.component";
import {ADDED_OR_UPDATED_DASHBOARD, REMOVED_DASHBOARD} from "./dashboard.actions";
import {Subscription} from "rxjs/Subscription";

@Component({
    templateUrl: "./dashboard-page.component.html",
    styleUrls: ["./dashboard-page.component.css"],
    selector: "ce-dashboard-page"    
})
export class DashboardPageComponent {
    constructor(
        private _activatedRoute: ActivatedRoute,
        private _dashboardsService: DashboardsService,
        private _dashboardTilesService: DashboardTilesService,
        private _router: Router,
        private _eventHub: EventHub,
        private _correlationIdsList: CorrelationIdsList,
        private _injector: Injector,
        private _modalService: ModalService,
        private _ngZone: NgZone
    ) {
        this.ngOnInit = this.ngOnInit.bind(this);
        this.openConfigureDashboardTileModal = this.openConfigureDashboardTileModal.bind(this);
        this.removeDashboardTile = this.removeDashboardTile.bind(this);
        this.saveDashboardTile = this.saveDashboardTile.bind(this);
        
    }


    public subscription: Subscription;

    public get dashboardTiles() { return this.dashboard.dashboardTiles; }

    public ngOnInit() {
        this.loadData();

        this.subscription = this._eventHub.events.subscribe((x) => {
            
            if (x.payload && this._correlationIdsList.hasId(x.payload.correlationId)
                && (x.type == ADDED_OR_UPDATED_DASHBOARD || x.type == REMOVED_DASHBOARD)) {                
                this._ngZone.run(() => {
                    this._dashboardsService.getByCurrentUsername().toPromise().then(x => this.dashboards = x.dashboards);
                });                
            }
            
            if (x.type == TILE_SELECTED) {
                const correlationId:any = this._correlationIdsList.newId();
                this._dashboardTilesService.addOrUpdate({
                    dashboardTile: {
                        dashboardId: this.dashboard.id,
                        tileId: x.payload.tile.id,
                        top: 1,
                        left: 1,
                        width: 1,
                        height:1
                    },
                    correlationId
                }).subscribe();        
            }      
            
            if (x.payload && this._correlationIdsList.hasId(x.payload.correlationId) && x.type == DASHBOARD_TILE_ADDED_OR_UPDATED) {                
                addOrUpdate({
                    items: this.dashboard.dashboardTiles,
                    item: x.payload.entity
                });                
            }
        });    

        this._activatedRoute.params.subscribe(x => {
            this.loadData();
        });

        document.body.addEventListener(SAVE_DASHBOARD_TILE, this.saveDashboardTile);
        document.body.addEventListener(CONFIGURE_DASHBOARD_TILE, this.openConfigureDashboardTileModal);
        document.body.addEventListener(REMOVE_DASHBOARD_TILE, this.removeDashboardTile);
        

    }

    public saveDashboardTile($event: CustomEvent) {
        
        const correlationId: any = this._correlationIdsList.newId();
        
        this._dashboardTilesService.addOrUpdate({
            dashboardTile: $event.detail.dashboardTile,
            correlationId
        }).subscribe(); 
    }

    public openConfigureDashboardTileModal(customEvent:CustomEvent) {
        this._modalService.open({
            html: `<ce-configure-dashboard-tile-modal-window dashboard-tile='${JSON.stringify(customEvent.detail.dashboardTile)}'></ce-configure-dashboard-tile-modal-window>`,
        });
    }

    public removeDashboardTile(customEvent: CustomEvent) {        
        const correlationId = this._correlationIdsList.newId();

        this._dashboardTilesService.remove({
            dashboardTile: customEvent.detail.dashboardTile,
            correlationId
        }).subscribe(); 
    }

    public loadData() {
        let observables: Array<Observable<any>> = [this._dashboardsService.getByCurrentUsername()];

        observables.push(this.dashboardId ? this._dashboardsService.getById({ id: this.dashboardId }) : this._dashboardsService.getDefault());

        Observable.forkJoin(observables).subscribe((results) => {
            this.dashboards = results[0].dashboards;
            this.dashboard = results[1].dashboard;
        });
    }

    public openTileCatalog() {
        this._modalService.open({ html: "<ce-tile-catalog-modal-window></ce-tile-catalog-modal-window>"});
    }

    public tryToSaveDashboard($event) {            
        this.newDashboard = null;
        var guid = this._correlationIdsList.newId();
        this._dashboardsService.addOrUpdate({ dashboard: $event.details.dashboard, correlationId: guid }).subscribe();        
    }

    public tryToAddDashboard($event) {
        this.newDashboard = new Dashboard();
    }

    public dashboard: Dashboard = new Dashboard();

    public newDashboard: Dashboard = null;

    public dashboards: Array<Dashboard> = [];    
    
    public get dashboardId() { return this._activatedRoute.snapshot.params["id"]; }
    
    public tryToDelete(id: number) {
        pluckOut({ value: this.dashboard, items: this.dashboards });
        const correlationId = this._correlationIdsList.newId();
        this._dashboardsService.remove({ dashboard: this.dashboard, correlationId: correlationId }).subscribe();
        this._router.navigate([""]);
    }

    ngOnDestroy() {
        document.body.removeEventListener(CONFIGURE_DASHBOARD_TILE, this.openConfigureDashboardTileModal);
        document.body.removeEventListener(REMOVE_DASHBOARD_TILE, this.removeDashboardTile);
        document.body.removeEventListener(SAVE_DASHBOARD_TILE, this.saveDashboardTile);
        this.subscription.unsubscribe();
        this.subscription = null;
    }
}
