import {Component} from "@angular/core";
import {DashboardTilesService} from "./dashboard-tiles.service";
import {Router,ActivatedRoute} from "@angular/router";
import {guid} from "../shared/utilities/guid";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./dashboard-tile-edit-page.component.html",
    styleUrls: ["./dashboard-tile-edit-page.component.css"],
    selector: "ce-dashboard-tile-edit-page"
})
export class DashboardTileEditPageComponent {
    constructor(private _dashboardTilesService: DashboardTilesService,
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        private _correlationIdsList: CorrelationIdsList
    ) { }

    public async ngOnInit() {
        if (this._activatedRoute.snapshot.params["id"]) {            
            this.dashboardTile = (await this._dashboardTilesService.getById({ id: this._activatedRoute.snapshot.params["id"] }).toPromise()).dashboardTile;
        }
    }

    public tryToSave($event) {
        const correlationId = this._correlationIdsList.newId();
        this._dashboardTilesService.addOrUpdate({ dashboardTile: $event.detail.dashboardTile, correlationId }).subscribe();
        this._router.navigateByUrl("/dashboardTiles");
    }

    public dashboardTile = {};
}
