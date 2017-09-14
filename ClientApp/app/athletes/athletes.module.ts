import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
import { SharedModule } from "../shared/shared.module";
import { UsersModule } from "../users/users.module";
import { DigitalAssetsModule } from "../digital-assets/digital-assets.module";
import { AuthGuardService } from "../shared/guards/auth-guard.service";
import { TenantGuardService } from "../shared/guards/tenant-guard.service";
import { EventHubConnectionGuardService } from "../shared/guards/event-hub-connection-guard.service";
import { CurrentUserGuardService } from "../users/current-user-guard.service";

import { AthletesService } from "./athletes.service";

import { AthleteEditComponent } from "./athlete-edit.component";
import { AthleteEditPageComponent } from "./athlete-edit-page.component";
import { AthleteListItemComponent } from "./athlete-list-item.component";
import { AthletePageComponent } from "./athlete-page.component";
import { AthletePaginatedListComponent } from "./athlete-paginated-list.component";
import { AthletePaginatedListPageComponent } from "./athlete-paginated-list-page.component";

export const ATHLETE_ROUTES: Routes = [{
    path: 'athletes',
    component: AthletePaginatedListPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
},
{
    path: 'athletes/create',
    component: AthleteEditPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
},
{
    path: 'athletes/:id',
    component: AthleteEditPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
}];

const declarables = [
    AthletePageComponent,
    AthleteEditComponent,
    AthleteEditPageComponent,
    AthleteListItemComponent,
    AthletePaginatedListComponent,
    AthletePaginatedListPageComponent
];

const providers = [AthletesService];

@NgModule({
    imports: [CommonModule, FormsModule, HttpClientModule, ReactiveFormsModule, RouterModule.forChild(ATHLETE_ROUTES), DigitalAssetsModule, SharedModule, UsersModule],
    exports: [declarables],
    declarations: [declarables],
    providers: providers
})
export class AthletesModule { }
