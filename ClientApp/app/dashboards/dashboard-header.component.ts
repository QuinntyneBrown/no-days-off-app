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

import { FormGroup, FormControl, Validators } from "@angular/forms";

import { Dashboard } from "./dashboard.model";

@Component({
    templateUrl: "./dashboard-header.component.html",
    styleUrls: [
        "../../styles/forms.css",
        "./dashboard-header.component.css"
    ],
    selector: "ce-dashboard-header"    
})
export class DashboardHeaderComponent {
    constructor() {
        this.tryToAddDashboard = new EventEmitter();
        this.tryToSaveDashboard = new EventEmitter();
    }

    @Input()
    public dashboards: Array<Dashboard> = [];

    @Input("dashboard")
    public set dashboard(value: Dashboard) {
        this._dashboard = value;
        if(value)
            this.form.patchValue({
                name: value.name
            }); 
    }

    @Output()
    public tryToSaveDashboard: EventEmitter<any>;

    @Output()
    public tryToAddDashboard: EventEmitter<any>;

    public form = new FormGroup({
        name: new FormControl('', [Validators.required])
    });

    private _dashboard: Dashboard;

    public get dashboard():Dashboard { return this._dashboard; }
}
