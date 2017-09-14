import {Routes, RouterModule} from '@angular/router';

import { AuthGuardService } from "./shared/guards/auth-guard.service";
import { TenantGuardService } from "./shared/guards/tenant-guard.service";
import { EventHubConnectionGuardService } from "./shared/guards/event-hub-connection-guard.service";
import { CurrentUserGuardService } from "./users/current-user-guard.service";

import { AthletePageComponent } from "./athletes/athlete-page.component";

import {DASHBOARD_TILE_ROUTES} from "./dashboard-tiles/dashboard-tiles.module";
import {DASHBOARD_ROUTES} from "./dashboards/dashboards.module";
import {HOME_ROUTES} from "./home/home.module";
import {USER_ROUTES} from "./users/users.module";
import {TENANT_ROUTES} from "./tenants/tenants.module";
import {TILE_ROUTES} from "./tiles/tiles.module";

const ROUTES = [
    {
        path: 'myprofile',
        component: AthletePageComponent,
        pathMatch: 'full',
        canActivate: [
            TenantGuardService,
            AuthGuardService,
            EventHubConnectionGuardService,
            CurrentUserGuardService
        ]
    },
];

export const RoutingModule = RouterModule.forRoot([
    ...ROUTES,
    ...DASHBOARD_TILE_ROUTES,
    ...DASHBOARD_ROUTES,
    ...TENANT_ROUTES,
    ...TILE_ROUTES,
    ...USER_ROUTES
]);