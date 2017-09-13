import { EventHub } from "../shared/services/event-hub";
import { TILE_SELECTED } from "./tiles.actions";

const template = document.createElement("template");

const html = require("./tile-catalog-item.component.html");
const css = require("./tile-catalog-item.component.css");

export class TileCatalogItemComponent extends HTMLElement {
    constructor(private _eventHub: EventHub = EventHub.instance) {
        super();
    }

    static get observedAttributes () {
        return [
            "tile"
        ];
    }

    async connectedCallback() {
        
        template.innerHTML = `<style>${css}</style>${html}`; 

        this.attachShadow({ mode: 'open' });
        this.shadowRoot.appendChild(document.importNode(template.content, true));  

        if (!this.hasAttribute('role'))
            this.setAttribute('role', 'tilecatalogitem');

        this._bind();
        this._setEventListeners();
    }

    private async _bind() {
        this.nameHTMLElement.innerText = this.tile.name;
    }

    private _setEventListeners() {
        this.addEventListener("click", () => {
            this._eventHub.events.next({
                type: TILE_SELECTED,
                payload: {
                    tile: this.tile
                }
            });
        });
    }

    disconnectedCallback() {

    }

    public tile: any;

    public get nameHTMLElement(): HTMLElement {
        return this.shadowRoot.querySelector("#name") as HTMLElement;
    }

    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            case "tile":
                this.tile = JSON.parse(newValue);
                break;
        }
    }
}

customElements.define(`ce-tile-catalog-item`,TileCatalogItemComponent);
