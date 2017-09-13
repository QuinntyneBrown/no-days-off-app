import { DashboardTile } from "./dashboard-tile.model";

const template = document.createElement("template");

import { EventHub } from "../shared/services/event-hub";
import { REMOVE_DASHBOARD_TILE_MENU_CLICK, CONFIGURE_DASHBOARD_TILE_MENU_CLICK } from "./dashboard-tiles.actions";

const html = require("./dashboard-tile-menu.component.html");
const css = require("./dashboard-tile-menu.component.css");

export class DashboardTileMenuComponent extends HTMLElement {
    constructor() {
        super();

        this.removeTile = this.removeTile.bind(this);
        this.configure = this.configure.bind(this);
    }

    static get observedAttributes () {
        return [
            "dashboard-tile"
        ];
    }

    async connectedCallback() {
        
        template.innerHTML = `<style>${css}</style>${html}`; 

        this.attachShadow({ mode: 'open' });
        this.shadowRoot.appendChild(document.importNode(template.content, true));  

        if (!this.hasAttribute('role'))
            this.setAttribute('role', 'customersdashboardtilemenu');

        this._bind();
        this._setEventListeners();
    }

    private async _bind() {

    }

    private _setEventListeners() {        
        this.shadowRoot.querySelector("a.remove").addEventListener("click", this.removeTile);
        this.shadowRoot.querySelector("a.configure").addEventListener("click", this.configure);
    }
    
    removeTile() {        
        this.dispatchEvent(new CustomEvent(REMOVE_DASHBOARD_TILE_MENU_CLICK, <CustomEventInit>{
            detail: {
                dashboardTile: this._dashboardTile
            },
            cancelable: true,
            bubbles: true,
            composed: true
        }));
    }

    configure() {                        
        this.dispatchEvent(new CustomEvent(CONFIGURE_DASHBOARD_TILE_MENU_CLICK, <CustomEventInit>{
            detail: {
                dashboardTile: this._dashboardTile
            },
            cancelable: true,
            bubbles: true,
            composed: true
        }));
    }

    disconnectedCallback() {
        this.shadowRoot.querySelector("a.remove").removeEventListener("click", this.removeTile);
        this.shadowRoot.querySelector("a.configure").removeEventListener("click", this.configure);
    }

    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            case "dashboard-tile":
                this._dashboardTile = JSON.parse(newValue);
                break;
        }
    }

    private _dashboardTile: DashboardTile;
}

customElements.define(`ce-dashboard-tile-menu`,DashboardTileMenuComponent);
