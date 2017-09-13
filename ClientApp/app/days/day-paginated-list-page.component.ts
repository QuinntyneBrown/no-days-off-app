import {Component, ChangeDetectorRef} from "@angular/core";
import {DaysService} from "./days.service";
import {Router} from "@angular/router";
import {pluckOut} from "../shared/utilities/pluck-out";
import {EventHub} from "../shared/services/event-hub";
import {Subscription} from "rxjs/Subscription";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./day-paginated-list-page.component.html",
    styleUrls: ["./day-paginated-list-page.component.css"],
    selector: "ce-day-paginated-list-page"   
})
export class DayPaginatedListPageComponent {
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _daysService: DaysService,
        private _correlationIdsList: CorrelationIdsList,
        private _eventHub: EventHub,
        private _router: Router
    ) {
        this.subscription = this._eventHub.events.subscribe(x => {      
            
            if (this._correlationIdsList.hasId(x.payload.correlationId) && x.type == "[Days] DayAddedOrUpdated") {
                this._daysService.get().toPromise().then(x => {
                    this.unfilteredDays = x.days;
                    this.days = this.filterTerm != null ? this.filteredDays : this.unfilteredDays;
                    this._changeDetectorRef.detectChanges();
                });
            } else if (x.type == "[Days] DayAddedOrUpdated") {
                
            }
        });      
    }
    
    public async ngOnInit() {
        this.unfilteredDays = (await this._daysService.get().toPromise()).days;   
        this.days = this.filterTerm != null ? this.filteredDays : this.unfilteredDays;       
    }

    public tryToDelete($event) {        
        const correlationId = this._correlationIdsList.newId();

        this.unfilteredDays = pluckOut({
            items: this.unfilteredDays,
            value: $event.detail.day.id
        });

        this.days = this.filterTerm != null ? this.filteredDays : this.unfilteredDays;
        
        this._daysService.remove({ day: $event.detail.day, correlationId }).subscribe();
    }

    public tryToEdit($event) {
        this._router.navigate(["days", $event.detail.day.id]);
    }

    public handleDaysFilterKeyUp($event) {
        this.filterTerm = $event.detail.value;
        this.pageNumber = 1;
        this.days = this.filterTerm != null ? this.filteredDays : this.unfilteredDays;        
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
        this.subscription = null;
    }

    private subscription: Subscription;
    public _days: Array<any> = [];
    public filterTerm: string;
    public pageNumber: number;

    public days: Array<any> = [];
    public unfilteredDays: Array<any> = [];
    public get filteredDays() {
        return this.unfilteredDays.filter((x) => x.email.indexOf(this.filterTerm) > -1);
    }
}
