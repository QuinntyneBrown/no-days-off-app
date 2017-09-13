import {Component} from "@angular/core";
import {DashboardsService} from "./dashboards.service";
import {Router,ActivatedRoute} from "@angular/router";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./dashboard-edit-page.component.html",
    styleUrls: ["./dashboard-edit-page.component.css"],
    selector: "ce-dashboard-edit-page"
})
export class DashboardEditPageComponent {
    constructor(private _dashboardsService: DashboardsService,
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        private _correlationIdsList: CorrelationIdsList
    ) { }

    public async ngOnInit() {
        if (this._activatedRoute.snapshot.params["id"]) {            
            this.dashboard = (await this._dashboardsService.getById({ id: this._activatedRoute.snapshot.params["id"] }).toPromise()).dashboard;
        }
    }

    public tryToSave($event) {
        const correlationId = this._correlationIdsList.newId();
        this._dashboardsService.addOrUpdate({ dashboard: $event.detail.dashboard, correlationId }).subscribe();
        this._router.navigateByUrl("/dashboards");
    }

    public dashboard = {};
}
