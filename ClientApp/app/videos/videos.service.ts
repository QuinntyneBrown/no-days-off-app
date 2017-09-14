import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Video } from "./video.model";
import { Observable } from "rxjs/Observable";
import { ErrorService } from "../shared/services/error.service";

@Injectable()
export class VideosService {
    constructor(
        private _errorService: ErrorService,
        private _httpClient: HttpClient)
    { }

    public addOrUpdate(options: { video: Video, correlationId: string }) {
        return this._httpClient
            .post(`${this._baseUrl}/api/videos/add`, options)
            .catch(this._errorService.catchErrorResponse);
    }

    public get(): Observable<{ videos: Array<Video> }> {
        return this._httpClient
            .get<{ videos: Array<Video> }>(`${this._baseUrl}/api/videos/get`)
            .catch(this._errorService.catchErrorResponse);
    }

    public getById(options: { id: number }): Observable<{ video:Video}> {
        return this._httpClient
            .get<{video: Video}>(`${this._baseUrl}/api/videos/getById?id=${options.id}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public remove(options: { video: Video, correlationId: string }) {
        return this._httpClient
            .delete(`${this._baseUrl}/api/videos/remove?id=${options.video.id}&correlationId=${options.correlationId}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public get _baseUrl() { return ""; }
}
