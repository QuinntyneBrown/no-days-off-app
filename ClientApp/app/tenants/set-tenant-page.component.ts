import { Component } from "@angular/core";
import { Storage } from "../shared/services/storage.service";
import { constants } from "../shared/constants";
import { LoginRedirectService } from "../shared/services/login-redirect.service";

@Component({
    templateUrl: "./set-tenant-page.component.html",
    styleUrls: ["./set-tenant-page.component.css"],
    selector: "ce-set-tenant-page"
})
export class SetTenantPageComponent {
    constructor(
        private _loginRedirectService: LoginRedirectService,
        private _storage: Storage) { }
    
    tryToSubmit($event) {
        this._storage.put({ name: constants.TENANT, value: $event.detail.tenant.id });
        this._loginRedirectService.redirectPreLogin();
    }
}
