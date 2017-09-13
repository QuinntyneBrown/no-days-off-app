import {Component,Input, Output, EventEmitter, NgZone} from "@angular/core";
import {toPageListFromInMemory,IPagedList} from "../shared/components/pager.component";
import {Observable} from "rxjs/Observable";
import {BehaviorSubject} from "rxjs/BehaviorSubject";

@Component({
    templateUrl: "./athlete-paginated-list.component.html",
    styleUrls: [
        "../../styles/forms.css",
        "../../styles/list.css",
        "./athlete-paginated-list.component.css"
    ],
    selector: "ce-athlete-paginated-list"
})
export class AthletePaginatedListComponent { 
    constructor() {
        this.edit = new EventEmitter();
        this.delete = new EventEmitter();
        this.filterKeyUp = new EventEmitter();
        this.pagedList = toPageListFromInMemory([], this.pageNumber, this.pageSize);
    }

    ngOnInit() {
        this.pagedList = toPageListFromInMemory(this.athletes, this.pageNumber, this.pageSize);
    }

    public setPageNumber($event) {        
        this.pageNumber = $event.detail.pageNumber;
        this.pagedList = toPageListFromInMemory(this.athletes, this.pageNumber, this.pageSize);
    }
    private _athletes = [];

    public get athletes() {
        return this._athletes;
    }
    @Input("athletes")
    public set athletes(value) {        
        this._athletes = value;
        this.pagedList = toPageListFromInMemory(this.athletes, this.pageNumber, this.pageSize);           
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
