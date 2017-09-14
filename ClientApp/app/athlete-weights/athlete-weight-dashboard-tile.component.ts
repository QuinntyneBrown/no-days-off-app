import { Storage } from "../shared/services/storage.service";
import { constants } from "../shared/constants";
import { DashboardTile } from "../dashboard-tiles/dashboard-tile.model";
import { CONFIGURE_DASHBOARD_TILE, REMOVE_DASHBOARD_TILE, REMOVE_DASHBOARD_TILE_MENU_CLICK, CONFIGURE_DASHBOARD_TILE_MENU_CLICK } from "../dashboard-tiles/dashboard-tiles.actions";
import { PopoverService } from "../shared/services/popover.service";
import { customEvents } from "../shared/services/custom-events";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { fetchClient } from "../shared/services/fetch-client";
import { AthleteWeight } from "./athlete-weight.model";

const template = document.createElement("template");

const html = require("./athlete-weight-dashboard-tile.component.html");
const css = [
    require("./athlete-weight-dashboard-tile.component.css"),
    require("../../styles/dashboard-tile.css")
].join(' ');

export class AthleteWeightDashboardTileComponent extends HTMLElement {
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

    static get observedAttributes() {
        return [
            "dashboard-tile"
        ];
    }

    public athleteWeight$: BehaviorSubject<any> = new BehaviorSubject(<AthleteWeight>{});

    public dashboardTile$: BehaviorSubject<DashboardTile> = new BehaviorSubject(<DashboardTile>{});

    async connectedCallback() {

        template.innerHTML = `<style>${css}</style>${html}`;

        this.attachShadow({ mode: 'open' });
        this.shadowRoot.appendChild(document.importNode(template.content, true));

        if (!this.hasAttribute('role'))
            this.setAttribute('role', 'customersdashboardtile');
        
        this.athleteWeight$.next((await fetchClient.get<{ athleteWeight: AthleteWeight }>("api/athleteweights/getcurrent")).athleteWeight);

        this._bind();
        this._setEventListeners();
    }

    
    private async _bind() {

        this.dashboardTile$.subscribe(x => {
            this.titleElement.innerText = x.tile.name;
            this.setCssCustomProperty({ name: '--grid-column-start', value: x.left });
            this.setCssCustomProperty({ name: '--grid-row-start', value: x.top });
            this.setCssCustomProperty({ name: '--grid-column-stop', value: `${x.left + x.width}` });
            this.setCssCustomProperty({ name: '--grid-row-stop', value: `${x.top + x.height}` });
        });

        this.athleteWeight$.subscribe(x => {
            if (x == null) {
                this.textElement.innerHTML = "No weight info yet!";
            } else {
                this.textElement.innerHTML = "";
            }
        });

    }

    public async toggleCustomerDashboardTileMenu() {
        if (this._popoverService.isOpen) {
            this._popoverService.hide();
        } else {
            await this._popoverService.show({
                html: `<ce-dashboard-tile-menu dashboard-tile='${JSON.stringify(this.dashboardTile$.value)}'></ce-customers-dashboard-tile-menu>`,
                target: this.buttonElement
            });
        }
    }
    public configure(customEvent: CustomEvent) {
        if (customEvent.detail.dashboardTile.id == this.dashboardTile$.value.id) {
            this._popoverService.hide();

            this.dispatchEvent(customEvents.create({
                name: CONFIGURE_DASHBOARD_TILE, detail: { dashboardTile: this.dashboardTile$.value }
            }));
        }
    }

    public removeTile(customEvent: CustomEvent) {
        if (customEvent.detail.dashboardTile.id == this.dashboardTile$.value.id) {
            this._popoverService.hide();

            this.dispatchEvent(customEvents.create({
                name: REMOVE_DASHBOARD_TILE, detail: { dashboardTile: this.dashboardTile$.value }
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

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "dashboard-tile":
                this.dashboardTile$.next(JSON.parse(newValue));
                break;
        }
    }
    public get buttonElement(): HTMLElement {
        return (<HTMLElement>this.shadowRoot.querySelector("ce-dots-button"));
    }

    public get titleElement(): HTMLElement { return this.shadowRoot.querySelector("h1") as HTMLElement; }

    public get subTitleElement(): HTMLElement { return this.shadowRoot.querySelector("h2") as HTMLElement; }

    public get textElement(): HTMLElement { return this.shadowRoot.querySelector("p") as HTMLElement; }
}

customElements.define(`ce-athlete-weight-dashboard-tile`, AthleteWeightDashboardTileComponent);
