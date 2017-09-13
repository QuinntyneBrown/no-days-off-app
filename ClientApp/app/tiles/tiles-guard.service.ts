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
import { TilesService } from "./tiles.service";

@Injectable()
export class TilesGuardService implements CanActivate {
    constructor(
        private _tilesService: TilesService,
        private _storage: Storage
    ) { }

    public canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<boolean> {
        return this._tilesService.get()
            .map(result => {
                this._storage.put({ name: constants.TILES, value: result.tiles });
                return true;
            });
    }
}