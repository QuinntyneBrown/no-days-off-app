import {Component, NgZone} from "@angular/core";
import {VideosService} from "./videos.service";
import {Router} from "@angular/router";
import {pluckOut} from "../shared/utilities/pluck-out";
import {EventHub} from "../shared/services/event-hub";
import {Subscription} from "rxjs/Subscription";
import {CorrelationIdsList} from "../shared/services/correlation-ids-list";
import {addOrUpdate} from "../shared/utilities/add-or-update";

@Component({
    templateUrl: "./video-paginated-list-page.component.html",
    styleUrls: ["./video-paginated-list-page.component.css"],
    selector: "ce-video-paginated-list-page"   
})
export class VideoPaginatedListPageComponent {
    constructor(
        private _videosService: VideosService,
        private _correlationIdsList: CorrelationIdsList,
        private _eventHub: EventHub,
        private _router: Router,
        private _ngZone: NgZone
    ) {
        this.subscription = this._eventHub.events.subscribe(message => {               
            this._ngZone.run(() => {
                if (this._correlationIdsList.hasId(message.payload.correlationId) && message.type == "[Videos] VideoAddedOrUpdated") {                    
                    this.unfilteredVideos = addOrUpdate({
                        items: this.unfilteredVideos,
                        item: message.payload.entity
                    });
                    this.videos = this.filterTerm != null ? this.filteredVideos : this.unfilteredVideos;
                }
            });            
        });    
    }
    
    public async ngOnInit() {
        this.unfilteredVideos = (await this._videosService.get().toPromise()).videos;   
        this.videos = this.filterTerm != null ? this.filteredVideos : this.unfilteredVideos;       
    }

    public tryToDelete($event) {        
        const correlationId = this._correlationIdsList.newId();

        this.unfilteredVideos = pluckOut({
            items: this.unfilteredVideos,
            value: $event.detail.video.id
        });

        this.videos = this.filterTerm != null ? this.filteredVideos : this.unfilteredVideos;
        
        this._videosService.remove({ video: $event.detail.video, correlationId }).subscribe();
    }

    public tryToEdit($event) {
        this._router.navigate(["videos", $event.detail.video.id]);
    }

    public handleVideosFilterKeyUp($event) {
        this.filterTerm = $event.detail.value;
        this.pageNumber = 1;
        this.videos = this.filterTerm != null ? this.filteredVideos : this.unfilteredVideos;        
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
        this.subscription = null;
    }

    private subscription: Subscription;
    public _videos: Array<any> = [];
    public filterTerm: string;
    public pageNumber: number;

    public videos: Array<any> = [];
    public unfilteredVideos: Array<any> = [];
    public get filteredVideos() {
        return this.unfilteredVideos.filter((x) => x.email.indexOf(this.filterTerm) > -1);
    }
}
