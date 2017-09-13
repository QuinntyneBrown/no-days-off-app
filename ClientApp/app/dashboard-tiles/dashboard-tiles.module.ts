import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
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

import { DashboardTilesService } from "./dashboard-tiles.service";
import { DashboardTilesGuardService } from "./dashboard-tiles-guard.service";

import { DashboardTileEditComponent } from "./dashboard-tile-edit.component";
import { DashboardTileEditPageComponent } from "./dashboard-tile-edit-page.component";
import { DashboardTileListItemComponent } from "./dashboard-tile-list-item.component";
import { DashboardTilePaginatedListComponent } from "./dashboard-tile-paginated-list.component";
import { DashboardTilePaginatedListPageComponent } from "./dashboard-tile-paginated-list-page.component";
import { AddDashboardTileModalComponent } from "./add-dashboard-tile-modal.component";
import { AddDashboardTileModalWindowComponent } from "./add-dashboard-tile-modal-window.component";

import "./dashboard-tile.component";
import "./dashboard-tile-menu.component";
import "./configure-dashboard-tile-modal-window.component";
import "./configure-dashboard-tile-modal.component";
import "./configure-dashboard-tile.component";

const canActivate = [
    TenantGuardService,
    AuthGuardService,
    EventHubConnectionGuardService,
    CurrentUserGuardService    
];

export const DASHBOARD_TILE_ROUTES: Routes = [
    {
        path: 'dashboardTiles',
        component: DashboardTilePaginatedListPageComponent,
        canActivate
    },
    {
        path: 'dashboardTiles/create',
        component: DashboardTileEditPageComponent,
        canActivate
    },
    {
        path: 'dashboardTiles/:id',
        component: DashboardTileEditPageComponent,
        canActivate
    }
];

const declarables = [
    AddDashboardTileModalComponent,
    DashboardTileEditComponent,
    DashboardTileEditPageComponent,
    DashboardTileListItemComponent,
    DashboardTilePaginatedListComponent,
    DashboardTilePaginatedListPageComponent    
];

const providers = [DashboardTilesService, DashboardTilesGuardService];

@NgModule({
    imports: [CommonModule, FormsModule, HttpClientModule, ReactiveFormsModule, RouterModule.forChild(DASHBOARD_TILE_ROUTES), SharedModule, UsersModule],
    exports: [declarables],
    declarations: [declarables],
    providers: providers,
    entryComponents: [   
        
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class DashboardTilesModule { }
