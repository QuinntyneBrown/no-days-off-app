import {Component,Input,Output,EventEmitter} from "@angular/core";

@Component({
    templateUrl: "./completed-scheduled-exercise-list-item.component.html",
    styleUrls: [
        "../../styles/list-item.css",
        "./completed-scheduled-exercise-list-item.component.css"
    ],
    selector: "ce-completed-scheduled-exercise-list-item"
})
export class CompletedScheduledExerciseListItemComponent {  
    constructor() {
        this.edit = new EventEmitter();
        this.delete = new EventEmitter();		
    }
      
    @Input()
    public completedScheduledExercise: any = {};
    
    @Output()
    public edit: EventEmitter<any>;

    @Output()
    public delete: EventEmitter<any>;        
}
