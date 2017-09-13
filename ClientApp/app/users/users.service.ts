import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Injectable()
export class UsersService {
    constructor(
        private _httpClient: HttpClient
    ) {

    }

    public getCurrent() {
        return this._httpClient.get<{user:any}>(`${this._baseUrl}/api/users/current`);
    }

    public get _baseUrl() { return window["__BASE_URL__"]; }
}