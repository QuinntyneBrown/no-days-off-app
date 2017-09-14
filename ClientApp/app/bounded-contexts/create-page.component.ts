import {Component} from "@angular/core";
import {BoundedContextsService} from "./bounded-contexts.service";

@Component({
    templateUrl: "./create-page.component.html",
    styleUrls: ["./create-page.component.css"],
    selector: "ce-create-page"
})
export class CreatePageComponent {
    constructor(private _boundedContextsService: BoundedContextsService) {

    }

    ngOnInit() {
        this._boundedContextsService.get().subscribe(x => this.boundedContexts = x.boundedContexts);
    }

    public boundedContexts: Array<any> = [];
}
