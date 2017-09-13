import { Injectable } from "@angular/core";
import { Rectangle } from "./rectangle";
import { createElement } from "../utilities/create-element";

export interface IRuler {
    measure(element: HTMLElement): Promise<Rectangle>
}

@Injectable()
export class Ruler implements IRuler {
    constructor() {
        this.measure = this.measure.bind(this);
    }

    public measure(element: HTMLElement): Promise<Rectangle> {
        return new Promise((resolve) => {
            if (document.body.contains(element)) {
                
                const result = Rectangle.fromClientRect(element.getBoundingClientRect());
                resolve(result);
            } else {                
                setTimeout(() => {
                    const containerElement = document.querySelector('body');
                    var el = createElement(element.outerHTML);
                    el.setAttribute("dummy", "dummy");
                    containerElement.appendChild(el);
                    const clientRect = element.getBoundingClientRect();
                    el.parentNode.removeChild(el);
                    var result = Rectangle.fromClientRect(clientRect);  
                    el = null;
                    resolve(result);
                }, 0);
            }
        });        
    }
}