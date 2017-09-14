import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { ErrorService } from "../shared/services/error.service";

@Injectable()
export class DigitalAssetsService {
    constructor(
        private _errorService: ErrorService,
        private _httpClient: HttpClient)
    { }

    public upload(options: { data: FormData }) {
        return this._httpClient.post<{ digitalAssets: Array<any> }>("/api/digitalasset/upload", options.data);        
    }

    public get _baseUrl() { return ""; }
}
