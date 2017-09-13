import {
    Component,
    Input,
    OnInit,
    EventEmitter,
    Output,
    AfterViewInit,
    AfterContentInit,
    Renderer,
    ElementRef,
} from "@angular/core";

import {FormGroup,FormControl,Validators} from "@angular/forms";

@Component({
    templateUrl: "./completed-scheduled-exercise-edit.component.html",
    styleUrls: [
        "../../styles/forms.css",
        "../../styles/edit.css",
        "./completed-scheduled-exercise-edit.component.css"],
    selector: "ce-completed-scheduled-exercise-edit"
})
export class CompletedScheduledExerciseEditComponent {
    constructor() {
        this.tryToSave = new EventEmitter();
    }

    @Output()
    public tryToSave: EventEmitter<any>;

    private _completedScheduledExercise: any = {};

    @Input("completedScheduledExercise")
    public set completedScheduledExercise(value) {
        this._completedScheduledExercise = value;

        this.form.patchValue({
            id: this._completedScheduledExercise.id,
            name: this._completedScheduledExercise.name,
        });
    }
   
    public form = new FormGroup({
        id: new FormControl(0, []),
        name: new FormControl('', [Validators.required])
    });
}
