import {Component} from "@angular/core";
import {AthleteWeightsService} from "./athlete-weights.service";
import {Router,ActivatedRoute} from "@angular/router";
import {guid} from "../shared/utilities/guid";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./athlete-weight-edit-page.component.html",
    styleUrls: ["./athlete-weight-edit-page.component.css"],
    selector: "ce-athlete-weight-edit-page"
})
export class AthleteWeightEditPageComponent {
    constructor(private _athleteWeightsService: AthleteWeightsService,
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        private _correlationIdsList: CorrelationIdsList
    ) { }

    public async ngOnInit() {
        if (this._activatedRoute.snapshot.params["id"]) {            
            this.athleteWeight = (await this._athleteWeightsService.getById({ id: this._activatedRoute.snapshot.params["id"] }).toPromise()).athleteWeight;
        }
    }

    public tryToSave($event) {
        const correlationId = this._correlationIdsList.newId();
        this._athleteWeightsService.addOrUpdate({ athleteWeight: $event.detail.athleteWeight, correlationId }).subscribe();
        this._router.navigateByUrl("/athleteWeights");
    }

    public athleteWeight = {};
}
