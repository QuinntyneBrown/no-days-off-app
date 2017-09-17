import { Component, ElementRef } from "@angular/core";
import { WeeklyPlannerManager } from "./weekly-planner.manager";

@Component({
    templateUrl: "./weekly-planner-day.component.html",
    styleUrls: ["./weekly-planner-day.component.css"],
    selector: "ce-weekly-planner-day"
})
export class WeeklyPlannerDayComponent {
    constructor(private _elementRef: ElementRef, private _weeklyPlannerManager: WeeklyPlannerManager) {
        this.onDragOver = this.onDragOver.bind(this);
        this.onDrop = this.onDrop.bind(this);
    }

    ngAfterViewInit() {

    }

    public onDragOver(e: DragEvent) {
        e.stopPropagation();
        e.preventDefault();
    }

    public onDrop(e: DragEvent) {
        e.stopPropagation();
        e.preventDefault();
    }

    public get nativeElement(): HTMLElement { return this._elementRef.nativeElement as HTMLElement; }
}
