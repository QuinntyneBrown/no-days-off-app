import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Tile } from "./tile.model";
import { Observable } from "rxjs/Observable";
import { ErrorService } from "../shared/services/error.service";

@Injectable()
export class TilesService {
    constructor(
        private _errorService: ErrorService,
        private _httpClient: HttpClient)
    { }

    public addOrUpdate(options: { tile: Tile, correlationId: string }) {
        return this._httpClient
            .post(`${this._baseUrl}/api/tiles/add`, options)
            .catch(this._errorService.catchErrorResponse);
    }

    public get(): Observable<{ tiles: Array<Tile> }> {
        return this._httpClient
            .get<{ tiles: Array<Tile> }>(`${this._baseUrl}/api/tiles/get`)
            .catch(this._errorService.catchErrorResponse);
    }

    public getById(options: { id: number }): Observable<{ tile:Tile}> {
        return this._httpClient
            .get<{tile: Tile}>(`${this._baseUrl}/api/tiles/getById?id=${options.id}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public remove(options: { tile: Tile, correlationId: string }) {
        return this._httpClient
            .delete(`${this._baseUrl}/api/tiles/remove?id=${options.tile.id}&correlationId=${options.correlationId}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public get _baseUrl() { return window["__BASE_URL__"]; }
}
