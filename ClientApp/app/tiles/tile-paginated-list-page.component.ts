import {Component, ChangeDetectorRef} from "@angular/core";
import {TilesService} from "./tiles.service";
import {Router} from "@angular/router";
import {pluckOut} from "../shared/utilities/pluck-out";
import {EventHub} from "../shared/services/event-hub";
import {Subscription} from "rxjs/Subscription";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./tile-paginated-list-page.component.html",
    styleUrls: ["./tile-paginated-list-page.component.css"],
    selector: "ce-tile-paginated-list-page"   
})
export class TilePaginatedListPageComponent {
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _tilesService: TilesService,
        private _correlationIdsList: CorrelationIdsList,
        private _eventHub: EventHub,
        private _router: Router
    ) {
        this.subscription = this._eventHub.events.subscribe(x => {      
            
            if (this._correlationIdsList.hasId(x.payload.correlationId) && x.type == "[Tiles] TileAddedOrUpdated") {
                this._tilesService.get().toPromise().then(x => {
                    this.unfilteredTiles = x.tiles;
                    this.tiles = this.filterTerm != null ? this.filteredTiles : this.unfilteredTiles;
                    this._changeDetectorRef.detectChanges();
                });
            } else if (x.type == "[Tiles] TileAddedOrUpdated") {
                
            }
        });      
    }
    
    public async ngOnInit() {
        this.unfilteredTiles = (await this._tilesService.get().toPromise()).tiles;   
        this.tiles = this.filterTerm != null ? this.filteredTiles : this.unfilteredTiles;       
    }

    public tryToDelete($event) {        
        const correlationId = this._correlationIdsList.newId();

        this.unfilteredTiles = pluckOut({
            items: this.unfilteredTiles,
            value: $event.detail.tile.id
        });

        this.tiles = this.filterTerm != null ? this.filteredTiles : this.unfilteredTiles;
        
        this._tilesService.remove({ tile: $event.detail.tile, correlationId }).subscribe();
    }

    public tryToEdit($event) {
        this._router.navigate(["tiles", $event.detail.tile.id]);
    }

    public handleTilesFilterKeyUp($event) {
        this.filterTerm = $event.detail.value;
        this.pageNumber = 1;
        this.tiles = this.filterTerm != null ? this.filteredTiles : this.unfilteredTiles;        
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
        this.subscription = null;
    }

    private subscription: Subscription;
    public _tiles: Array<any> = [];
    public filterTerm: string;
    public pageNumber: number;

    public tiles: Array<any> = [];
    public unfilteredTiles: Array<any> = [];
    public get filteredTiles() {
        return this.unfilteredTiles.filter((x) => x.email.indexOf(this.filterTerm) > -1);
    }
}
