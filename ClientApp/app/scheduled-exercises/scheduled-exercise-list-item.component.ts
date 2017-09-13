import {Component,Input,Output,EventEmitter} from "@angular/core";

@Component({
    templateUrl: "./scheduled-exercise-list-item.component.html",
    styleUrls: [
        "../../styles/list-item.css",
        "./scheduled-exercise-list-item.component.css"
    ],
    selector: "ce-scheduled-exercise-list-item"
})
export class ScheduledExerciseListItemComponent {  
    constructor() {
        this.edit = new EventEmitter();
        this.delete = new EventEmitter();		
    }
      
    @Input()
    public scheduledExercise: any = {};
    
    @Output()
    public edit: EventEmitter<any>;

    @Output()
    public delete: EventEmitter<any>;        
}
