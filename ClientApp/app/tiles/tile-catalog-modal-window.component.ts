import { Storage } from "../shared/services/storage.service";
import { constants } from "../shared/constants";
import { createElement } from "../shared/utilities/create-element";
import { EventHub } from "../shared/services/event-hub";
import { TILE_SELECTED, TILE_SELECT_CANCEL } from "./tiles.actions";
import { ModalService } from "../shared/services/modal.service";
import { Inject } from "@angular/core";
const template = document.createElement("template");

const html = require("./tile-catalog-modal-window.component.html");
const css = require("./tile-catalog-modal-window.component.css");

export class TileCatalogModalWindowComponent extends HTMLElement {
    constructor(
        private _eventHub: EventHub = EventHub.instance,
        private _storage: Storage = Storage.instance,
        private _modalService: ModalService = ModalService.instance,

    ) {
        super();
        this.cancel = this.cancel.bind(this);
    }

    static get observedAttributes () {
        return [];
    }

    async connectedCallback() {
        
        template.innerHTML = `<style>${css}</style>${html}`; 
        
        this.attachShadow({ mode: 'open' });
        this.shadowRoot.appendChild(document.importNode(template.content, true));  

        if (!this.hasAttribute('role'))
            this.setAttribute('role', 'tilecatalogmodalwindow');

        this._bind();
        this._setEventListeners();
    }

    private async _bind() {

    }

    private _setEventListeners() {
        var subscription = this._eventHub.events.subscribe(x => {
            if (x.type == TILE_SELECTED) {
                subscription.unsubscribe();
                subscription = null;
                this._modalService.close();
            }
        });

        document.body.addEventListener(TILE_SELECT_CANCEL, this.cancel);
    }

    cancel(customEvent: CustomEvent) {
        this._modalService.close();
    } 

    disconnectedCallback() {
        document.body.removeEventListener(TILE_SELECT_CANCEL, this.cancel);
    }

    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            default:
                break;
        }
    }
}

customElements.define(`ce-tile-catalog-modal-window`,TileCatalogModalWindowComponent);
