import {Component, NgZone} from "@angular/core";
import {DaysService} from "../days/days.service";
import {Day} from "../days/day.model";
import {BodyPartsService} from "../body-parts/body-parts.service";
import {BodyPart} from "../body-parts/body-part.model";

@Component({
    templateUrl: "./weekly-planner-page.component.html",
    styleUrls: ["./weekly-planner-page.component.css"],
    selector: "ce-weekly-planner-page",
})
export class WeeklyPlannerPageComponent {
    constructor(
        private _bodyPartsService: BodyPartsService,
        private _daysService: DaysService,
        private _ngZone: NgZone
    ) { }

    ngOnInit() {
        this._bodyPartsService.get().subscribe(x => this.bodyParts = x.bodyParts);
        this._daysService.get().subscribe(x => this.days = x.days);
    }

    public days: Array<Day> = [];
    public bodyParts: Array<BodyPart> = [];
}
