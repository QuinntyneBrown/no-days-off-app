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

import { AthleteWeightsService } from "./athlete-weights.service";

import { AthleteWeightEditComponent } from "./athlete-weight-edit.component";
import { AthleteWeightEditPageComponent } from "./athlete-weight-edit-page.component";
import { AthleteWeightListItemComponent } from "./athlete-weight-list-item.component";
import { AthleteWeightPaginatedListComponent } from "./athlete-weight-paginated-list.component";
import { AthleteWeightPaginatedListPageComponent } from "./athlete-weight-paginated-list-page.component";
import "./athlete-weight-dashboard-tile.component";

export const ATHLETE_WEIGHT_ROUTES: Routes = [{
    path: 'athleteWeights',
    component: AthleteWeightPaginatedListPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
},
{
    path: 'athleteWeights/create',
    component: AthleteWeightEditPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
},
{
    path: 'athleteWeights/:id',
    component: AthleteWeightEditPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
}];

const declarables = [
    AthleteWeightEditComponent,
    AthleteWeightEditPageComponent,
    AthleteWeightListItemComponent,
    AthleteWeightPaginatedListComponent,
    AthleteWeightPaginatedListPageComponent    
];

const providers = [AthleteWeightsService];

@NgModule({
    imports: [CommonModule, FormsModule, HttpClientModule, ReactiveFormsModule, RouterModule.forChild(ATHLETE_WEIGHT_ROUTES), SharedModule, UsersModule],
    exports: [declarables],
    declarations: [declarables],
    providers: providers
})
export class AthleteWeightsModule { }
