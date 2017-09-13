import { Injectable } from "@angular/core";
import {
    CanActivate,
    CanActivateChild,
    CanLoad,
    Route,
    ActivatedRouteSnapshot,
    RouterStateSnapshot
} from '@angular/router';

import { Storage } from "../services/storage.service";
import { LoginRedirectService } from "../services/login-redirect.service";
import { Observable } from "rxjs";
import { constants } from "../constants";

@Injectable()
export class TenantGuardService implements CanActivate {
    constructor(
        private _storage: Storage,
        private _loginRedirectService: LoginRedirectService
    ) { }

    public canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean> {
        const tenant = this._storage.get({ name: constants.TENANT });

        if (tenant)
            return Observable.of(true);

        this._loginRedirectService.lastPath = state.url;
        this._loginRedirectService.redirectToSetTenant();

        return Observable.of(false);
    }
}