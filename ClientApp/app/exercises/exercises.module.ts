import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
import { SharedModule } from "../shared/shared.module";
import { UsersModule } from "../users/users.module";
import { BodyPartsModule } from "../body-parts/body-parts.module";
import { AuthGuardService } from "../shared/guards/auth-guard.service";
import { TenantGuardService } from "../shared/guards/tenant-guard.service";
import { EventHubConnectionGuardService } from "../shared/guards/event-hub-connection-guard.service";
import { CurrentUserGuardService } from "../users/current-user-guard.service";

import { ExercisesService } from "./exercises.service";

import { ExerciseEditComponent } from "./exercise-edit.component";
import { ExerciseEditPageComponent } from "./exercise-edit-page.component";
import { ExerciseListItemComponent } from "./exercise-list-item.component";
import { ExercisePaginatedListComponent } from "./exercise-paginated-list.component";
import { ExercisePaginatedListPageComponent } from "./exercise-paginated-list-page.component";

export const EXERCISE_ROUTES: Routes = [{
    path: 'exercises',
    component: ExercisePaginatedListPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
},
{
    path: 'exercises/create',
    component: ExerciseEditPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
},
{
    path: 'exercises/:id',
    component: ExerciseEditPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
}];

const declarables = [
    ExerciseEditComponent,
    ExerciseEditPageComponent,
    ExerciseListItemComponent,
    ExercisePaginatedListComponent,
    ExercisePaginatedListPageComponent
];

const providers = [ExercisesService];

@NgModule({
    imports: [BodyPartsModule, CommonModule, FormsModule, HttpClientModule, ReactiveFormsModule, RouterModule.forChild(EXERCISE_ROUTES), SharedModule, UsersModule],
    exports: [declarables],
    declarations: [declarables],
    providers: providers
})
export class ExercisesModule { }
