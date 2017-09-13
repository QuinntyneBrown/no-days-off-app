import { Injectable } from "@angular/core";
import { Space } from "./space";
import { Rectangle } from "./rectangle";
import { Ruler } from "./ruler";
import { translateXY } from "../utilities/translate-xy";

@Injectable()
export class Position {
    constructor(
        private _ruler: Ruler,
        private _space: Space
    ) { }

    public somewhere = (a: HTMLElement, b: HTMLElement, space: number, directionPriorityList: Array<string>) => {
        return new Promise(() => {
            throw new Error("");
        });
    }

    public top(a: HTMLElement, b: HTMLElement, space: number): Promise<any> {
        return new Promise(resolve => {
            Promise.all([this._ruler.measure(a), this._ruler.measure(b)])
                .then((resultsArray: Array<Rectangle>) => {
                    var rectangleA = resultsArray[0];
                    var rectangleB = resultsArray[1];
                    translateXY(b, rectangleA.centerX - rectangleB.radiusX, rectangleA.bottom + space);
                    resolve();
                });            
        });
    }

    public right(a: HTMLElement, b: HTMLElement, space: number): Promise<any> {
        return new Promise(resolve => {
            Promise.all([this._ruler.measure(a), this._ruler.measure(b)])
                .then((resultsArray: Array<Rectangle>) => {
                    resolve();
                });
        });
    }

    public bottom(options: { component: HTMLElement, target: HTMLElement, space: number}): Promise<any> {
        return new Promise(resolve => {
            Promise.all([this._ruler.measure(options.target), this._ruler.measure(options.component)])
                .then((resultsArray: Array<Rectangle>) => {                    
                    const targetRectangle = resultsArray[0];
                    const componentRectangle = resultsArray[1];
                    translateXY(options.component, targetRectangle.centerX - componentRectangle.radiusX, targetRectangle.bottom + options.space);
                    resolve();
                });  
        });
    }

    public bottomLeft(options: { component: HTMLElement, target: HTMLElement, space: number }): Promise<any> {
        return new Promise(resolve => {
            Promise.all([this._ruler.measure(options.target), this._ruler.measure(options.component)])
                .then((resultsArray: Array<Rectangle>) => {
                    const targetRectangle = resultsArray[0];
                    const componentRectangle = resultsArray[1];
                    translateXY(options.component, targetRectangle.left, targetRectangle.bottom + options.space);
                    resolve();
                });
        });
    }
    public left(a: HTMLElement, b: HTMLElement, space: number): Promise<any> {
        return new Promise(resolve => {
            Promise.all([this._ruler.measure(a), this._ruler.measure(b)])
                .then((resultsArray: Array<Rectangle>) => {
                    resolve();
                });
        });
    }
}