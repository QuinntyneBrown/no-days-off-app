import { Storage } from "../shared/services/storage.service";
import { constants } from "../shared/constants";
import { DashboardTile } from "./dashboard-tile.model";
import { CONFIGURE_DASHBOARD_TILE, REMOVE_DASHBOARD_TILE, REMOVE_DASHBOARD_TILE_MENU_CLICK, CONFIGURE_DASHBOARD_TILE_MENU_CLICK } from "./dashboard-tiles.actions";
import { PopoverService } from "../shared/services/popover.service";
import { customEvents } from "../shared/services/custom-events";

const template = document.createElement("template");

const html = require("./dashboard-tile.component.html");
const css = require("./dashboard-tile.component.css");

export class DashboardTileComponent extends HTMLElement {
    constructor(
        private _storage: Storage = Storage.instance,
        private _popoverService: PopoverService = PopoverService.instance
    ) {
        super();

        this.configure = this.configure.bind(this);
        this.removeTile = this.removeTile.bind(this);
        this.toggleCustomerDashboardTileMenu = this.toggleCustomerDashboardTileMenu.bind(this);
        this.onConfigureDashboardTileMenuClick = this.onConfigureDashboardTileMenuClick.bind(this);
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
            this.setAttribute('role', 'customersdashboardtile');

        this._bind();
        this._setEventListeners();
    }

    private async _bind() {        

        this.titleElement.innerText = this._dashboardTile.tile.name;

        this.setCssCustomProperty({ name: '--grid-column-start', value: this._dashboardTile.left });
        this.setCssCustomProperty({ name: '--grid-row-start', value: this._dashboardTile.top });
        this.setCssCustomProperty({ name: '--grid-column-stop', value: `${this._dashboardTile.left + this._dashboardTile.width}` });
        this.setCssCustomProperty({ name: '--grid-row-stop', value: `${this._dashboardTile.top + this._dashboardTile.height}` });
    }

    public async toggleCustomerDashboardTileMenu() {
        if (this._popoverService.isOpen) {
            this._popoverService.hide();
        } else {
            await this._popoverService.show({
                html: `<ce-dashboard-tile-menu dashboard-tile='${JSON.stringify(this._dashboardTile)}'></ce-customers-dashboard-tile-menu>`,
                target: this.buttonElement
            });
        }
    }
    public configure(customEvent:CustomEvent) {        
        if (customEvent.detail.dashboardTile.id == this._dashboardTile.id) {
            this._popoverService.hide();

            this.dispatchEvent(customEvents.create({
                name: CONFIGURE_DASHBOARD_TILE, detail: { dashboardTile: this._dashboardTile }
            }));
        }
    }

    public removeTile(customEvent: CustomEvent) {
        if (customEvent.detail.dashboardTile.id == this._dashboardTile.id) {
            this._popoverService.hide();

            this.dispatchEvent(customEvents.create({
                name: REMOVE_DASHBOARD_TILE, detail: { dashboardTile: this._dashboardTile }
            }));
        }
    }

    public setCssCustomProperty(options: { name: string, value: string }) {
        this.style.setProperty(options.name, options.value);
    }
    private _setEventListeners() {
        this.buttonElement.addEventListener("click", this.toggleCustomerDashboardTileMenu);

        document.body.addEventListener(CONFIGURE_DASHBOARD_TILE_MENU_CLICK, this.configure);
        document.body.addEventListener(REMOVE_DASHBOARD_TILE_MENU_CLICK, this.removeTile);
    }

    public onConfigureDashboardTileMenuClick(response: CustomEvent) {
        this._popoverService.hide();
    }

    disconnectedCallback() {        
        document.body.removeEventListener(CONFIGURE_DASHBOARD_TILE_MENU_CLICK, this.configure);
        document.body.removeEventListener(REMOVE_DASHBOARD_TILE_MENU_CLICK, this.removeTile);
        this.buttonElement.removeEventListener("click", this.toggleCustomerDashboardTileMenu);
    }

    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            case "dashboard-tile":
                this._dashboardTile = JSON.parse(newValue);
                break;
        }
    }
    public get buttonElement(): HTMLElement {
        return (<HTMLElement>this.shadowRoot.querySelector("ce-dots-button"));
    }
    private _dashboardTile: DashboardTile;
    
    public get titleElement(): HTMLElement { return this.shadowRoot.querySelector("h1") as HTMLElement; }
}

customElements.define(`ce-dashboard-tile`,DashboardTileComponent);
