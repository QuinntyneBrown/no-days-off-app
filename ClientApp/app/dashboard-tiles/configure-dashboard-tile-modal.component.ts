import { BehaviorSubject } from "rxjs/BehaviorSubject";
const template = document.createElement("template");
const html = require("./configure-dashboard-tile-modal.component.html");
const css = require("./configure-dashboard-tile-modal.component.css");

import { DashboardTile } from "./dashboard-tile.model";

export class ConfigureDashboardTileModalComponent extends HTMLElement {
    constructor() {
        super();
    }

    public dashboardTile$: BehaviorSubject<DashboardTile> = new BehaviorSubject(<DashboardTile>{});
    
    static get observedAttributes() {
        return [
            "dashboard-tile"
        ];
    }

    async connectedCallback() {
        template.innerHTML = `<style>${css}</style>${html}`;

        this.attachShadow({ mode: 'open' });
        this.shadowRoot.appendChild(document.importNode(template.content, true));

        if (!this.hasAttribute('role'))
            this.setAttribute('role', 'configuredashboardtilemodal');

        this._bind();
    }

    private async _bind() {
        this.dashboardTile$.subscribe(x => {
            this.dashboardTileElement.style.height = `${190 * x.height + ((x.height - 1) * 20)}px`;
            this.dashboardTileElement.style.width = `${120 * x.width + ((x.width - 1) * 20)}px`;
        });
    }

    public get columnWidth() { return 120; }

    public get columnHeight() { return 190; }

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "dashboard-tile":
                this.dashboardTile$.next(JSON.parse(newValue));
                break;
        }
    }

    public get dashboardTileElement(): HTMLElement { return this.shadowRoot.querySelector(".dashboard-tile-container") as HTMLElement; }
}

customElements.define(`ce-configure-dashboard-tile-modal`, ConfigureDashboardTileModalComponent);
