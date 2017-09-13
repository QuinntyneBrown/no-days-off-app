import {Component,Input,Output,EventEmitter} from "@angular/core";

@Component({
    templateUrl: "./dashboard-tile-list-item.component.html",
    styleUrls: [
        "../../styles/list-item.css",
        "./dashboard-tile-list-item.component.css"
    ],
    selector: "ce-dashboard-tile-list-item"
})
export class DashboardTileListItemComponent {  
    constructor() {
        this.edit = new EventEmitter();
        this.delete = new EventEmitter();		
    }
      
    @Input()
    public dashboardTile: any = {};
    
    @Output()
    public edit: EventEmitter<any>;

    @Output()
    public delete: EventEmitter<any>;        
}
