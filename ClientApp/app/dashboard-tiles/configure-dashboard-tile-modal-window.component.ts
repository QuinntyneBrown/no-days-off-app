import {
    CONFIGURE_DASHBOARD_TILE_CANCEL_CLICK,
    CONFIGURE_DASHBOARD_TILE_SAVE_CLICK,
    SAVE_DASHBOARD_TILE,
    CONFIGURE_DASHBOARD_TILE_CONFIGURATION_CHANGE
} from "./dashboard-tiles.actions";
import { EventHub } from "../shared/services/event-hub";
import { ModalService } from "../shared/services/modal.service";
import { DashboardTile } from "./dashboard-tile.model";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { ConfigureDashboardTileModalComponent } from "./configure-dashboard-tile-modal.component";
import { ConfigureDashboardTileComponent } from "./configure-dashboard-tile.component";
import { customEvents } from "../shared/services/custom-events";

const template = document.createElement("template");

const html = require("./configure-dashboard-tile-modal-window.component.html");
const css = require("./configure-dashboard-tile-modal-window.component.css");
const modalCss = require("../../styles/modal-window.css");

export class ConfigureDashboardTileModalWindowComponent extends HTMLElement {
    constructor(
        private _eventHub: EventHub = EventHub.instance,
        private _modalService: ModalService = ModalService.instance
    ) {
        super();
        this.saveDashboardTile = this.saveDashboardTile.bind(this);
        this.onConfigurationChange = this.onConfigurationChange.bind(this);
        this.cancel = this.cancel.bind(this);
    }

    static get observedAttributes () {
        return [
            "dashboard-tile"
        ];
    }
    
    async connectedCallback() {
        
        template.innerHTML = `<style>${modalCss} ${css}</style>${html}`; 

        this.attachShadow({ mode: 'open' });
        this.shadowRoot.appendChild(document.importNode(template.content, true));  

        if (!this.hasAttribute('role'))
            this.setAttribute('role', 'configuredashboardtilemodalwindow');

        this._bind();
        this._setEventListeners();
    }
    
    private async _bind() {
        this.dashboardTile$.subscribe(x => {
            this.configureDashboardTileModalElement.setAttribute("dashboard-tile", JSON.stringify(x));
            this.configureDashboardTileElement.setAttribute("dashboard-tile", JSON.stringify(x));
        });
    }

    private _setEventListeners() {        
        this.addEventListener(CONFIGURE_DASHBOARD_TILE_CANCEL_CLICK, this.cancel);
        this.addEventListener(CONFIGURE_DASHBOARD_TILE_SAVE_CLICK, this.saveDashboardTile);
        this.addEventListener(CONFIGURE_DASHBOARD_TILE_CONFIGURATION_CHANGE,this.onConfigurationChange)
    }

    public saveDashboardTile(customEvent: CustomEvent) {                
        this.dispatchEvent(customEvents.create({ name: SAVE_DASHBOARD_TILE, detail: { dashboardTile: customEvent.detail.dashboardTile } }));
        this._modalService.close();        
    }

    public cancel() {        
        this._modalService.close();
    }

    public onConfigurationChange($event) {        
        this.dashboardTile$.next(Object.assign(this.dashboardTile$.value, $event.detail.dashboardTile));
    }

    disconnectedCallback() {
        this.removeEventListener(CONFIGURE_DASHBOARD_TILE_CANCEL_CLICK, this.cancel);
        this.removeEventListener(CONFIGURE_DASHBOARD_TILE_SAVE_CLICK, this.saveDashboardTile);
        this.removeEventListener(CONFIGURE_DASHBOARD_TILE_CONFIGURATION_CHANGE, this.onConfigurationChange);
    }

    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            case "dashboard-tile":                
                this.dashboardTile$.next(JSON.parse(newValue));                
                break;
        }
    }
    
    public dashboardTile$: BehaviorSubject<DashboardTile> = new BehaviorSubject(<DashboardTile>{});
    
    public get configureDashboardTileModalElement() { return this.shadowRoot.querySelector("ce-configure-dashboard-tile-modal") as ConfigureDashboardTileModalComponent; }

    public get configureDashboardTileElement() { return this.shadowRoot.querySelector("ce-configure-dashboard-tile") as ConfigureDashboardTileComponent; }
}

customElements.define(`ce-configure-dashboard-tile-modal-window`, ConfigureDashboardTileModalWindowComponent);