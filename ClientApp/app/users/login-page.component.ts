import {Component, OnInit} from "@angular/core";
import {AuthenticationService,LoginRedirectService,constants,Storage} from "../shared";

@Component({
    templateUrl: "./login-page.component.html",
    styleUrls: ["./login-page.component.css"],
    selector: "ce-login-page"
})
export class LoginPageComponent implements OnInit {
    constructor(
        private _authenticationService: AuthenticationService,
        private _loginRedirectService: LoginRedirectService,
        private _storage: Storage
    ) { }

    public ngOnInit() {

        this._storage.put({ name: constants.ACCESS_TOKEN_KEY, value: null });

        const loginCredentials = this._storage.get({ name: constants.LOGIN_CREDENTIALS });

        if (loginCredentials && loginCredentials.rememberMe) {                      
            this.username = loginCredentials.username;
            this.password = loginCredentials.password;
            this.rememberMe = loginCredentials.rememberMe;
        }
    }

    public username: string = "";

    public password: string = "";

    public rememberMe: boolean = false;

    public get teamName() { return this._storage.get({ name: constants.CURRENT_TEAM_KEY  }).team.name; }

    public async tryToLogin($event: { value: { username: string, password: string, rememberMe: boolean } }) {      
        
        this._storage.put({ name: constants.LOGIN_CREDENTIALS, value: $event.value.rememberMe ? $event.value : null });

        await this._authenticationService.tryToLogin({ username: $event.value.username, password: $event.value.password }).toPromise();

        this._loginRedirectService.redirectPreLogin();
    }
}