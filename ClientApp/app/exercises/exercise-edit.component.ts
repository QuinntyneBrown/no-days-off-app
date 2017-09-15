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

import { BodyPart } from "../body-parts/body-part.model";

@Component({
    templateUrl: "./exercise-edit.component.html",
    styleUrls: [
        "../../styles/forms.css",
        "../../styles/edit.css",
        "./exercise-edit.component.css"],
    selector: "ce-exercise-edit"
})
export class ExerciseEditComponent {
    constructor() {
        this.tryToSave = new EventEmitter();
    }

    @Output()
    public tryToSave: EventEmitter<any>;

    private _exercise: any = {};

    @Input("exercise")
    public set exercise(value) {
        this._exercise = value;

        this.form.patchValue({
            id: this._exercise.id,
            name: this._exercise.name,
            bodyPartId: this._exercise.bodyPartId
        });
    }
   
    public form = new FormGroup({
        id: new FormControl(0, []),
        name: new FormControl('', [Validators.required]),
        bodyPartId: new FormControl('')
    });

    @Input()
    public bodyParts: Array<BodyPart> = [];
}
