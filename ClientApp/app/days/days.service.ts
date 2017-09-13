import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Day } from "./day.model";
import { Observable } from "rxjs/Observable";
import { ErrorService } from "../shared/services/error.service";

@Injectable()
export class DaysService {
    constructor(
        private _errorService: ErrorService,
        private _httpClient: HttpClient)
    { }

    public addOrUpdate(options: { day: Day, correlationId: string }) {
        return this._httpClient
            .post(`${this._baseUrl}/api/days/add`, options)
            .catch(this._errorService.catchErrorResponse);
    }

    public get(): Observable<{ days: Array<Day> }> {
        return this._httpClient
            .get<{ days: Array<Day> }>(`${this._baseUrl}/api/days/get`)
            .catch(this._errorService.catchErrorResponse);
    }

    public getById(options: { id: number }): Observable<{ day:Day}> {
        return this._httpClient
            .get<{day: Day}>(`${this._baseUrl}/api/days/getById?id=${options.id}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public remove(options: { day: Day, correlationId: string }) {
        return this._httpClient
            .delete(`${this._baseUrl}/api/days/remove?id=${options.day.id}&correlationId=${options.correlationId}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public get _baseUrl() { return ""; }
}
