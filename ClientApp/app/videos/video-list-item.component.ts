import {Component,Input,Output,EventEmitter} from "@angular/core";

@Component({
    templateUrl: "./video-list-item.component.html",
    styleUrls: [
        "../../styles/list-item.css",
        "./video-list-item.component.css"
    ],
    selector: "ce-video-list-item"
})
export class VideoListItemComponent {  
    constructor() {
        this.edit = new EventEmitter();
        this.delete = new EventEmitter();		
    }
      
    @Input()
    public video: any = {};
    
    @Output()
    public edit: EventEmitter<any>;

    @Output()
    public delete: EventEmitter<any>;        
}
