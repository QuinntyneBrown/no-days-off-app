import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { BoundedContext } from "./bounded-context.model";
import { Observable } from "rxjs/Observable";
import { ErrorService } from "../shared/services/error.service";

@Injectable()
export class BoundedContextsService {
    constructor(
        private _errorService: ErrorService,
        private _httpClient: HttpClient)
    { }

    public addOrUpdate(options: { boundedContext: BoundedContext, correlationId: string }) {
        return this._httpClient
            .post(`${this._baseUrl}/api/boundedContexts/add`, options)
            .catch(this._errorService.catchErrorResponse);
    }

    public get(): Observable<{ boundedContexts: Array<BoundedContext> }> {
        return this._httpClient
            .get<{ boundedContexts: Array<BoundedContext> }>(`${this._baseUrl}/api/boundedContexts/get`)
            .catch(this._errorService.catchErrorResponse);
    }

    public getById(options: { id: number }): Observable<{ boundedContext:BoundedContext}> {
        return this._httpClient
            .get<{boundedContext: BoundedContext}>(`${this._baseUrl}/api/boundedContexts/getById?id=${options.id}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public remove(options: { boundedContext: BoundedContext, correlationId: string }) {
        return this._httpClient
            .delete(`${this._baseUrl}/api/boundedContexts/remove?id=${options.boundedContext.id}&correlationId=${options.correlationId}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public get _baseUrl() { return ""; }
}
