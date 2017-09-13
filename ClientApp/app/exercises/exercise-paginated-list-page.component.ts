import {Component, ChangeDetectorRef} from "@angular/core";
import {ExercisesService} from "./exercises.service";
import {Router} from "@angular/router";
import {pluckOut} from "../shared/utilities/pluck-out";
import {EventHub} from "../shared/services/event-hub";
import {Subscription} from "rxjs/Subscription";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./exercise-paginated-list-page.component.html",
    styleUrls: ["./exercise-paginated-list-page.component.css"],
    selector: "ce-exercise-paginated-list-page"   
})
export class ExercisePaginatedListPageComponent {
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _exercisesService: ExercisesService,
        private _correlationIdsList: CorrelationIdsList,
        private _eventHub: EventHub,
        private _router: Router
    ) {
        this.subscription = this._eventHub.events.subscribe(x => {      
            
            if (this._correlationIdsList.hasId(x.payload.correlationId) && x.type == "[Exercises] ExerciseAddedOrUpdated") {
                this._exercisesService.get().toPromise().then(x => {
                    this.unfilteredExercises = x.exercises;
                    this.exercises = this.filterTerm != null ? this.filteredExercises : this.unfilteredExercises;
                    this._changeDetectorRef.detectChanges();
                });
            } else if (x.type == "[Exercises] ExerciseAddedOrUpdated") {
                
            }
        });      
    }
    
    public async ngOnInit() {
        this.unfilteredExercises = (await this._exercisesService.get().toPromise()).exercises;   
        this.exercises = this.filterTerm != null ? this.filteredExercises : this.unfilteredExercises;       
    }

    public tryToDelete($event) {        
        const correlationId = this._correlationIdsList.newId();

        this.unfilteredExercises = pluckOut({
            items: this.unfilteredExercises,
            value: $event.detail.exercise.id
        });

        this.exercises = this.filterTerm != null ? this.filteredExercises : this.unfilteredExercises;
        
        this._exercisesService.remove({ exercise: $event.detail.exercise, correlationId }).subscribe();
    }

    public tryToEdit($event) {
        this._router.navigate(["exercises", $event.detail.exercise.id]);
    }

    public handleExercisesFilterKeyUp($event) {
        this.filterTerm = $event.detail.value;
        this.pageNumber = 1;
        this.exercises = this.filterTerm != null ? this.filteredExercises : this.unfilteredExercises;        
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
        this.subscription = null;
    }

    private subscription: Subscription;
    public _exercises: Array<any> = [];
    public filterTerm: string;
    public pageNumber: number;

    public exercises: Array<any> = [];
    public unfilteredExercises: Array<any> = [];
    public get filteredExercises() {
        return this.unfilteredExercises.filter((x) => x.email.indexOf(this.filterTerm) > -1);
    }
}
