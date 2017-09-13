import {Component,Input,Output,EventEmitter} from "@angular/core";

@Component({
    templateUrl: "./exercise-list-item.component.html",
    styleUrls: [
        "../../styles/list-item.css",
        "./exercise-list-item.component.css"
    ],
    selector: "ce-exercise-list-item"
})
export class ExerciseListItemComponent {  
    constructor() {
        this.edit = new EventEmitter();
        this.delete = new EventEmitter();		
    }
      
    @Input()
    public exercise: any = {};
    
    @Output()
    public edit: EventEmitter<any>;

    @Output()
    public delete: EventEmitter<any>;        
}
