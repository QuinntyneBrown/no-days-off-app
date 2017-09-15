import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { BodyPartDay } from "./body-part-day.model";
import { Observable } from "rxjs/Observable";
import { ErrorService } from "../shared/services/error.service";

@Injectable()
export class BodyPartDaysService {
    constructor(
        private _errorService: ErrorService,
        private _httpClient: HttpClient)
    { }

    public addOrUpdate(options: { bodyPartDay: BodyPartDay, correlationId: string }) {
        return this._httpClient
            .post(`${this._baseUrl}/api/bodyPartDays/add`, options)
            .catch(this._errorService.catchErrorResponse);
    }

    public get(): Observable<{ bodyPartDays: Array<BodyPartDay> }> {
        return this._httpClient
            .get<{ bodyPartDays: Array<BodyPartDay> }>(`${this._baseUrl}/api/bodyPartDays/get`)
            .catch(this._errorService.catchErrorResponse);
    }

    public getById(options: { id: number }): Observable<{ bodyPartDay:BodyPartDay}> {
        return this._httpClient
            .get<{bodyPartDay: BodyPartDay}>(`${this._baseUrl}/api/bodyPartDays/getById?id=${options.id}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public remove(options: { bodyPartDay: BodyPartDay, correlationId: string }) {
        return this._httpClient
            .delete(`${this._baseUrl}/api/bodyPartDays/remove?id=${options.bodyPartDay.id}&correlationId=${options.correlationId}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public get _baseUrl() { return ""; }
}
