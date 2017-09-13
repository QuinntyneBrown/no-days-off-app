import {Component,Input, Output, EventEmitter, NgZone} from "@angular/core";
import {toPageListFromInMemory,IPagedList} from "../shared/components/pager.component";
import {Observable} from "rxjs/Observable";
import {BehaviorSubject} from "rxjs/BehaviorSubject";

@Component({
    templateUrl: "./completed-scheduled-exercise-paginated-list.component.html",
    styleUrls: [
        "../../styles/forms.css",
        "../../styles/list.css",
        "./completed-scheduled-exercise-paginated-list.component.css"
    ],
    selector: "ce-completed-scheduled-exercise-paginated-list"
})
export class CompletedScheduledExercisePaginatedListComponent { 
    constructor() {
        this.edit = new EventEmitter();
        this.delete = new EventEmitter();
        this.filterKeyUp = new EventEmitter();
        this.pagedList = toPageListFromInMemory([], this.pageNumber, this.pageSize);
    }

    ngOnInit() {
        this.pagedList = toPageListFromInMemory(this.completedScheduledExercises, this.pageNumber, this.pageSize);
    }

    public setPageNumber($event) {        
        this.pageNumber = $event.detail.pageNumber;
        this.pagedList = toPageListFromInMemory(this.completedScheduledExercises, this.pageNumber, this.pageSize);
    }
    private _completedScheduledExercises = [];

    public get completedScheduledExercises() {
        return this._completedScheduledExercises;
    }
    @Input("completedScheduledExercises")
    public set completedScheduledExercises(value) {        
        this._completedScheduledExercises = value;
        this.pagedList = toPageListFromInMemory(this.completedScheduledExercises, this.pageNumber, this.pageSize);           
    }
    
    public pagedList: IPagedList<any> = <any>{};

    @Output()
    public edit: EventEmitter<any>;

    @Output()
    public delete: EventEmitter<any>;
    
    @Output()
    public filterKeyUp: EventEmitter<any>;
    
    public pageNumber: number = 1;

    public pageSize: number = 5;    
}
