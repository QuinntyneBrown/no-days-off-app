import {Component, ChangeDetectorRef} from "@angular/core";
import {DashboardTilesService} from "./dashboard-tiles.service";
import {Router} from "@angular/router";
import {pluckOut} from "../shared/utilities/pluck-out";
import {EventHub} from "../shared/services/event-hub";
import {Subscription} from "rxjs/Subscription";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./dashboard-tile-paginated-list-page.component.html",
    styleUrls: ["./dashboard-tile-paginated-list-page.component.css"],
    selector: "ce-dashboard-tile-paginated-list-page"   
})
export class DashboardTilePaginatedListPageComponent {
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _dashboardTilesService: DashboardTilesService,
        private _correlationIdsList: CorrelationIdsList,
        private _eventHub: EventHub,
        private _router: Router
    ) {
        this.subscription = this._eventHub.events.subscribe(x => {      
            
            if (this._correlationIdsList.hasId(x.payload.correlationId) && x.type == "[DashboardTiles] DashboardTileAddedOrUpdated") {
                this._dashboardTilesService.get().toPromise().then(x => {
                    this.unfilteredDashboardTiles = x.dashboardTiles;
                    this.dashboardTiles = this.filterTerm != null ? this.filteredDashboardTiles : this.unfilteredDashboardTiles;
                    this._changeDetectorRef.detectChanges();
                });
            } else if (x.type == "[DashboardTiles] DashboardTileAddedOrUpdated") {
                
            }
        });      
    }
    
    public async ngOnInit() {
        this.unfilteredDashboardTiles = (await this._dashboardTilesService.get().toPromise()).dashboardTiles;   
        this.dashboardTiles = this.filterTerm != null ? this.filteredDashboardTiles : this.unfilteredDashboardTiles;       
    }

    public tryToDelete($event) {        
        const correlationId = this._correlationIdsList.newId();

        this.unfilteredDashboardTiles = pluckOut({
            items: this.unfilteredDashboardTiles,
            value: $event.detail.dashboardTile.id
        });

        this.dashboardTiles = this.filterTerm != null ? this.filteredDashboardTiles : this.unfilteredDashboardTiles;
        
        this._dashboardTilesService.remove({ dashboardTile: $event.detail.dashboardTile, correlationId }).subscribe();
    }

    public tryToEdit($event) {
        this._router.navigate(["dashboardTiles", $event.detail.dashboardTile.id]);
    }

    public handleDashboardTilesFilterKeyUp($event) {
        this.filterTerm = $event.detail.value;
        this.pageNumber = 1;
        this.dashboardTiles = this.filterTerm != null ? this.filteredDashboardTiles : this.unfilteredDashboardTiles;        
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
        this.subscription = null;
    }

    private subscription: Subscription;
    public _dashboardTiles: Array<any> = [];
    public filterTerm: string;
    public pageNumber: number;

    public dashboardTiles: Array<any> = [];
    public unfilteredDashboardTiles: Array<any> = [];
    public get filteredDashboardTiles() {
        return this.unfilteredDashboardTiles.filter((x) => x.email.indexOf(this.filterTerm) > -1);
    }
}
