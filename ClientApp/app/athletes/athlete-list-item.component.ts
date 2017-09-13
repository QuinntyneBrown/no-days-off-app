import {Component,Input,Output,EventEmitter} from "@angular/core";

@Component({
    templateUrl: "./athlete-list-item.component.html",
    styleUrls: [
        "../../styles/list-item.css",
        "./athlete-list-item.component.css"
    ],
    selector: "ce-athlete-list-item"
})
export class AthleteListItemComponent {  
    constructor() {
        this.edit = new EventEmitter();
        this.delete = new EventEmitter();		
    }
      
    @Input()
    public athlete: any = {};
    
    @Output()
    public edit: EventEmitter<any>;

    @Output()
    public delete: EventEmitter<any>;        
}
