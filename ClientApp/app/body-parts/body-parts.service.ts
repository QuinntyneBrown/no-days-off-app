import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { BodyPart } from "./body-part.model";
import { Observable } from "rxjs/Observable";
import { ErrorService } from "../shared/services/error.service";

@Injectable()
export class BodyPartsService {
    constructor(
        private _errorService: ErrorService,
        private _httpClient: HttpClient)
    { }

    public addOrUpdate(options: { bodyPart: BodyPart, correlationId: string }) {
        return this._httpClient
            .post(`${this._baseUrl}/api/bodyParts/add`, options)
            .catch(this._errorService.catchErrorResponse);
    }

    public get(): Observable<{ bodyParts: Array<BodyPart> }> {
        return this._httpClient
            .get<{ bodyParts: Array<BodyPart> }>(`${this._baseUrl}/api/bodyParts/get`)
            .catch(this._errorService.catchErrorResponse);
    }

    public getById(options: { id: number }): Observable<{ bodyPart:BodyPart}> {
        return this._httpClient
            .get<{bodyPart: BodyPart}>(`${this._baseUrl}/api/bodyParts/getById?id=${options.id}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public remove(options: { bodyPart: BodyPart, correlationId: string }) {
        return this._httpClient
            .delete(`${this._baseUrl}/api/bodyParts/remove?id=${options.bodyPart.id}&correlationId=${options.correlationId}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public get _baseUrl() { return ""; }
}
