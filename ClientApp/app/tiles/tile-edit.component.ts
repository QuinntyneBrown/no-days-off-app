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
    templateUrl: "./tile-edit.component.html",
    styleUrls: [
        "../../styles/forms.css",
        "../../styles/edit.css",
        "./tile-edit.component.css"],
    selector: "ce-tile-edit"
})
export class TileEditComponent {
    constructor() {
        this.tryToSave = new EventEmitter();
    }

    @Output()
    public tryToSave: EventEmitter<any>;

    private _tile: any = {};

    @Input("tile")
    public set tile(value) {
        this._tile = value;

        this.form.patchValue({
            id: this._tile.id,
            name: this._tile.name,
        });
    }
   
    public form = new FormGroup({
        id: new FormControl(0, []),
        name: new FormControl('', [Validators.required])
    });
}
