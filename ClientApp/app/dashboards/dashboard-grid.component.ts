import {
    Component,
    ElementRef,
    Input
} from "@angular/core";

import { DashboardTile } from "../dashboard-tiles/dashboard-tile.model";
import { EventHub } from "../shared/services/event-hub";
import { addOrUpdate } from "../shared/utilities/add-or-update";
import { Storage } from "../shared/services/storage.service";
import { constants } from "../shared/constants";
import { REMOVE_DASHBOARD_TILE } from "../dashboard-tiles/dashboard-tiles.actions";
import { pluckOut } from "../shared/utilities/pluck-out";
import { Subscription } from "rxjs/Subscription";
import { createElement } from "../shared/utilities/create-element";

@Component({
    templateUrl: "./dashboard-grid.component.html",
    styleUrls: ["./dashboard-grid.component.css"],
    selector: "ce-dashboard-grid"
})
export class DashboardGridComponent {
    constructor(
        private _elementRef: ElementRef,        
        private _eventHub: EventHub,
        private _storage: Storage
    ) {
        this.removeDashboardTile = this.removeDashboardTile.bind(this);
    }

    private _subscription: Subscription;

    ngOnInit() {
        this._subscription = this._eventHub.events.subscribe(x => {
            if (x.type == "[DashboardTiles] DashboardTileAddedOrUpdated") {                
                addOrUpdate({
                    items: this._dashboardTiles,
                    item: x.payload.entity
                });

                this._storage.put({ name: constants.DASHBOARD_TILES, value: this._dashboardTiles });
                this._updateGrid();
            }            
        });

        document.body.addEventListener(REMOVE_DASHBOARD_TILE, this.removeDashboardTile);
    }

    ngOnDestroy() {
        document.body.removeEventListener(REMOVE_DASHBOARD_TILE, this.removeDashboardTile);
    }

    public removeDashboardTile(customEvent: CustomEvent) {
        this._dashboardTiles = pluckOut({
            items: this._dashboardTiles,
            value: customEvent.detail.dashboardTile.id
        });
        this._storage.put({ name: constants.DASHBOARD_TILES, value: this._dashboardTiles });
        this._updateGrid();
    }

    ngDestroy() {
        this._subscription.unsubscribe();
        this._subscription = null;
    }

    private _dashboardTiles: Array<DashboardTile> = [];

    @Input("dashboardTiles")
    public set dashboardTiles(value: any) {        
        this._dashboardTiles = value;        
        this._updateGrid();
    }
    
    private _updateGrid() {        
        this._nativeElement.innerHTML = null;                
        for (let i = 0; i < this._dashboardTiles.length; i++) {            
            var el = document.createElement("ce-dashboard-tile");
            el.setAttribute("dashboard-tile", JSON.stringify(this._dashboardTiles[i]));
            this._nativeElement.appendChild(el);
        }
    }

    private get _nativeElement(): HTMLElement { return (<HTMLElement>this._elementRef.nativeElement); }
    
}
