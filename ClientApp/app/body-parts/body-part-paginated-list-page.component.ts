import {Component, ChangeDetectorRef} from "@angular/core";
import {BodyPartsService} from "./body-parts.service";
import {Router} from "@angular/router";
import {pluckOut} from "../shared/utilities/pluck-out";
import {EventHub} from "../shared/services/event-hub";
import {Subscription} from "rxjs/Subscription";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./body-part-paginated-list-page.component.html",
    styleUrls: ["./body-part-paginated-list-page.component.css"],
    selector: "ce-body-part-paginated-list-page"   
})
export class BodyPartPaginatedListPageComponent {
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _bodyPartsService: BodyPartsService,
        private _correlationIdsList: CorrelationIdsList,
        private _eventHub: EventHub,
        private _router: Router
    ) {
        this.subscription = this._eventHub.events.subscribe(x => {      
            
            if (this._correlationIdsList.hasId(x.payload.correlationId) && x.type == "[BodyParts] BodyPartAddedOrUpdated") {
                this._bodyPartsService.get().toPromise().then(x => {
                    this.unfilteredBodyParts = x.bodyParts;
                    this.bodyParts = this.filterTerm != null ? this.filteredBodyParts : this.unfilteredBodyParts;
                    this._changeDetectorRef.detectChanges();
                });
            } else if (x.type == "[BodyParts] BodyPartAddedOrUpdated") {
                
            }
        });      
    }
    
    public async ngOnInit() {
        this.unfilteredBodyParts = (await this._bodyPartsService.get().toPromise()).bodyParts;   
        this.bodyParts = this.filterTerm != null ? this.filteredBodyParts : this.unfilteredBodyParts;       
    }

    public tryToDelete($event) {        
        const correlationId = this._correlationIdsList.newId();

        this.unfilteredBodyParts = pluckOut({
            items: this.unfilteredBodyParts,
            value: $event.detail.bodyPart.id
        });

        this.bodyParts = this.filterTerm != null ? this.filteredBodyParts : this.unfilteredBodyParts;
        
        this._bodyPartsService.remove({ bodyPart: $event.detail.bodyPart, correlationId }).subscribe();
    }

    public tryToEdit($event) {
        this._router.navigate(["bodyParts", $event.detail.bodyPart.id]);
    }

    public handleBodyPartsFilterKeyUp($event) {
        this.filterTerm = $event.detail.value;
        this.pageNumber = 1;
        this.bodyParts = this.filterTerm != null ? this.filteredBodyParts : this.unfilteredBodyParts;        
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
        this.subscription = null;
    }

    private subscription: Subscription;
    public _bodyParts: Array<any> = [];
    public filterTerm: string;
    public pageNumber: number;

    public bodyParts: Array<any> = [];
    public unfilteredBodyParts: Array<any> = [];
    public get filteredBodyParts() {
        return this.unfilteredBodyParts.filter((x) => x.email.indexOf(this.filterTerm) > -1);
    }
}
