import {Injectable} from "@angular/core";
import {Observable} from "rxjs/Observable";
import {BehaviorSubject} from 'rxjs/BehaviorSubject';
import {constants} from "../constants";
import {Storage} from "./storage.service";
import "rxjs/add/observable/fromPromise";
import "rxjs/add/operator/map";
import {Subject} from "rxjs/Subject";
declare var $: any;

enum connectionState {
    connecting,
    connected,
    disconnected
}

@Injectable()
export class EventHub {
    private _eventHub: any;
    private _connection: any;
    private _connectionState: connectionState = connectionState.disconnected;
    private _connectPromise: Promise<any>;
    public events: Subject<any> = new Subject();
    private static _instance;

    public static get instance() {
        this._instance = this._instance || new EventHub(Storage.instance);
        return this._instance;
    }

    constructor(private _storage: Storage) { } 

    public connect() {        
        if (this._connectPromise)
            return this._connectPromise;

        this._connectPromise = new Promise((resolve) => {
            if (this._connectionState === connectionState.disconnected) {
                this._connection = this._connection || $.hubConnection(constants.HUB_URL);
                this._connection.qs = { "Bearer": this._storage.get({ name: constants.ACCESS_TOKEN_KEY }) };
                this._eventHub = this._connection.createHubProxy("eventHub");                
                this._eventHub.on("events", (value) => this.events.next(value));                                             
                this._connection.start({ transport: 'webSockets' }).done(resolve);
            } else {
                resolve();
            }
        });
        return this._connectPromise;
    }      

    public disconnect() {
        if (this._connection) {
            this._connection.stop();
            this._connectPromise = null;
            this._connectionState = connectionState.disconnected;
        }
    } 
}