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
    templateUrl: "./athlete-edit.component.html",
    styleUrls: [
        "../../styles/forms.css",
        "../../styles/edit.css",
        "./athlete-edit.component.css"],
    selector: "ce-athlete-edit"
})
export class AthleteEditComponent {
    constructor() {
        this.tryToSave = new EventEmitter();
    }

    @Output()
    public tryToSave: EventEmitter<any>;

    private _athlete: any = {};

    @Input("athlete")
    public set athlete(value) {
        this._athlete = value;

        this.form.patchValue({
            id: this._athlete.id,
            name: this._athlete.name,
            imageUrl: this._athlete.imageUrl
        });
    }
   
    public form = new FormGroup({
        id: new FormControl(0, []),
        name: new FormControl('', [Validators.required]),
        imageUrl: new FormControl('')
    });
}
