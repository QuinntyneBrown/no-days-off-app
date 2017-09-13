import { Injectable } from "@angular/core";
import { HttpClient, HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { Storage } from "../services/storage.service";
import { constants } from "../constants";

@Injectable()
export class TenantInterceptor implements HttpInterceptor {
    constructor(private _storage: Storage) { }
    intercept(httpRequest: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(httpRequest.clone({
            headers: httpRequest.headers.set('Tenant', this._storage.get({ name: constants.TENANT }))
        }));
    }
}
