import {Component} from "@angular/core";
import {DaysService} from "./days.service";
import {Router,ActivatedRoute} from "@angular/router";
import {guid} from "../shared/utilities/guid";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./day-edit-page.component.html",
    styleUrls: ["./day-edit-page.component.css"],
    selector: "ce-day-edit-page"
})
export class DayEditPageComponent {
    constructor(private _daysService: DaysService,
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        private _correlationIdsList: CorrelationIdsList
    ) { }

    public async ngOnInit() {
        if (this._activatedRoute.snapshot.params["id"]) {            
            this.day = (await this._daysService.getById({ id: this._activatedRoute.snapshot.params["id"] }).toPromise()).day;
        }
    }

    public tryToSave($event) {
        const correlationId = this._correlationIdsList.newId();
        this._daysService.addOrUpdate({ day: $event.detail.day, correlationId }).subscribe();
        this._router.navigateByUrl("/days");
    }

    public day = {};
}
