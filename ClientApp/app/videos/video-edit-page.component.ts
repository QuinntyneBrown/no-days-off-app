import {Component} from "@angular/core";
import {VideosService} from "./videos.service";
import {Router,ActivatedRoute} from "@angular/router";
import {guid} from "../shared/utilities/guid";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";

@Component({
    templateUrl: "./video-edit-page.component.html",
    styleUrls: ["./video-edit-page.component.css"],
    selector: "ce-video-edit-page"
})
export class VideoEditPageComponent {
    constructor(private _videosService: VideosService,
        private _router: Router,
        private _activatedRoute: ActivatedRoute,
        private _correlationIdsList: CorrelationIdsList
    ) { }

    public async ngOnInit() {
        if (this._activatedRoute.snapshot.params["id"]) {            
            this.video = (await this._videosService.getById({ id: this._activatedRoute.snapshot.params["id"] }).toPromise()).video;
        }
    }

    public tryToSave($event) {
        const correlationId = this._correlationIdsList.newId();
        this._videosService.addOrUpdate({ video: $event.detail.video, correlationId }).subscribe();
        this._router.navigateByUrl("/videos");
    }

    public video = {};
}
