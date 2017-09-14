import { NgModule, forwardRef } from "@angular/core";
import { HttpClientModule } from "@angular/common/http";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule, NG_VALUE_ACCESSOR } from "@angular/forms";
import { DigitalAssetsService } from "./digital-assets.service";
import { DigitalAssetInputUrlComponent } from "./digital-asset-input-url.component";

const declarations = [
    DigitalAssetInputUrlComponent
];

const providers = [
    DigitalAssetsService
];

@NgModule({
    imports: [CommonModule, HttpClientModule, FormsModule, ReactiveFormsModule],
    declarations: declarations,
    providers: providers,
    exports: declarations
})
export class DigitalAssetsModule { }