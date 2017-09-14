import {Component} from "@angular/core";
import { BoundedContextsService } from "./bounded-contexts/bounded-contexts.service";
import { FormGroup, FormControl, Validators } from "@angular/forms";

@Component({
    templateUrl: "./app-left-nav.component.html",
    styleUrls: [
        "../styles/forms.css",        
        "./app-left-nav.component.css"
    ],
    selector: "ce-app-left-nav"
})
export class AppLeftNavComponent {
    constructor(private _boundedContextsService: BoundedContextsService) {
        
    }

    public ngOnInit() {
        this._boundedContextsService.get()
            .subscribe(x => this._boundedContexts = x.boundedContexts);
    }

    private _boundedContexts: Array<any> = [];

    public get boundedContexts() {
        return this._boundedContexts.filter(x => {
            if (this.filterControl.value)
                return x.name.toLowerCase().indexOf(this.filterControl.value.toLowerCase()) > -1;

            return true;
        });
    }
    
    public filterControl: FormControl = new FormControl('');
}
