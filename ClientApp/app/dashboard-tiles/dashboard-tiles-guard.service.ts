import { Injectable } from "@angular/core";
import {
    CanActivate,
    CanActivateChild,
    CanLoad,
    Route,
    ActivatedRouteSnapshot,
    RouterStateSnapshot
} from '@angular/router';

import { Storage } from "../shared/services/storage.service";
import { Observable } from "rxjs";
import { constants } from "../shared/constants";
import { DashboardTilesService } from "./dashboard-tiles.service";

@Injectable()
export class DashboardTilesGuardService implements CanActivate {
    constructor(
        private _dashboardTilesService: DashboardTilesService,
        private _storage: Storage
    ) { }

    public canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean> {        
        const dashboardId = next.params["id"];

        const observable = dashboardId ? this._dashboardTilesService.getByDashboardId({ dashboardId }) :
            this._dashboardTilesService.getByDefaultDashboard();
        
        return observable
            .map(result => {
                this._storage.put({ name: constants.DASHBOARD_TILES, value: result.dashboardTiles });
                return true;
            });
    }
}