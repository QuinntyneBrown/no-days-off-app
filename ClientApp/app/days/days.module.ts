import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
import { SharedModule } from "../shared/shared.module";
import { UsersModule } from "../users/users.module";

import { AuthGuardService } from "../shared/guards/auth-guard.service";
import { TenantGuardService } from "../shared/guards/tenant-guard.service";
import { EventHubConnectionGuardService } from "../shared/guards/event-hub-connection-guard.service";
import { CurrentUserGuardService } from "../users/current-user-guard.service";

import { DaysService } from "./days.service";

import { DayEditComponent } from "./day-edit.component";
import { DayEditPageComponent } from "./day-edit-page.component";
import { DayListItemComponent } from "./day-list-item.component";
import { DayPaginatedListComponent } from "./day-paginated-list.component";
import { DayPaginatedListPageComponent } from "./day-paginated-list-page.component";

export const DAY_ROUTES: Routes = [{
    path: 'days',
    component: DayPaginatedListPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
},
{
    path: 'days/create',
    component: DayEditPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
},
{
    path: 'days/:id',
    component: DayEditPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
}];

const declarables = [
    DayEditComponent,
    DayEditPageComponent,
    DayListItemComponent,
    DayPaginatedListComponent,
    DayPaginatedListPageComponent
];

const providers = [DaysService];

@NgModule({
    imports: [CommonModule, FormsModule, HttpClientModule, ReactiveFormsModule, RouterModule.forChild(DAY_ROUTES), SharedModule, UsersModule],
    exports: [declarables],
    declarations: [declarables],
    providers: providers
})
export class DaysModule { }
