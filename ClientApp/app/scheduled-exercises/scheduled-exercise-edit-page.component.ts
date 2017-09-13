import {Component} from "@angular/core";
import {ScheduledExercisesService} from "./scheduled-exercises.service";
import {Router,ActivatedRoute} from "@angular/router";
import {guid} from "../shared/utilities/guid";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./scheduled-exercise-edit-page.component.html",
    styleUrls: ["./scheduled-exercise-edit-page.component.css"],
    selector: "ce-scheduled-exercise-edit-page"
})
export class ScheduledExerciseEditPageComponent {
    constructor(private _scheduledExercisesService: ScheduledExercisesService,
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        private _correlationIdsList: CorrelationIdsList
    ) { }

    public async ngOnInit() {
        if (this._activatedRoute.snapshot.params["id"]) {            
            this.scheduledExercise = (await this._scheduledExercisesService.getById({ id: this._activatedRoute.snapshot.params["id"] }).toPromise()).scheduledExercise;
        }
    }

    public tryToSave($event) {
        const correlationId = this._correlationIdsList.newId();
        this._scheduledExercisesService.addOrUpdate({ scheduledExercise: $event.detail.scheduledExercise, correlationId }).subscribe();
        this._router.navigateByUrl("/scheduledExercises");
    }

    public scheduledExercise = {};
}
