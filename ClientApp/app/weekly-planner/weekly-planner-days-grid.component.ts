import {Component, Input} from "@angular/core";
import {Day} from "../days/day.model";

@Component({
    templateUrl: "./weekly-planner-days-grid.component.html",
    styleUrls: ["./weekly-planner-days-grid.component.css"],
    selector: "ce-weekly-planner-days-grid"
})
export class WeeklyPlannerDaysGridComponent { 

    ngOnInit() {

    }

    @Input()
    public days:Array<Day> = [];
}
