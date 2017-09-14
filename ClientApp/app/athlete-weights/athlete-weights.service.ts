import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { AthleteWeight } from "./athlete-weight.model";
import { Observable } from "rxjs/Observable";
import { ErrorService } from "../shared/services/error.service";

@Injectable()
export class AthleteWeightsService {
    constructor(
        private _errorService: ErrorService,
        private _httpClient: HttpClient)
    { }

    public addOrUpdate(options: { athleteWeight: AthleteWeight, correlationId: string }) {
        return this._httpClient
            .post(`${this._baseUrl}/api/athleteWeights/add`, options)
            .catch(this._errorService.catchErrorResponse);
    }

    public get(): Observable<{ athleteWeights: Array<AthleteWeight> }> {
        return this._httpClient
            .get<{ athleteWeights: Array<AthleteWeight> }>(`${this._baseUrl}/api/athleteWeights/get`)
            .catch(this._errorService.catchErrorResponse);
    }

    public getById(options: { id: number }): Observable<{ athleteWeight:AthleteWeight}> {
        return this._httpClient
            .get<{athleteWeight: AthleteWeight}>(`${this._baseUrl}/api/athleteWeights/getById?id=${options.id}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public remove(options: { athleteWeight: AthleteWeight, correlationId: string }) {
        return this._httpClient
            .delete(`${this._baseUrl}/api/athleteWeights/remove?id=${options.athleteWeight.id}&correlationId=${options.correlationId}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public get _baseUrl() { return ""; }
}
