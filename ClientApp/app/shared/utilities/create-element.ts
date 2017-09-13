export function createElement(options: { html?:string}): HTMLElement {
    let divElement = document.createElement("div")
    divElement.innerHTML = options.html || "<div></div>";
    return divElement.firstChild as HTMLElement;
}