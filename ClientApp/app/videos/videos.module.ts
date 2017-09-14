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

import { VideosService } from "./videos.service";

import { VideoEditComponent } from "./video-edit.component";
import { VideoEditPageComponent } from "./video-edit-page.component";
import { VideoListItemComponent } from "./video-list-item.component";
import { VideoPaginatedListComponent } from "./video-paginated-list.component";
import { VideoPaginatedListPageComponent } from "./video-paginated-list-page.component";

export const VIDEO_ROUTES: Routes = [{
    path: 'videos',
    component: VideoPaginatedListPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
},
{
    path: 'videos/create',
    component: VideoEditPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
},
{
    path: 'videos/:id',
    component: VideoEditPageComponent,
    canActivate: [
        TenantGuardService,
        AuthGuardService,
        EventHubConnectionGuardService,
        CurrentUserGuardService
    ]
}];

const declarables = [
    VideoEditComponent,
    VideoEditPageComponent,
    VideoListItemComponent,
    VideoPaginatedListComponent,
    VideoPaginatedListPageComponent
];

const providers = [VideosService];

@NgModule({
    imports: [CommonModule, FormsModule, HttpClientModule, ReactiveFormsModule, RouterModule.forChild(VIDEO_ROUTES), SharedModule, UsersModule],
    exports: [declarables],
    declarations: [declarables],
    providers: providers
})
export class VideosModule { }
