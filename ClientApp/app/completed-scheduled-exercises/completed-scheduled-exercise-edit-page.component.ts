import {Component} from "@angular/core";
import {CompletedScheduledExercisesService} from "./completed-scheduled-exercises.service";
import {Router,ActivatedRoute} from "@angular/router";
import {guid} from "../shared/utilities/guid";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./completed-scheduled-exercise-edit-page.component.html",
    styleUrls: ["./completed-scheduled-exercise-edit-page.component.css"],
    selector: "ce-completed-scheduled-exercise-edit-page"
})
export class CompletedScheduledExerciseEditPageComponent {
    constructor(private _completedScheduledExercisesService: CompletedScheduledExercisesService,
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        private _correlationIdsList: CorrelationIdsList
    ) { }

    public async ngOnInit() {
        if (this._activatedRoute.snapshot.params["id"]) {            
            this.completedScheduledExercise = (await this._completedScheduledExercisesService.getById({ id: this._activatedRoute.snapshot.params["id"] }).toPromise()).completedScheduledExercise;
        }
    }

    public tryToSave($event) {
        const correlationId = this._correlationIdsList.newId();
        this._completedScheduledExercisesService.addOrUpdate({ completedScheduledExercise: $event.detail.completedScheduledExercise, correlationId }).subscribe();
        this._router.navigateByUrl("/completedScheduledExercises");
    }

    public completedScheduledExercise = {};
}
