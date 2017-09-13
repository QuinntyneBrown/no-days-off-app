import { Injectable } from "@angular/core";
import { Rectangle } from "./rectangle";

@Injectable()
export class Space {

    constructor() { }

    public above(spaceNeed: number, rectangle: Rectangle) {
        return false;
    }

    public below(spaceNeed: number, rectangle: Rectangle) {
        return false;
    }

    public left(spaceNeed: number, rectangle: Rectangle) {
        return false;
    }

    public right(spaceNeed: number, rectangle: Rectangle) {
        return false;
    }
}