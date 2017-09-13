import {Component, ChangeDetectorRef} from "@angular/core";
import {CompletedScheduledExercisesService} from "./completed-scheduled-exercises.service";
import {Router} from "@angular/router";
import {pluckOut} from "../shared/utilities/pluck-out";
import {EventHub} from "../shared/services/event-hub";
import {Subscription} from "rxjs/Subscription";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./completed-scheduled-exercise-paginated-list-page.component.html",
    styleUrls: ["./completed-scheduled-exercise-paginated-list-page.component.css"],
    selector: "ce-completed-scheduled-exercise-paginated-list-page"   
})
export class CompletedScheduledExercisePaginatedListPageComponent {
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _completedScheduledExercisesService: CompletedScheduledExercisesService,
        private _correlationIdsList: CorrelationIdsList,
        private _eventHub: EventHub,
        private _router: Router
    ) {
        this.subscription = this._eventHub.events.subscribe(x => {      
            
            if (this._correlationIdsList.hasId(x.payload.correlationId) && x.type == "[CompletedScheduledExercises] CompletedScheduledExerciseAddedOrUpdated") {
                this._completedScheduledExercisesService.get().toPromise().then(x => {
                    this.unfilteredCompletedScheduledExercises = x.completedScheduledExercises;
                    this.completedScheduledExercises = this.filterTerm != null ? this.filteredCompletedScheduledExercises : this.unfilteredCompletedScheduledExercises;
                    this._changeDetectorRef.detectChanges();
                });
            } else if (x.type == "[CompletedScheduledExercises] CompletedScheduledExerciseAddedOrUpdated") {
                
            }
        });      
    }
    
    public async ngOnInit() {
        this.unfilteredCompletedScheduledExercises = (await this._completedScheduledExercisesService.get().toPromise()).completedScheduledExercises;   
        this.completedScheduledExercises = this.filterTerm != null ? this.filteredCompletedScheduledExercises : this.unfilteredCompletedScheduledExercises;       
    }

    public tryToDelete($event) {        
        const correlationId = this._correlationIdsList.newId();

        this.unfilteredCompletedScheduledExercises = pluckOut({
            items: this.unfilteredCompletedScheduledExercises,
            value: $event.detail.completedScheduledExercise.id
        });

        this.completedScheduledExercises = this.filterTerm != null ? this.filteredCompletedScheduledExercises : this.unfilteredCompletedScheduledExercises;
        
        this._completedScheduledExercisesService.remove({ completedScheduledExercise: $event.detail.completedScheduledExercise, correlationId }).subscribe();
    }

    public tryToEdit($event) {
        this._router.navigate(["completedScheduledExercises", $event.detail.completedScheduledExercise.id]);
    }

    public handleCompletedScheduledExercisesFilterKeyUp($event) {
        this.filterTerm = $event.detail.value;
        this.pageNumber = 1;
        this.completedScheduledExercises = this.filterTerm != null ? this.filteredCompletedScheduledExercises : this.unfilteredCompletedScheduledExercises;        
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
        this.subscription = null;
    }

    private subscription: Subscription;
    public _completedScheduledExercises: Array<any> = [];
    public filterTerm: string;
    public pageNumber: number;

    public completedScheduledExercises: Array<any> = [];
    public unfilteredCompletedScheduledExercises: Array<any> = [];
    public get filteredCompletedScheduledExercises() {
        return this.unfilteredCompletedScheduledExercises.filter((x) => x.email.indexOf(this.filterTerm) > -1);
    }
}
