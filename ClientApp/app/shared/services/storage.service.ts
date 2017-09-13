import {constants} from "../constants";
import {Injectable} from "@angular/core";
import {BehaviorSubject} from "rxjs/BehaviorSubject";

@Injectable()
export class Storage {

    private static _instance;

    public static get instance() {
        this._instance = this._instance || new Storage();
        return this._instance;
    }
    constructor() {
        this.onPageHide = this.onPageHide.bind(this);
        window.addEventListener("pagehide", this.onPageHide);
        this.items$ = new BehaviorSubject<any>(this.items);
    }

    private onPageHide() {
        localStorage.setItem(constants.STORAGE_KEY, JSON.stringify(this._items));        
    }

    public items$: BehaviorSubject<any>;

    private _items = null;

    public get items() {
        if (this._items === null) {
            var storageItems = localStorage.getItem(constants.STORAGE_KEY);
            if (storageItems === "null") {
                storageItems = null;
            }
            this._items = JSON.parse(storageItems || "[]");
        }
        return this._items;
    }

    public set items(value: Array<any>) {
        this._items = value;        
        this.items$.next(this.items);
    }

    public get = (options: { name: string }) => {
        var storageItem = null;
        for (var i = 0; i < this.items.length; i++) {
            if (options.name === this.items[i].name)
                storageItem = this.items[i].value;
        }
        return storageItem;
    }

    public put = (options: { name: string, value: any }) => {
        var itemExists = false;

        this.items.forEach((item: any) => {
            if (options.name === item.name) {
                itemExists = true;
                item.value = options.value
            }
        });

        if (!itemExists) {
            var items = this.items;
            items.push({ name: options.name, value: options.value });
            this.items = items;
            items = null;
        }

        this.items$.next(this.items);
    }

    public clear = () => {
        this._items = [];
    }
}