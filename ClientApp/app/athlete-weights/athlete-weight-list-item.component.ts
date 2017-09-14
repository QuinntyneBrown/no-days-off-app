import {Component,Input,Output,EventEmitter} from "@angular/core";

@Component({
    templateUrl: "./athlete-weight-list-item.component.html",
    styleUrls: [
        "./athlete-weight-list-item.component.css"
    ],
    selector: "ce-athlete-weight-list-item"
})
export class AthleteWeightListItemComponent {  
    constructor() {
        this.edit = new EventEmitter();
        this.delete = new EventEmitter();		
    }
      
    @Input()
    public athleteWeight: any = {};
    
    @Output()
    public edit: EventEmitter<any>;

    @Output()
    public delete: EventEmitter<any>;        
}
