import {Component, ChangeDetectorRef} from "@angular/core";
import {ScheduledExercisesService} from "./scheduled-exercises.service";
import {Router} from "@angular/router";
import {pluckOut} from "../shared/utilities/pluck-out";
import {EventHub} from "../shared/services/event-hub";
import {Subscription} from "rxjs/Subscription";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./scheduled-exercise-paginated-list-page.component.html",
    styleUrls: ["./scheduled-exercise-paginated-list-page.component.css"],
    selector: "ce-scheduled-exercise-paginated-list-page"   
})
export class ScheduledExercisePaginatedListPageComponent {
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _scheduledExercisesService: ScheduledExercisesService,
        private _correlationIdsList: CorrelationIdsList,
        private _eventHub: EventHub,
        private _router: Router
    ) {
        this.subscription = this._eventHub.events.subscribe(x => {      
            
            if (this._correlationIdsList.hasId(x.payload.correlationId) && x.type == "[ScheduledExercises] ScheduledExerciseAddedOrUpdated") {
                this._scheduledExercisesService.get().toPromise().then(x => {
                    this.unfilteredScheduledExercises = x.scheduledExercises;
                    this.scheduledExercises = this.filterTerm != null ? this.filteredScheduledExercises : this.unfilteredScheduledExercises;
                    this._changeDetectorRef.detectChanges();
                });
            } else if (x.type == "[ScheduledExercises] ScheduledExerciseAddedOrUpdated") {
                
            }
        });      
    }
    
    public async ngOnInit() {
        this.unfilteredScheduledExercises = (await this._scheduledExercisesService.get().toPromise()).scheduledExercises;   
        this.scheduledExercises = this.filterTerm != null ? this.filteredScheduledExercises : this.unfilteredScheduledExercises;       
    }

    public tryToDelete($event) {        
        const correlationId = this._correlationIdsList.newId();

        this.unfilteredScheduledExercises = pluckOut({
            items: this.unfilteredScheduledExercises,
            value: $event.detail.scheduledExercise.id
        });

        this.scheduledExercises = this.filterTerm != null ? this.filteredScheduledExercises : this.unfilteredScheduledExercises;
        
        this._scheduledExercisesService.remove({ scheduledExercise: $event.detail.scheduledExercise, correlationId }).subscribe();
    }

    public tryToEdit($event) {
        this._router.navigate(["scheduledExercises", $event.detail.scheduledExercise.id]);
    }

    public handleScheduledExercisesFilterKeyUp($event) {
        this.filterTerm = $event.detail.value;
        this.pageNumber = 1;
        this.scheduledExercises = this.filterTerm != null ? this.filteredScheduledExercises : this.unfilteredScheduledExercises;        
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
        this.subscription = null;
    }

    private subscription: Subscription;
    public _scheduledExercises: Array<any> = [];
    public filterTerm: string;
    public pageNumber: number;

    public scheduledExercises: Array<any> = [];
    public unfilteredScheduledExercises: Array<any> = [];
    public get filteredScheduledExercises() {
        return this.unfilteredScheduledExercises.filter((x) => x.email.indexOf(this.filterTerm) > -1);
    }
}
