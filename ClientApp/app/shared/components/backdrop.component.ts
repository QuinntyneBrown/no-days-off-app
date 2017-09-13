const template = document.createElement("template");
const html = require("./backdrop.component.html");
const css = require("./backdrop.component.css");

export class BackdropComponent extends HTMLElement {
    constructor() {
        super();
    }
    
    connectedCallback() {
        template.innerHTML = `<style>${css}</style>${html}`;

        this.attachShadow({ mode: 'open' });
        this.shadowRoot.appendChild(document.importNode(template.content, true));

        if (!this.hasAttribute('role'))
            this.setAttribute('role', 'backdrop');        
    }
    
}

customElements.define(`ce-backdrop`, BackdropComponent);