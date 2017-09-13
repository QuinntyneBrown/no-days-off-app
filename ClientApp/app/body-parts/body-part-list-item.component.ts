import {Component,Input,Output,EventEmitter} from "@angular/core";

@Component({
    templateUrl: "./body-part-list-item.component.html",
    styleUrls: [
        "../../styles/list-item.css",
        "./body-part-list-item.component.css"
    ],
    selector: "ce-body-part-list-item"
})
export class BodyPartListItemComponent {  
    constructor() {
        this.edit = new EventEmitter();
        this.delete = new EventEmitter();		
    }
      
    @Input()
    public bodyPart: any = {};
    
    @Output()
    public edit: EventEmitter<any>;

    @Output()
    public delete: EventEmitter<any>;        
}
