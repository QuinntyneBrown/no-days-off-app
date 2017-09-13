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
    templateUrl: "./dashboard-tile-edit.component.html",
    styleUrls: [
        "../../styles/forms.css",
        "../../styles/edit.css",
        "./dashboard-tile-edit.component.css"],
    selector: "ce-dashboard-tile-edit"
})
export class DashboardTileEditComponent {
    constructor() {
        this.tryToSave = new EventEmitter();
    }

    @Output()
    public tryToSave: EventEmitter<any>;

    private _dashboardTile: any = {};

    @Input("dashboardTile")
    public set dashboardTile(value) {
        this._dashboardTile = value;

        this.form.patchValue({
            id: this._dashboardTile.id,
            name: this._dashboardTile.name,
        });
    }
   
    public form = new FormGroup({
        id: new FormControl(0, []),
        name: new FormControl('', [Validators.required])
    });
}
