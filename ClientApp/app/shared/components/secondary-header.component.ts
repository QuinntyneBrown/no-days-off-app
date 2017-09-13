import { Component, ApplicationRef, ElementRef, ComponentFactoryResolver } from "@angular/core";

@Component({
    templateUrl: "./secondary-header.component.html",
    styleUrls: ["./secondary-header.component.css"],
    selector: "ce-secondary-header"
})
export class SecondaryHeaderComponent {
    constructor(
        private _applicationRef: ApplicationRef,
        private _componentFactoryResolver: ComponentFactoryResolver,
        private _elementRef: ElementRef
    ) {

    }
}
