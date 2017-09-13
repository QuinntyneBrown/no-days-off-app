import { Injectable } from "@angular/core";
import { CanLoad } from '@angular/router';
import { Storage } from "../services/storage.service";
import { constants } from "../constants";
import { EventHub } from "../services/event-hub";

@Injectable()
export class EventHubConnectionGuardService  {
    constructor(
        private _eventHub: EventHub,
        private _storage: Storage
    ) { }

    public canActivate(): Promise<boolean> {        
        return new Promise(resolve =>
            this._eventHub.connect().then(() => {
                resolve(true);
            }));    
    }
}