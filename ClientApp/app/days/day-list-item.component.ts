import {Component,Input,Output,EventEmitter} from "@angular/core";

@Component({
    templateUrl: "./day-list-item.component.html",
    styleUrls: [
        "../../styles/list-item.css",
        "./day-list-item.component.css"
    ],
    selector: "ce-day-list-item"
})
export class DayListItemComponent {  
    constructor() {
        this.edit = new EventEmitter();
        this.delete = new EventEmitter();		
    }
      
    @Input()
    public day: any = {};
    
    @Output()
    public edit: EventEmitter<any>;

    @Output()
    public delete: EventEmitter<any>;        
}
