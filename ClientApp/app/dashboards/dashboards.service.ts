import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Dashboard } from "./dashboard.model";
import { Observable } from "rxjs/Observable";
import { ErrorService } from "../shared/services/error.service";

@Injectable()
export class DashboardsService {
    constructor(
        private _errorService: ErrorService,
        private _httpClient: HttpClient)
    { }

    public addOrUpdate(options: { dashboard: Dashboard, correlationId: string }) {
        return this._httpClient
            .post(`${this._baseUrl}/api/dashboards/add`, options)
            .catch(this._errorService.catchErrorResponse);
    }

    public get(): Observable<{ dashboards: Array<Dashboard> }> {
        return this._httpClient
            .get<{ dashboards: Array<Dashboard> }>(`${this._baseUrl}/api/dashboards/get`)
            .catch(this._errorService.catchErrorResponse);
    }

    public getByCurrentUsername(): Observable<{ dashboards: Array<Dashboard> }> {
        return this._httpClient
            .get<{ dashboards: Array<Dashboard> }>(`${this._baseUrl}/api/dashboards/getByCurrentUsername`)
            .catch(this._errorService.catchErrorResponse);
    }

    public getById(options: { id: number }): Observable<{ dashboard:Dashboard}> {
        return this._httpClient
            .get<{dashboard: Dashboard}>(`${this._baseUrl}/api/dashboards/getById?id=${options.id}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public getDefault() {
        return this._httpClient
            .get<{ dashboard: Dashboard }>(`${this._baseUrl}/api/dashboards/getDefault`)
            .catch(this._errorService.catchErrorResponse);
    }

    public remove(options: { dashboard: Dashboard, correlationId: string }) {
        return this._httpClient
            .delete(`${this._baseUrl}/api/dashboards/remove?id=${options.dashboard.id}&correlationId=${options.correlationId}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public get _baseUrl() { return window["__BASE_URL__"]; }
}
