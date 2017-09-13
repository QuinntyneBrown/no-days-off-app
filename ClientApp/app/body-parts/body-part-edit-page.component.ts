import {Component} from "@angular/core";
import {BodyPartsService} from "./body-parts.service";
import {Router,ActivatedRoute} from "@angular/router";
import {guid} from "../shared/utilities/guid";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./body-part-edit-page.component.html",
    styleUrls: ["./body-part-edit-page.component.css"],
    selector: "ce-body-part-edit-page"
})
export class BodyPartEditPageComponent {
    constructor(private _bodyPartsService: BodyPartsService,
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        private _correlationIdsList: CorrelationIdsList
    ) { }

    public async ngOnInit() {
        if (this._activatedRoute.snapshot.params["id"]) {            
            this.bodyPart = (await this._bodyPartsService.getById({ id: this._activatedRoute.snapshot.params["id"] }).toPromise()).bodyPart;
        }
    }

    public tryToSave($event) {
        const correlationId = this._correlationIdsList.newId();
        this._bodyPartsService.addOrUpdate({ bodyPart: $event.detail.bodyPart, correlationId }).subscribe();
        this._router.navigateByUrl("/bodyParts");
    }

    public bodyPart = {};
}
