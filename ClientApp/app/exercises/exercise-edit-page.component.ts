import {Component} from "@angular/core";
import {ExercisesService} from "./exercises.service";
import {Router,ActivatedRoute} from "@angular/router";
import {guid} from "../shared/utilities/guid";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";
import {BodyPartsService} from "../body-parts/body-parts.service";

@Component({
    templateUrl: "./exercise-edit-page.component.html",
    styleUrls: ["./exercise-edit-page.component.css"],
    selector: "ce-exercise-edit-page"
})
export class ExerciseEditPageComponent {
    constructor(private _exercisesService: ExercisesService,
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        private _bodyPartsService: BodyPartsService,
        private _correlationIdsList: CorrelationIdsList
    ) {

    }

    public ngOnInit() {        
        if (this._activatedRoute.snapshot.params["id"]) {            
            this._exercisesService.getById({ id: this._activatedRoute.snapshot.params["id"] }).subscribe(x => this.exercise = x.exercise);            
        }

        this._bodyPartsService.get().subscribe(x => this.bodyParts = x.bodyParts);
    }


    public bodyParts: Array<any> = [];

    public tryToSave($event) {
        const correlationId = this._correlationIdsList.newId();
        this._exercisesService.addOrUpdate({ exercise: $event.detail.exercise, correlationId }).subscribe();
        this._router.navigateByUrl("/exercises");
    }

    public exercise = {};
}
