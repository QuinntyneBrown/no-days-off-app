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

import { CompletedScheduledExercisesService } from "./completed-scheduled-exercises.service";

import { CompletedScheduledExerciseEditComponent } from "./completed-scheduled-exercise-edit.component";
import { CompletedScheduledExerciseEditPageComponent } from "./completed-scheduled-exercise-edit-page.component";
import { CompletedScheduledExerciseListItemComponent } from "./completed-scheduled-exercise-list-item.component";
import { CompletedScheduledExercisePaginatedListComponent } from "./completed-scheduled-exercise-paginated-list.component";
import { CompletedScheduledExercisePaginatedListPageComponent } from "./completed-scheduled-exercise-paginated-list-page.component";

export const COMPLETED_SCHEDULED_EXERCISE_ROUTES: Routes = [{
    path: 'completedScheduledExercises',
    component: CompletedScheduledExercisePaginatedListPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
},
{
    path: 'completedScheduledExercises/create',
    component: CompletedScheduledExerciseEditPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
},
{
    path: 'completedScheduledExercises/:id',
    component: CompletedScheduledExerciseEditPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
}];

const declarables = [
    CompletedScheduledExerciseEditComponent,
    CompletedScheduledExerciseEditPageComponent,
    CompletedScheduledExerciseListItemComponent,
    CompletedScheduledExercisePaginatedListComponent,
    CompletedScheduledExercisePaginatedListPageComponent
];

const providers = [CompletedScheduledExercisesService];

@NgModule({
    imports: [CommonModule, FormsModule, HttpClientModule, ReactiveFormsModule, RouterModule.forChild(COMPLETED_SCHEDULED_EXERCISE_ROUTES), SharedModule, UsersModule],
    exports: [declarables],
    declarations: [declarables],
    providers: providers
})
export class CompletedScheduledExercisesModule { }
