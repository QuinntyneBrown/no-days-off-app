import {NgModule, CUSTOM_ELEMENTS_SCHEMA} from '@angular/core';
import {CommonModule} from "@angular/common";
import {RouterModule} from "@angular/router";
import {HttpClientModule,HTTP_INTERCEPTORS} from "@angular/common/http";

import {AuthenticationService} from "./services/authentication.service";
import {CorrelationIdsList} from "./services/correlation-ids-list";
import {LoginRedirectService} from "./services/login-redirect.service";
import {EventHub} from "./services/event-hub";
import {ModalService} from "./services/modal.service";
import {Storage} from "./services/storage.service";
import {ErrorService} from "./services/error.service";
import {PopoverService} from "./services/popover.service";
import {Ruler} from "./services/ruler";
import {Position} from "./services/position";
import {Space} from "./services/space";

import {AuthGuardService} from "./guards/auth-guard.service"
import {TenantGuardService} from "./guards/tenant-guard.service";
import {EventHubConnectionGuardService} from "./guards/event-hub-connection-guard.service";

import {JwtInterceptor} from "./interceptors/jwt.interceptor";
import {AuthInterceptor} from "./interceptors/auth.interceptor";
import {TenantInterceptor} from "./interceptors/tenant.interceptor";

import {HeaderComponent} from "./components/header.component";
import {SecondaryHeaderComponent} from "./components/secondary-header.component";
import {LeftNavComponent} from "./components/left-nav.component";
import {ModalWindowComponent} from "./components/modal-window.component";
import {PagerComponent} from "./components/pager.component";
import {PlusButtonComponent} from "./components/plus-button.component";
import {TabContentComponent} from "./components/tab-content.component";
import {TabTitleComponent} from "./components/tab-title.component";
import {TabsComponent} from "./components/tabs.component";
import {HamburgerButtonComponent} from "./components/hamburger-button.component";

import "./components/dots-button.component";
import "./components/backdrop.component";

const providers = [
    AuthGuardService,
    AuthenticationService,
    CorrelationIdsList,
    ErrorService,
    LoginRedirectService,  
    Space,
    Ruler,
    Position,
    TenantGuardService,  
    EventHubConnectionGuardService,
    {
        provide: ModalService,
        useFactory: (): ModalService => ModalService.instance,
        deps: []
    }, 
    {
        provide: EventHub,
        useFactory: (): EventHub => EventHub.instance,
        deps: []
    },    
    {
        provide: Storage,
        useFactory: ():Storage => Storage.instance,
        deps:[]
    },
    {
        provide: PopoverService,
        useFactory: (): PopoverService => PopoverService.instance,
        deps: []
    },
    {
        provide: HTTP_INTERCEPTORS,
        useClass: JwtInterceptor,
        multi: true
    },
    {
        provide: HTTP_INTERCEPTORS,
        useClass: AuthInterceptor,
        multi: true
    },
    {
        provide: HTTP_INTERCEPTORS,
        useClass: TenantInterceptor,
        multi: true
    }
];

const declarables = [
    HeaderComponent,
    SecondaryHeaderComponent,
    LeftNavComponent,
    ModalWindowComponent,
    PagerComponent,
    PlusButtonComponent,
    TabContentComponent,
    TabTitleComponent,
    TabsComponent,
    HamburgerButtonComponent
];

@NgModule({
    imports: [CommonModule, RouterModule, HttpClientModule],
    entryComponents: [ModalWindowComponent],
    declarations: [declarables],
    exports:[declarables],
    providers: providers,
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class SharedModule {}