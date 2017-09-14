import {Component,Input, Output, EventEmitter, NgZone} from "@angular/core";
import {toPageListFromInMemory,IPagedList} from "../shared/components/pager.component";
import {Observable} from "rxjs/Observable";
import {BehaviorSubject} from "rxjs/BehaviorSubject";
import {AthleteWeight} from "./athlete-weight.model";

@Component({
    templateUrl: "./athlete-weight-paginated-list.component.html",
    styleUrls: [
        "../../styles/forms.css",
        "../../styles/list.css",
        "./athlete-weight-paginated-list.component.css"
    ],
    selector: "ce-athlete-weight-paginated-list"
})
export class AthleteWeightPaginatedListComponent { 
    constructor() {
        this.edit = new EventEmitter();
        this.delete = new EventEmitter();
        this.filterKeyUp = new EventEmitter();
        this.pagedList = toPageListFromInMemory([], this.pageNumber, this.pageSize);
    }

    ngOnInit() {
        this.athleteWeights$.subscribe(x => this.pagedList = toPageListFromInMemory(x, this.pageNumber, this.pageSize));        
    }

    public setPageNumber($event) {        
        this.pageNumber = $event.detail.pageNumber;
        this.pagedList = toPageListFromInMemory(this.athleteWeights, this.pageNumber, this.pageSize);
    }

    public athleteWeights$: BehaviorSubject<Array<AthleteWeight>> = new BehaviorSubject(<Array<AthleteWeight>>[]);
    
    @Input("athleteWeights")
    public set athleteWeights(value) { this.athleteWeights$.next(value); }
    
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
