import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
import { SharedModule } from "../shared/shared.module";
import { UsersModule } from "../users/users.module";
import { BodyPartsModule } from "../body-parts/body-parts.module";
import { DaysModule } from "../days/days.module";

import { AuthGuardService } from "../shared/guards/auth-guard.service";
import { TenantGuardService } from "../shared/guards/tenant-guard.service";
import { EventHubConnectionGuardService } from "../shared/guards/event-hub-connection-guard.service";
import { CurrentUserGuardService } from "../users/current-user-guard.service";

import { BodyPartDayAssignComponent } from "./body-part-day-assign.component";
import { BodyPartDayAssignPageComponent } from "./body-part-day-assign-page.component";
import { BodyPartDaysService } from "./body-part-days.service";

export const BODY_PART_DAY_ROUTES: Routes = [{
    path: 'bodyPartDays/assign',
    component: BodyPartDayAssignPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
}];

const declarables = [
    BodyPartDayAssignComponent,
    BodyPartDayAssignPageComponent
];

const providers = [
    BodyPartDaysService
];

@NgModule({
    imports: [BodyPartsModule, CommonModule, DaysModule, FormsModule, HttpClientModule, ReactiveFormsModule, RouterModule.forChild(BODY_PART_DAY_ROUTES), SharedModule, UsersModule],
    exports: [declarables],
    declarations: [declarables],
    providers: providers
})
export class BodyPartDaysModule { }
