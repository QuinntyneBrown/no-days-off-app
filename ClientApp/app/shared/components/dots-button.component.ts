const template = document.createElement("template");
const html = require("./dots-button.component.html");
const css = require("./dots-button.component.css");

export class DotsButtonComponent extends HTMLElement {
    constructor() {
        super();
    }

    async connectedCallback() {        
        template.innerHTML = `<style>${css}</style>${html}`; 

        this.attachShadow({ mode: 'open' });
        this.shadowRoot.appendChild(document.importNode(template.content, true));  
        
        this.style.height = `${0.6875 * Number(this.offsetWidth)}px`;
        if (!this.hasAttribute('role'))
            this.setAttribute('role', 'dotsbutton');
    }

}

customElements.define(`ce-dots-button`,DotsButtonComponent);
