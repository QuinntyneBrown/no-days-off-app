import { NgModule } from '@angular/core';
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule, FormsModule } from "@angular/forms";
import { SharedModule } from "../shared/shared.module";

import { SetTenantFormComponent } from "./set-tenant-form.component";
import { SetTenantPageComponent } from "./set-tenant-page.component";

const declarables = [SetTenantFormComponent, SetTenantPageComponent];

const providers = [];

export const TENANT_ROUTES = [
    {
        path: 'tenants/set',
        component: SetTenantPageComponent
    }
];

@NgModule({
    imports: [CommonModule, ReactiveFormsModule, FormsModule, SharedModule],
    exports: [declarables],
    declarations: [declarables],
    providers: providers
})
export class TenantsModule { }
