import {Component} from "@angular/core";
import {AthletesService} from "./athletes.service";

@Component({
    templateUrl: "./athlete-page.component.html",
    styleUrls: ["./athlete-page.component.css"],
    selector: "ce-athlete-page"
})
export class AthletePageComponent {
    constructor(private _athletesService: AthletesService) { }

    ngOnInit() {
        this._athletesService.getCurrent().subscribe(x => this.athlete = x.athlete);
    }

    public athlete: any;
}
