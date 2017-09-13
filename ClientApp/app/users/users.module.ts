import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { SharedModule } from "../shared/shared.module";
import { TenantGuardService } from "../shared/guards/tenant-guard.service";
import { LoginComponent } from './login.component';
import { LoginPageComponent } from "./login-page.component";
import { CurrentUserGuardService } from "./current-user-guard.service";
import { UsersService } from "./users.service";

const declarables = [LoginComponent, LoginPageComponent];
const providers = [
    CurrentUserGuardService,
    UsersService
];

export const USER_ROUTES = [
    {
        path: 'login',
        component: LoginPageComponent,
        canActivate: [
            TenantGuardService
        ]
    }
];

@NgModule({
    imports: [CommonModule, HttpClientModule, FormsModule, ReactiveFormsModule, SharedModule],
    exports: [declarables],
    declarations: [declarables],
    providers: providers
})
export class UsersModule { }
