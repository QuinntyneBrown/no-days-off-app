import {Component} from '@angular/core';

@Component({
    templateUrl: "./tab-content.component.html",
    styleUrls: ["./tab-content.component.css"],
    selector: "ce-tab-content",
    host: { '[class.is-active]': 'active', 'class': 'tabs__panel' },
})
export class TabContentComponent {
    active: boolean = false;

    activate() { this.active = true; }

    deactivate() { this.active = false; }
}
