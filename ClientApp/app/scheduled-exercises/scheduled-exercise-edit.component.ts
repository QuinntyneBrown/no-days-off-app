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
    templateUrl: "./scheduled-exercise-edit.component.html",
    styleUrls: [
        "../../styles/forms.css",
        "../../styles/edit.css",
        "./scheduled-exercise-edit.component.css"],
    selector: "ce-scheduled-exercise-edit"
})
export class ScheduledExerciseEditComponent {
    constructor() {
        this.tryToSave = new EventEmitter();
    }

    @Output()
    public tryToSave: EventEmitter<any>;

    private _scheduledExercise: any = {};

    @Input("scheduledExercise")
    public set scheduledExercise(value) {
        this._scheduledExercise = value;

        this.form.patchValue({
            id: this._scheduledExercise.id,
            name: this._scheduledExercise.name,
        });
    }
   
    public form = new FormGroup({
        id: new FormControl(0, []),
        name: new FormControl('', [Validators.required])
    });
}
