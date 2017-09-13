import {Component} from "@angular/core";
import {ExercisesService} from "./exercises.service";
import {Router,ActivatedRoute} from "@angular/router";
import {guid} from "../shared/utilities/guid";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./exercise-edit-page.component.html",
    styleUrls: ["./exercise-edit-page.component.css"],
    selector: "ce-exercise-edit-page"
})
export class ExerciseEditPageComponent {
    constructor(private _exercisesService: ExercisesService,
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        private _correlationIdsList: CorrelationIdsList
    ) { }

    public async ngOnInit() {
        if (this._activatedRoute.snapshot.params["id"]) {            
            this.exercise = (await this._exercisesService.getById({ id: this._activatedRoute.snapshot.params["id"] }).toPromise()).exercise;
        }
    }

    public tryToSave($event) {
        const correlationId = this._correlationIdsList.newId();
        this._exercisesService.addOrUpdate({ exercise: $event.detail.exercise, correlationId }).subscribe();
        this._router.navigateByUrl("/exercises");
    }

    public exercise = {};
}
