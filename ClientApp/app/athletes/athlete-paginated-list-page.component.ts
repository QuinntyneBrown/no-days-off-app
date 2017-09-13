import {Component, ChangeDetectorRef} from "@angular/core";
import {AthletesService} from "./athletes.service";
import {Router} from "@angular/router";
import {pluckOut} from "../shared/utilities/pluck-out";
import {EventHub} from "../shared/services/event-hub";
import {Subscription} from "rxjs/Subscription";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./athlete-paginated-list-page.component.html",
    styleUrls: ["./athlete-paginated-list-page.component.css"],
    selector: "ce-athlete-paginated-list-page"   
})
export class AthletePaginatedListPageComponent {
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _athletesService: AthletesService,
        private _correlationIdsList: CorrelationIdsList,
        private _eventHub: EventHub,
        private _router: Router
    ) {
        this.subscription = this._eventHub.events.subscribe(x => {      
            
            if (this._correlationIdsList.hasId(x.payload.correlationId) && x.type == "[Athletes] AthleteAddedOrUpdated") {
                this._athletesService.get().toPromise().then(x => {
                    this.unfilteredAthletes = x.athletes;
                    this.athletes = this.filterTerm != null ? this.filteredAthletes : this.unfilteredAthletes;
                    this._changeDetectorRef.detectChanges();
                });
            } else if (x.type == "[Athletes] AthleteAddedOrUpdated") {
                
            }
        });      
    }
    
    public async ngOnInit() {
        this.unfilteredAthletes = (await this._athletesService.get().toPromise()).athletes;   
        this.athletes = this.filterTerm != null ? this.filteredAthletes : this.unfilteredAthletes;       
    }

    public tryToDelete($event) {        
        const correlationId = this._correlationIdsList.newId();

        this.unfilteredAthletes = pluckOut({
            items: this.unfilteredAthletes,
            value: $event.detail.athlete.id
        });

        this.athletes = this.filterTerm != null ? this.filteredAthletes : this.unfilteredAthletes;
        
        this._athletesService.remove({ athlete: $event.detail.athlete, correlationId }).subscribe();
    }

    public tryToEdit($event) {
        this._router.navigate(["athletes", $event.detail.athlete.id]);
    }

    public handleAthletesFilterKeyUp($event) {
        this.filterTerm = $event.detail.value;
        this.pageNumber = 1;
        this.athletes = this.filterTerm != null ? this.filteredAthletes : this.unfilteredAthletes;        
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
        this.subscription = null;
    }

    private subscription: Subscription;
    public _athletes: Array<any> = [];
    public filterTerm: string;
    public pageNumber: number;

    public athletes: Array<any> = [];
    public unfilteredAthletes: Array<any> = [];
    public get filteredAthletes() {
        return this.unfilteredAthletes.filter((x) => x.email.indexOf(this.filterTerm) > -1);
    }
}
