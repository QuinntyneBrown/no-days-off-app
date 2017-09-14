import {Component, ElementRef, AfterViewInit, Input, forwardRef} from "@angular/core";
import { DigitalAssetsService } from "./digital-assets.service";
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from "@angular/forms";

@Component({
    templateUrl: "./digital-asset-input-url.component.html",
    styleUrls: [
        "../../styles/forms.css",
        "./digital-asset-input-url.component.css"
    ],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => DigitalAssetInputUrlComponent),
            multi: true
        }
    ],
    selector: "ce-digital-asset-input-url"
})
export class DigitalAssetInputUrlComponent implements ControlValueAccessor {
    constructor(private _elementRef: ElementRef, private _digitalAssetsService: DigitalAssetsService) {
        this.onDragOver = this.onDragOver.bind(this);
        this.onDrop = this.onDrop.bind(this);
    }

    public get value() { return this.inputElement.value; }
    
    public writeValue(value: any): void { this.inputElement.value = value; }

    public registerOnChange(fn: any): void { this.onChangeCallback = fn; }

    public registerOnTouched(fn: any): void { this.onTouchedCallback = fn; }

    public setDisabledState?(isDisabled: boolean): void { this.inputElement.disabled = isDisabled; }

    public onTouchedCallback: () => void = () => { };

    public onChangeCallback: (_: any) => void = () => { };
  
    public ngAfterViewInit() {        
        this._elementRef.nativeElement.addEventListener("dragover", this.onDragOver);
        this._elementRef.nativeElement.addEventListener("drop", this.onDrop, false);
    }

    public ngOnDestroy() {
        this._elementRef.nativeElement.removeEventListener("dragover", this.onDragOver);
        this._elementRef.nativeElement.removeEventListener("drop", this.onDrop, false);
    }

    public onDragOver(e: DragEvent) {
        e.stopPropagation();
        e.preventDefault();
    }

    public async onDrop(e: DragEvent) {
        e.stopPropagation();
        e.preventDefault();

        if (e.dataTransfer && e.dataTransfer.files) {
            const packageFiles = function (fileList: FileList) {
                let formData = new FormData();
                for (var i = 0; i < fileList.length; i++) {
                    formData.append(fileList[i].name, fileList[i]);
                }
                return formData;
            }

            const data = packageFiles(e.dataTransfer.files);

            this._digitalAssetsService.upload({ data: data }).subscribe(x => {                
                this.inputElement.value = x.digitalAssets[0].relativePath;                
                this.onChangeCallback(this.inputElement.value);
            });
            
        }
    }
    
    public get inputElement(): HTMLInputElement { return this._elementRef.nativeElement.querySelector("input"); }
}
