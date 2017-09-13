import {Component,Input,Output,EventEmitter} from "@angular/core";

@Component({
    templateUrl: "./tile-list-item.component.html",
    styleUrls: [
        "../../styles/list-item.css",
        "./tile-list-item.component.css"
    ],
    selector: "ce-tile-list-item"
})
export class TileListItemComponent {  
    constructor() {
        this.edit = new EventEmitter();
        this.delete = new EventEmitter();		
    }
      
    @Input()
    public tile: any = {};
    
    @Output()
    public edit: EventEmitter<any>;

    @Output()
    public delete: EventEmitter<any>;        
}
