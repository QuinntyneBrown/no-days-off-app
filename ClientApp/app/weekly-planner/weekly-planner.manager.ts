import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { BodyPart } from "../body-parts/body-part.model";

@Injectable()
export class WeeklyPlannerManager {
    constructor() {

    }

    public selectedBodyPart$: BehaviorSubject<BodyPart> = new BehaviorSubject(null);
}