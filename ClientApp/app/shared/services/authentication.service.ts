import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {constants} from "../constants";
import {Storage} from "./storage.service";
import {HttpClient, HttpHeaders} from "@angular/common/http";

function formEncode(data: any) {
    var pairs = [];
    for (var name in data) {
        pairs.push(encodeURIComponent(name) + '=' + encodeURIComponent(data[name]));
    }
    return pairs.join('&').replace(/%20/g, '+');
}

@Injectable()
export class AuthenticationService {
    constructor(private _httpClient: HttpClient, private _storage: Storage) { }  

    public tryToLogin(options: any) {
        Object.assign(options, { grant_type: "password" });

        const headers = new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded');

        return this._httpClient.post(`${this._baseUrl}/api/users/token`, formEncode(options), { headers })
            .map((response) => {
                const accessToken = response["access_token"];
                this._storage.put({ name: constants.ACCESS_TOKEN_KEY, value: accessToken });
                return accessToken;
            });
    }

    public get _baseUrl() { return window["__BASE_URL__"]; }
}