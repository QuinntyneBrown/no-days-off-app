import { Injectable } from "@angular/core";
import { Position } from "./position";
import { translateXY } from "../utilities/translate-xy";
import { createElement } from "../utilities/create-element";
import { Ruler } from "./ruler";
import { Space } from "./space";

export interface IPopoverService { }

@Injectable()
export class PopoverService implements IPopoverService {
    constructor(
        private _position: Position
    ) { }

    private static _instance: PopoverService;

    public static get instance() {        
        this._instance = this._instance || new PopoverService(new Position(new Ruler(), new Space()));
        return this._instance;
    }
    public async show(options: { target: HTMLElement, html:string }): Promise<any> {
        
        if (this._nativeElement)
            return new Promise(resolve => resolve());

        const containerElement = document.querySelector('body');

        this._nativeElement = createElement({ html: options.html });

        this.setInitialCss();

        await this._position.bottomLeft({
            component: this.nativeElement,
            target: options.target,
            space: 0
        });

        containerElement.appendChild(this.nativeElement);
        setTimeout(() => { this.nativeElement.style.opacity = "100"; }, 100);

    }

    public hide(): Promise<any> {
        return new Promise((resolve) => {            
            if (this._nativeElement) {
                this._nativeElement.parentNode.removeChild(this.nativeElement);
                this._nativeElement = null;                
            }
            resolve();
        });
    }

    public get isOpen(): boolean {
        return this._nativeElement != null;
    }
    private setInitialCss() {
        this.nativeElement.setAttribute("style", `-webkit-transition: opacity ${this.transitionDurationInMilliseconds}ms ease-in-out;-o-transition: opacity ${this.transitionDurationInMilliseconds}ms ease-in-out;transition: opacity ${this.transitionDurationInMilliseconds}ms ease-in-out;`);
        this.nativeElement.style.opacity = "0";
        this.nativeElement.style.position = "fixed";
        this.nativeElement.style.top = "0";
        this.nativeElement.style.left = "0";
        this.nativeElement.style.display = "block";
    }

    public transitionDurationInMilliseconds: number;

    private _nativeElement: HTMLElement;

    public get nativeElement(): HTMLElement {
        return this._nativeElement;        
    }

    public templateHTML: string;
}