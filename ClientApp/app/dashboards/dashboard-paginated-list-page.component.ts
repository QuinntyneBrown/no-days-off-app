import {Component, ChangeDetectorRef} from "@angular/core";
import {DashboardsService} from "./dashboards.service";
import {Router} from "@angular/router";
import {pluckOut} from "../shared/utilities/pluck-out";
import {EventHub} from "../shared/services/event-hub";
import {Subscription} from "rxjs/Subscription";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./dashboard-paginated-list-page.component.html",
    styleUrls: ["./dashboard-paginated-list-page.component.css"],
    selector: "ce-dashboard-paginated-list-page"   
})
export class DashboardPaginatedListPageComponent {
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _dashboardsService: DashboardsService,
        private _correlationIdsList: CorrelationIdsList,
        private _eventHub: EventHub,
        private _router: Router
    ) {
        this.subscription = this._eventHub.events.subscribe(x => {      
            
            if (this._correlationIdsList.hasId(x.payload.correlationId) && x.type == "[Dashboards] DashboardAddedOrUpdated") {
                this._dashboardsService.get().toPromise().then(x => {
                    this.unfilteredDashboards = x.dashboards;
                    this.dashboards = this.filterTerm != null ? this.filteredDashboards : this.unfilteredDashboards;
                    this._changeDetectorRef.detectChanges();
                });
            } else if (x.type == "[Dashboards] DashboardAddedOrUpdated") {
                
            }
        });      
    }
    
    public async ngOnInit() {
        this.unfilteredDashboards = (await this._dashboardsService.get().toPromise()).dashboards;   
        this.dashboards = this.filterTerm != null ? this.filteredDashboards : this.unfilteredDashboards;       
    }

    public tryToDelete($event) {        
        const correlationId = this._correlationIdsList.newId();

        this.unfilteredDashboards = pluckOut({
            items: this.unfilteredDashboards,
            value: $event.detail.dashboard.id
        });

        this.dashboards = this.filterTerm != null ? this.filteredDashboards : this.unfilteredDashboards;
        
        this._dashboardsService.remove({ dashboard: $event.detail.dashboard, correlationId }).subscribe();
    }

    public tryToEdit($event) {
        this._router.navigate(["dashboards", $event.detail.dashboard.id]);
    }

    public handleDashboardsFilterKeyUp($event) {
        this.filterTerm = $event.detail.value;
        this.pageNumber = 1;
        this.dashboards = this.filterTerm != null ? this.filteredDashboards : this.unfilteredDashboards;        
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
        this.subscription = null;
    }

    private subscription: Subscription;
    public _dashboards: Array<any> = [];
    public filterTerm: string;
    public pageNumber: number;

    public dashboards: Array<any> = [];
    public unfilteredDashboards: Array<any> = [];
    public get filteredDashboards() {
        return this.unfilteredDashboards.filter((x) => x.email.indexOf(this.filterTerm) > -1);
    }
}
