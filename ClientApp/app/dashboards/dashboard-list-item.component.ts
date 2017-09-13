import {Component,Input,Output,EventEmitter} from "@angular/core";

@Component({
    templateUrl: "./dashboard-list-item.component.html",
    styleUrls: [
        "../../styles/list-item.css",
        "./dashboard-list-item.component.css"
    ],
    selector: "ce-dashboard-list-item"
})
export class DashboardListItemComponent {  
    constructor() {
        this.edit = new EventEmitter();
        this.delete = new EventEmitter();		
    }
      
    @Input()
    public dashboard: any = {};
    
    @Output()
    public edit: EventEmitter<any>;

    @Output()
    public delete: EventEmitter<any>;        
}
