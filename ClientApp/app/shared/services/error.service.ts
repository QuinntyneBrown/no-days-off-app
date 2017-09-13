import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest, HttpHandler, HttpEvent, HttpEventType, HttpInterceptor, HttpErrorResponse } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { ErrorObservable } from 'rxjs/observable/ErrorObservable';
import 'rxjs/add/observable/throw';
import { Storage } from "../services/storage.service";
import { LoginRedirectService } from "../services/login-redirect.service";
import { constants } from "../constants";

@Injectable()
export class ErrorService {
    constructor(private _loginRedirectService: LoginRedirectService, private _storage: Storage) {
        this.catchErrorResponse = this.catchErrorResponse.bind(this);
    }

    public catchErrorResponse(error: any): ErrorObservable {
        if (error instanceof HttpErrorResponse && error.status === 401) {
            this._storage.put({ name: constants.ACCESS_TOKEN_KEY, value: null });
            this._loginRedirectService.redirectToLogin();
        }
        return Observable.throw(error.message);
    }
}
