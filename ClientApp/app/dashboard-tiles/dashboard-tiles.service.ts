import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { DashboardTile } from "./dashboard-tile.model";
import { Observable } from "rxjs/Observable";
import { ErrorService } from "../shared/services/error.service";

@Injectable()
export class DashboardTilesService {
    constructor(
        private _errorService: ErrorService,
        private _httpClient: HttpClient)
    { }

    public addOrUpdate(options: { dashboardTile: DashboardTile, correlationId: string }) {
        return this._httpClient
            .post(`${this._baseUrl}/api/dashboardTiles/add`, options)
            .catch(this._errorService.catchErrorResponse);
    }

    public get(): Observable<{ dashboardTiles: Array<DashboardTile> }> {
        return this._httpClient
            .get<{ dashboardTiles: Array<DashboardTile> }>(`${this._baseUrl}/api/dashboardTiles/get`)
            .catch(this._errorService.catchErrorResponse);
    }

    public getById(options: { id: number }): Observable<{ dashboardTile:DashboardTile}> {
        return this._httpClient
            .get<{dashboardTile: DashboardTile}>(`${this._baseUrl}/api/dashboardTiles/getById?id=${options.id}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public getByDashboardId(options: { dashboardId: number }): Observable<{ dashboardTiles: Array<DashboardTile> }> {
        return this._httpClient
            .get<{ dashboardTile: DashboardTile }>(`${this._baseUrl}/api/dashboardTiles/getByDashboardId?dashboardId=${options.dashboardId}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public getByDefaultDashboard(): Observable<{ dashboardTiles: Array<DashboardTile> }> {
        return this._httpClient
            .get<{ dashboardTile: DashboardTile }>(`${this._baseUrl}/api/dashboardTiles/getByDefaultDashboard`)
            .catch(this._errorService.catchErrorResponse);
    }

    public remove(options: { dashboardTile: DashboardTile, correlationId: string }) {
        return this._httpClient
            .delete(`${this._baseUrl}/api/dashboardTiles/remove?id=${options.dashboardTile.id}&correlationId=${options.correlationId}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public get _baseUrl() { return window["__BASE_URL__"]; }
}
