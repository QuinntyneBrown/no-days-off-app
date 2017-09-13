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

import { TilesService } from "./tiles.service";
import { TilesGuardService } from "./tiles-guard.service";

import { TileEditComponent } from "./tile-edit.component";
import { TileEditPageComponent } from "./tile-edit-page.component";
import { TileListItemComponent } from "./tile-list-item.component";
import { TilePaginatedListComponent } from "./tile-paginated-list.component";
import { TilePaginatedListPageComponent } from "./tile-paginated-list-page.component";

import { TileCatalogComponent } from "./tile-catalog.component";

// Custom Elements
import "./tile-catalog-item.component";
import "./tile-catalog.component";
import "./tile-catalog-modal-window.component";

export const TILE_ROUTES: Routes = [{
    path: 'tiles',
    component: TilePaginatedListPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
},
{
    path: 'tiles/create',
    component: TileEditPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
},
{
    path: 'tiles/:id',
    component: TileEditPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
}];

const declarables = [
    TileEditComponent,
    TileEditPageComponent,
    TileListItemComponent,
    TilePaginatedListComponent,
    TilePaginatedListPageComponent
];

const providers = [TilesService, TilesGuardService];

@NgModule({
    imports: [CommonModule, FormsModule, HttpClientModule, ReactiveFormsModule, RouterModule.forChild(TILE_ROUTES), SharedModule, UsersModule],
    exports: [declarables],
    declarations: [declarables],
    providers: providers,
    entryComponents: [

    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class TilesModule { }
