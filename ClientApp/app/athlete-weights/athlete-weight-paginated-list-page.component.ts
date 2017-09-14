import {Component, ChangeDetectorRef} from "@angular/core";
import {AthleteWeightsService} from "./athlete-weights.service";
import {Router} from "@angular/router";
import {pluckOut} from "../shared/utilities/pluck-out";
import {EventHub} from "../shared/services/event-hub";
import {Subscription} from "rxjs/Subscription";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./athlete-weight-paginated-list-page.component.html",
    styleUrls: ["./athlete-weight-paginated-list-page.component.css"],
    selector: "ce-athlete-weight-paginated-list-page"   
})
export class AthleteWeightPaginatedListPageComponent {
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _athleteWeightsService: AthleteWeightsService,
        private _correlationIdsList: CorrelationIdsList,
        private _eventHub: EventHub,
        private _router: Router
    ) {
        this.subscription = this._eventHub.events.subscribe(x => {      
            
            if (this._correlationIdsList.hasId(x.payload.correlationId) && x.type == "[AthleteWeights] AthleteWeightAddedOrUpdated") {
                this._athleteWeightsService.get().toPromise().then(x => {
                    this.unfilteredAthleteWeights = x.athleteWeights;
                    this.athleteWeights = this.filterTerm != null ? this.filteredAthleteWeights : this.unfilteredAthleteWeights;
                    this._changeDetectorRef.detectChanges();
                });
            } else if (x.type == "[AthleteWeights] AthleteWeightAddedOrUpdated") {
                
            }
        });      
    }
    
    public async ngOnInit() {
        this.unfilteredAthleteWeights = (await this._athleteWeightsService.get().toPromise()).athleteWeights;   
        this.athleteWeights = this.filterTerm != null ? this.filteredAthleteWeights : this.unfilteredAthleteWeights;       
    }

    public tryToDelete($event) {        
        const correlationId = this._correlationIdsList.newId();

        this.unfilteredAthleteWeights = pluckOut({
            items: this.unfilteredAthleteWeights,
            value: $event.detail.athleteWeight.id
        });

        this.athleteWeights = this.filterTerm != null ? this.filteredAthleteWeights : this.unfilteredAthleteWeights;
        
        this._athleteWeightsService.remove({ athleteWeight: $event.detail.athleteWeight, correlationId }).subscribe();
    }

    public tryToEdit($event) {
        this._router.navigate(["athleteWeights", $event.detail.athleteWeight.id]);
    }

    public handleAthleteWeightsFilterKeyUp($event) {
        this.filterTerm = $event.detail.value;
        this.pageNumber = 1;
        this.athleteWeights = this.filterTerm != null ? this.filteredAthleteWeights : this.unfilteredAthleteWeights;        
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
        this.subscription = null;
    }

    private subscription: Subscription;
    public _athleteWeights: Array<any> = [];
    public filterTerm: string;
    public pageNumber: number;

    public athleteWeights: Array<any> = [];
    public unfilteredAthleteWeights: Array<any> = [];
    public get filteredAthleteWeights() {
        return this.unfilteredAthleteWeights.filter((x) => x.email.indexOf(this.filterTerm) > -1);
    }
}
