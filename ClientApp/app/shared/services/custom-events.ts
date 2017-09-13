export class customEvents {
    public static create(options: { name: string, detail?: any }) {
        return new CustomEvent(options.name, {
            detail: options.detail,
            composed: true,
            cancelable: true,
            bubbles: true
        } as CustomEventInit)
    }
}
