import {Component} from "@angular/core";
import {TilesService} from "./tiles.service";
import {Router,ActivatedRoute} from "@angular/router";
import {guid} from "../shared/utilities/guid";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./tile-edit-page.component.html",
    styleUrls: ["./tile-edit-page.component.css"],
    selector: "ce-tile-edit-page"
})
export class TileEditPageComponent {
    constructor(private _tilesService: TilesService,
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        private _correlationIdsList: CorrelationIdsList
    ) { }

    public async ngOnInit() {
        if (this._activatedRoute.snapshot.params["id"]) {            
            this.tile = (await this._tilesService.getById({ id: this._activatedRoute.snapshot.params["id"] }).toPromise()).tile;
        }
    }

    public tryToSave($event) {
        const correlationId = this._correlationIdsList.newId();
        this._tilesService.addOrUpdate({ tile: $event.detail.tile, correlationId }).subscribe();
        this._router.navigateByUrl("/tiles");
    }

    public tile = {};
}
