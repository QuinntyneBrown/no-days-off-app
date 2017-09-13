import { Injectable } from "@angular/core";
import {
    CanActivate,
    CanActivateChild,
    CanLoad,
    Route,
    ActivatedRouteSnapshot,
    RouterStateSnapshot
} from '@angular/router';

import { Storage } from "../shared/services/storage.service";
import { Observable } from "rxjs";
import { constants } from "../shared/constants";
import { UsersService } from "./users.service";

@Injectable()
export class CurrentUserGuardService implements CanActivate {
    constructor(
        private _usersService: UsersService,
        private _storage: Storage
    ) { }

    public canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean> {

        var currentUser = this._storage.get({ name: constants.CURRENT_USER_KEY });

        if (currentUser)
            return Observable.of(true);

        return this._usersService.getCurrent()
            .map(result => {                
                this._storage.put({ name: constants.CURRENT_USER_KEY, value: result.user });
                return true;
            });
    }
}