import {Component,Input} from "@angular/core";
import {BodyPart} from "../body-parts/body-part.model";

@Component({
    templateUrl: "./weekly-planner-body-part-list.component.html",
    styleUrls: ["./weekly-planner-body-part-list.component.css"],
    selector: "ce-weekly-planner-body-part-list"
})
export class WeeklyPlannerBodyPartListComponent {

    ngOnInit() {

    }

    @Input()
    public bodyParts: Array<BodyPart> = [];
}
