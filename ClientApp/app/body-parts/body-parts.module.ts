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

import { BodyPartsService } from "./body-parts.service";

import { BodyPartEditComponent } from "./body-part-edit.component";
import { BodyPartEditPageComponent } from "./body-part-edit-page.component";
import { BodyPartListItemComponent } from "./body-part-list-item.component";
import { BodyPartPaginatedListComponent } from "./body-part-paginated-list.component";
import { BodyPartPaginatedListPageComponent } from "./body-part-paginated-list-page.component";

export const BODY_PART_ROUTES: Routes = [{
    path: 'bodyParts',
    component: BodyPartPaginatedListPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
},
{
    path: 'bodyParts/create',
    component: BodyPartEditPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
},
{
    path: 'bodyParts/:id',
    component: BodyPartEditPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
}];

const declarables = [
    BodyPartEditComponent,
    BodyPartEditPageComponent,
    BodyPartListItemComponent,
    BodyPartPaginatedListComponent,
    BodyPartPaginatedListPageComponent
];

const providers = [BodyPartsService];

@NgModule({
    imports: [CommonModule, FormsModule, HttpClientModule, ReactiveFormsModule, RouterModule.forChild(BODY_PART_ROUTES), SharedModule, UsersModule],
    exports: [declarables],
    declarations: [declarables],
    providers: providers
})
export class BodyPartsModule { }
