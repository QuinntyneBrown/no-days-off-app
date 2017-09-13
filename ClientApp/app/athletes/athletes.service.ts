import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Athlete } from "./athlete.model";
import { Observable } from "rxjs/Observable";
import { ErrorService } from "../shared/services/error.service";

@Injectable()
export class AthletesService {
    constructor(
        private _errorService: ErrorService,
        private _httpClient: HttpClient)
    { }

    public addOrUpdate(options: { athlete: Athlete, correlationId: string }) {
        return this._httpClient
            .post(`${this._baseUrl}/api/athletes/add`, options)
            .catch(this._errorService.catchErrorResponse);
    }

    public get(): Observable<{ athletes: Array<Athlete> }> {
        return this._httpClient
            .get<{ athletes: Array<Athlete> }>(`${this._baseUrl}/api/athletes/get`)
            .catch(this._errorService.catchErrorResponse);
    }

    public getCurrent(): Observable<{ athlete: Athlete }> {
        return this._httpClient
            .get<{ athlete: Athlete }>(`${this._baseUrl}/api/athletes/getCurrent`)
            .catch(this._errorService.catchErrorResponse);
    }

    public getById(options: { id: number }): Observable<{ athlete:Athlete}> {
        return this._httpClient
            .get<{athlete: Athlete}>(`${this._baseUrl}/api/athletes/getById?id=${options.id}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public remove(options: { athlete: Athlete, correlationId: string }) {
        return this._httpClient
            .delete(`${this._baseUrl}/api/athletes/remove?id=${options.athlete.id}&correlationId=${options.correlationId}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public get _baseUrl() { return ""; }
}
