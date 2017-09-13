import { Router, ActivatedRoute } from "@angular/router";

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

@Component({
    templateUrl: "./set-tenant-form.component.html",
    styleUrls: [
        "../../styles/forms.css",
        "./set-tenant-form.component.css"
    ],
    selector: "ce-set-tenant-form"
})
export class SetTenantFormComponent {
    constructor() {
        this.tryToSubmit = new EventEmitter();
    }

    public form = new FormGroup({
        id: new FormControl("", [Validators.required])
    });

    @Output()
    tryToSubmit: EventEmitter<any>;
}
