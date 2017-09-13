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

import { ScheduledExercisesService } from "./scheduled-exercises.service";

import { ScheduledExerciseEditComponent } from "./scheduled-exercise-edit.component";
import { ScheduledExerciseEditPageComponent } from "./scheduled-exercise-edit-page.component";
import { ScheduledExerciseListItemComponent } from "./scheduled-exercise-list-item.component";
import { ScheduledExercisePaginatedListComponent } from "./scheduled-exercise-paginated-list.component";
import { ScheduledExercisePaginatedListPageComponent } from "./scheduled-exercise-paginated-list-page.component";

export const SCHEDULED_EXERCISE_ROUTES: Routes = [{
    path: 'scheduledExercises',
    component: ScheduledExercisePaginatedListPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
},
{
    path: 'scheduledExercises/create',
    component: ScheduledExerciseEditPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
},
{
    path: 'scheduledExercises/:id',
    component: ScheduledExerciseEditPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
}];

const declarables = [
    ScheduledExerciseEditComponent,
    ScheduledExerciseEditPageComponent,
    ScheduledExerciseListItemComponent,
    ScheduledExercisePaginatedListComponent,
    ScheduledExercisePaginatedListPageComponent
];

const providers = [ScheduledExercisesService];

@NgModule({
    imports: [CommonModule, FormsModule, HttpClientModule, ReactiveFormsModule, RouterModule.forChild(SCHEDULED_EXERCISE_ROUTES), SharedModule, UsersModule],
    exports: [declarables],
    declarations: [declarables],
    providers: providers
})
export class ScheduledExercisesModule { }
