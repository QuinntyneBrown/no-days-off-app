import {Component, Input, HostListener} from "@angular/core";
import {BodyPart} from "../body-parts/body-part.model";
import {WeeklyPlannerManager} from "./weekly-planner.manager";

@Component({
    templateUrl: "./weekly-planner-body-part.component.html",
    styleUrls: ["./weekly-planner-body-part.component.css"],
    selector: "ce-weekly-planner-body-part"
})
export class WeeklyPlannerBodyPartComponent {
    constructor(private _weeklyPlannerManager: WeeklyPlannerManager) {

    }

    ngAfterViewInit() {

    }

    ngOnInit() {

    }

    @HostListener("mousedown")
    public mousedown() {

    }

    @Input()
    public bodyPart: BodyPart = <BodyPart>{};
}
