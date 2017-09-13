import { Storage } from "../shared/services/storage.service";
import { Tile } from "./tile.model";
import { constants } from "../shared/constants";
import { TILE_SELECT_CANCEL } from "./tiles.actions";
declare var System: any;

const template = document.createElement("template");

const html = require("./tile-catalog.component.html");
const css = require("./tile-catalog.component.css");
const formsCss = require("../../styles/forms.css");

export class TileCatalogComponent extends HTMLElement {
    constructor(private _storage:Storage = Storage.instance) {
        super();        
    }

    static get observedAttributes () {
        return ["tiles"];
    }

    async connectedCallback() {        
        template.innerHTML = `<style>${formsCss} ${css}</style>${html}`; 

        this.attachShadow({ mode: 'open' });
        this.shadowRoot.appendChild(document.importNode(template.content, true));  

        if (!this.hasAttribute('role'))
            this.setAttribute('role', 'tilecatalog');

        this._bind();
        this._setEventListeners();
    }

    private async _bind() {
        for (let i = 0; i < this.tiles.length; i++) {
            const item = document.createElement("ce-tile-catalog-item");
            item.setAttribute("tile", JSON.stringify(this.tiles[i]));
            this.tileCatalogItemsHTMLElement.appendChild(item);
        }
    }

    private _setEventListeners() {
        this.cancelButtonElement.addEventListener("click", this.cancel);
    }

    disconnectedCallback() {
        this.cancelButtonElement.removeEventListener("click", this.cancel);
    }

    public cancel() {
        this.dispatchEvent(new CustomEvent(TILE_SELECT_CANCEL, {
            cancelable: true,
            bubbles: true,
            composed: true
        } as CustomEventInit));
    }

    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            default:
                break;
        }
    }

    public get tiles(): Array<Tile> {
        return this._storage.get({ name: constants.TILES });
    }

    public get tileCatalogItemsHTMLElement() {
        return this.shadowRoot.querySelector(".tile-catalog-items");
    }

    public get cancelButtonElement(): HTMLElement { return this.shadowRoot.querySelector("button.cancel") as HTMLElement; }
}

customElements.define(`ce-tile-catalog`,TileCatalogComponent);
