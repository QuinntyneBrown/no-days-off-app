import {Component, ViewEncapsulation} from "@angular/core";
import {ModalService} from "../shared/services/modal.service";

@Component({
    templateUrl: "./add-dashboard-tile-modal-window.component.html",
    styleUrls: ["./add-dashboard-tile-modal-window.component.css"],
    selector: "ce-add-dashboard-tile-modal-window"
})
export class AddDashboardTileModalWindowComponent { 
    constructor(private _modalService: ModalService) {
    }

    public tryToClose() {
        this._modalService.close();
    }
}
