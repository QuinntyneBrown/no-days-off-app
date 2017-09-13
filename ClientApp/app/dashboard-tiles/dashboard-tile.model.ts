import { Tile } from "../tiles/tile.model";

export class DashboardTile { 

    public id?:any;
    
    public name?: string;

    public dashboardId: any;

    public tileId: any;

    public top?: any;

    public left?: any;

    public width?: any;

    public height?: any;

    public tile?: Tile = <Tile>{};
}
