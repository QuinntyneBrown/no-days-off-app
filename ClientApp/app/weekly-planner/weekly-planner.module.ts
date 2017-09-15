import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
import { SharedModule } from "../shared/shared.module";
import { UsersModule } from "../users/users.module";
import { DaysModule } from "../days/days.module";
import { BodyPartsModule } from "../body-parts/body-parts.module";

import { AuthGuardService } from "../shared/guards/auth-guard.service";
import { TenantGuardService } from "../shared/guards/tenant-guard.service";
import { EventHubConnectionGuardService } from "../shared/guards/event-hub-connection-guard.service";
import { CurrentUserGuardService } from "../users/current-user-guard.service";

import { WeeklyPlannerPageComponent } from "./weekly-planner-page.component";

export const WEEKLY_PLANNER_ROUTES: Routes = [{
    path: 'weeklyPlanner',
    component: WeeklyPlannerPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
}];

const declarables = [
    WeeklyPlannerPageComponent
];

const providers = [];

@NgModule({
    imports: [BodyPartsModule, CommonModule, DaysModule, FormsModule, HttpClientModule, ReactiveFormsModule, RouterModule.forChild(WEEKLY_PLANNER_ROUTES), SharedModule, UsersModule],
    exports: [declarables],
    declarations: [declarables],
    providers: providers
})
export class WeeklyPlannerModule { }
