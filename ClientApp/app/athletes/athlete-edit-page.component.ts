import {Component} from "@angular/core";
import {AthletesService} from "./athletes.service";
import {Router,ActivatedRoute} from "@angular/router";
import {guid} from "../shared/utilities/guid";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./athlete-edit-page.component.html",
    styleUrls: ["./athlete-edit-page.component.css"],
    selector: "ce-athlete-edit-page"
})
export class AthleteEditPageComponent {
    constructor(private _athletesService: AthletesService,
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        private _correlationIdsList: CorrelationIdsList
    ) { }

    public async ngOnInit() {
        if (this._activatedRoute.snapshot.params["id"]) {            
            this.athlete = (await this._athletesService.getById({ id: this._activatedRoute.snapshot.params["id"] }).toPromise()).athlete;
        }
    }

    public tryToSave($event) {
        const correlationId = this._correlationIdsList.newId();
        this._athletesService.addOrUpdate({ athlete: $event.detail.athlete, correlationId }).subscribe();
        this._router.navigateByUrl("/athletes");
    }

    public athlete = {};
}
