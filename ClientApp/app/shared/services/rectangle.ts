export interface IPoint {

}

export class Rectangle {
    constructor() { }

    public static fromClientRect(clientRect: ClientRect): Rectangle {
        let rectangle = new Rectangle();
        rectangle.left = clientRect.left;
        rectangle.top = clientRect.top;
        rectangle.height = clientRect.height;
        rectangle.width = clientRect.width;
        return rectangle;
    }

    public static fromaJSON(data: { left:any, top: any, height:any, width:any }): Rectangle {
        let rectangle = new Rectangle();
        rectangle.left = data.left;
        rectangle.top = data.top;
        rectangle.height = data.height;
        rectangle.width = data.width;
        return rectangle;
    }

    public left: number;

    public get right(): number { return this.left + this.width; }

    public top: number;

    public get bottom(): number { return this.top + this.height; }

    public height: number;

    public width: number;

    public get centerX(): number { return this.left + (this.width / 2); }

    public get centerY(): number { return this.top + (this.height / 2); }

    public get radiusX(): number { return this.width / 2; }

    public get radiusY(): number { return this.height / 2; }

    public get middle(): IPoint { return { x: this.centerX, y: this.centerY }; }
}