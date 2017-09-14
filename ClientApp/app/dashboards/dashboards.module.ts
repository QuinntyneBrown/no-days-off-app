import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
import { DashboardTilesModule } from "../dashboard-tiles/dashboard-tiles.module";
import { SharedModule } from "../shared/shared.module";
import { UsersModule } from "../users/users.module";
import { TilesModule } from "../tiles/tiles.module";
import { AthleteWeightsModule } from "../athlete-weights/athlete-weights.module";

import { TilesGuardService } from "../tiles/tiles-guard.service";
import { DashboardTilesGuardService } from "../dashboard-tiles/dashboard-tiles-guard.service";
import { AuthGuardService } from "../shared/guards/auth-guard.service";
import { TenantGuardService } from "../shared/guards/tenant-guard.service";
import { EventHubConnectionGuardService } from "../shared/guards/event-hub-connection-guard.service";
import { CurrentUserGuardService } from "../users/current-user-guard.service";

import { DashboardsService } from "./dashboards.service";

import { DashboardEditComponent } from "./dashboard-edit.component";
import { DashboardEditPageComponent } from "./dashboard-edit-page.component";
import { DashboardHeaderComponent } from "./dashboard-header.component";
import { DashboardListItemComponent } from "./dashboard-list-item.component";
import { DashboardPaginatedListComponent } from "./dashboard-paginated-list.component";
import { DashboardPaginatedListPageComponent } from "./dashboard-paginated-list-page.component";
import { DashboardPageComponent } from "./dashboard-page.component";
import { DashboardGridComponent } from "./dashboard-grid.component";

const canActivate = [
    TenantGuardService,
    AuthGuardService,
    EventHubConnectionGuardService,
    CurrentUserGuardService,
    TilesGuardService
];

export const DASHBOARD_ROUTES: Routes = [
    {
        path: 'dashboards',
        component: DashboardPaginatedListPageComponent,
        canActivate
    },
{
        path: 'dashboards/create',
        component: DashboardEditPageComponent,
        canActivate
    },
    {
        path: 'dashboards/view/:id',
        component: DashboardPageComponent,
        pathMatch: 'full',
        canActivate: [
            TenantGuardService,
            AuthGuardService,
            EventHubConnectionGuardService,
            CurrentUserGuardService,
            TilesGuardService,
            DashboardTilesGuardService
        ]
    },
    {
        path: 'dashboard',
        component: DashboardPageComponent,
        canActivate: [
            TenantGuardService,
            AuthGuardService,
            EventHubConnectionGuardService,
            CurrentUserGuardService,
            TilesGuardService,
            DashboardTilesGuardService
        ]
    },
    {
        path: 'dashboards/:id',
        component: DashboardEditPageComponent,
        canActivate
    }
];

const declarables = [
    DashboardEditComponent,
    DashboardEditPageComponent,
    DashboardHeaderComponent,
    DashboardListItemComponent,
    DashboardPaginatedListComponent,
    DashboardPaginatedListPageComponent,
    DashboardPageComponent,
    DashboardGridComponent
];

const providers = [DashboardsService];

@NgModule({
    imports: [
        CommonModule,
        DashboardTilesModule,
        FormsModule,
        HttpClientModule,
        ReactiveFormsModule,
        RouterModule.forChild(DASHBOARD_ROUTES),
        SharedModule,
        TilesModule,
        UsersModule,
        AthleteWeightsModule
    ],
    exports: [declarables],
    declarations: [declarables],
    providers: providers
})
export class DashboardsModule { }