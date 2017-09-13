const template = document.createElement("template");

const html = require("./configure-dashboard-tile.component.html");
const css = require("./configure-dashboard-tile.component.css");
const formsCss = require("../../styles/forms.css");

import { EventHub } from "../shared/services/event-hub";
import { CONFIGURE_DASHBOARD_TILE_CANCEL_CLICK, CONFIGURE_DASHBOARD_TILE_SAVE_CLICK, CONFIGURE_DASHBOARD_TILE_CONFIGURATION_CHANGE } from "./dashboard-tiles.actions";
import { DashboardTile } from "./dashboard-tile.model";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { customEvents } from "../shared/services/custom-events";

export class ConfigureDashboardTileComponent extends HTMLElement {
    constructor(
        private _eventHub: EventHub = EventHub.instance
    ) {
        super();

        this.save = this.save.bind(this);
        this.cancel = this.cancel.bind(this);
        this.onConfigurationChange = this.onConfigurationChange.bind(this);
    }

    static get observedAttributes () {
        return [
            "dashboard-tile"
        ];
    }

    async connectedCallback() {
    
        template.innerHTML = `<style>${formsCss}${css}</style>${html}`; 

        this.attachShadow({ mode: 'open' });
        this.shadowRoot.appendChild(document.importNode(template.content, true));  

        if (!this.hasAttribute('role'))
            this.setAttribute('role', 'configuredashboardtile');

        this._bind();
        this._setEventListeners();        
    }

    private async _bind() {

        this.dashboardTile$.subscribe(x => {
            this.topInputHTMLElement.value = x.top;
            this.leftInputHTMLElement.value = x.left;
            this.widthInputHTMLElement.value = x.width;
            this.heightInputHTMLElement.value = x.height;
        });
    }

    private _setEventListeners() {
        this.saveButtonHTMLElement.addEventListener("click", this.save);
        this.cancelButtonHTMLElement.addEventListener("click", this.cancel);

        this.topInputHTMLElement.addEventListener("keyup", this.onConfigurationChange);
        this.leftInputHTMLElement.addEventListener("keyup", this.onConfigurationChange);
        this.heightInputHTMLElement.addEventListener("keyup", this.onConfigurationChange);
        this.widthInputHTMLElement.addEventListener("keyup", this.onConfigurationChange);        
    }

    disconnectedCallback() {
        this.saveButtonHTMLElement.removeEventListener("click", this.save);
        this.cancelButtonHTMLElement.removeEventListener("click", this.cancel);

        this.topInputHTMLElement.removeEventListener("keyup", this.onConfigurationChange);
        this.leftInputHTMLElement.removeEventListener("keyup", this.onConfigurationChange);
        this.heightInputHTMLElement.removeEventListener("keyup", this.onConfigurationChange);
        this.widthInputHTMLElement.removeEventListener("keyup", this.onConfigurationChange);
    }

    public onConfigurationChange() {        
        this.dispatchEvent(customEvents.create({
            name: CONFIGURE_DASHBOARD_TILE_CONFIGURATION_CHANGE, detail: {
                dashboardTile: Object.assign(this.dashboardTile$.value, this.toJSON())
            }
        }));
    }

    public save() {                      
        this.dispatchEvent(customEvents.create({
            name: CONFIGURE_DASHBOARD_TILE_SAVE_CLICK, detail: {
                dashboardTile: Object.assign(this.dashboardTile$.value, this.toJSON())
            }
        }));
    }

    public toJSON() {
        return {
            top: this.topInputHTMLElement.value,
            left: this.leftInputHTMLElement.value,
            width: this.widthInputHTMLElement.value,
            height: this.heightInputHTMLElement.value
        };
    }
    public cancel() {                
        this.dispatchEvent(customEvents.create({ name: CONFIGURE_DASHBOARD_TILE_CANCEL_CLICK }));
    }
    
    public dashboardTile$: BehaviorSubject<DashboardTile> = new BehaviorSubject(<DashboardTile>{});

    public get saveButtonHTMLElement(): HTMLElement {
        return (<HTMLElement>this.shadowRoot.querySelector("button.save"));
    }

    public get cancelButtonHTMLElement(): HTMLElement {
        return (<HTMLElement>this.shadowRoot.querySelector("button.cancel"));
    }

    public get topInputHTMLElement(): HTMLInputElement {
        return (<HTMLInputElement>this.shadowRoot.querySelector("#top"));
    }

    public get leftInputHTMLElement(): HTMLInputElement {
        return (<HTMLInputElement>this.shadowRoot.querySelector("#left"));
    }

    public get widthInputHTMLElement(): HTMLInputElement {
        return (<HTMLInputElement>this.shadowRoot.querySelector("#width"));
    }

    public get heightInputHTMLElement(): HTMLInputElement {
        return (<HTMLInputElement>this.shadowRoot.querySelector("#height"));
    }

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "dashboard-tile":
                this.dashboardTile$.next(JSON.parse(newValue));
                break;
        }
    }
}

customElements.define(`ce-configure-dashboard-tile`,ConfigureDashboardTileComponent);
