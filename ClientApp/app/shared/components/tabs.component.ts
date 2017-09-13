import {Component,QueryList,ContentChildren,Inject,forwardRef, AfterContentInit, ElementRef, OnInit} from '@angular/core';
import { TabTitleComponent, UPDATE_ACTIVE_TAB } from "./tab-title.component";
import { TabContentComponent } from "./tab-content.component";

@Component({    
    host: { 'class': 'tabs' },
    templateUrl: "./tabs.component.html",
    styleUrls: ["./tabs.component.css"],
    selector: 'ce-tabs'
})
export class TabsComponent implements AfterContentInit, OnInit {
    constructor(private _elementRef: ElementRef) {
        this.updateActiveTabByTitle = this.updateActiveTabByTitle.bind(this);
    }

    @ContentChildren(TabTitleComponent)
    titles: QueryList<TabTitleComponent>;

    @ContentChildren(TabContentComponent)
    contents: QueryList<TabContentComponent>;

    activeTitle: any = null;

    ngOnInit() {
        this._elementRef.nativeElement.addEventListener(UPDATE_ACTIVE_TAB, this.updateActiveTabByTitle);
    }

    ngAfterContentInit() {              
        this.updateActiveTabByTitle({ detail: { tabTitle: this.titles.first }});

        this.titles.changes.subscribe(x => {       
            this.updateActiveTabByTitle({ detail: { tabTitle: this.titles.first } });
        });
    }

    updateActiveTabByTitle($event: { detail: { tabTitle: TabTitleComponent } }) {   
        this.updateActiveTab((titleArr) => titleArr.indexOf($event.detail.tabTitle));
    }

    nextTab() {
        this.updateActiveTab((titleArr, lastIndex) => (lastIndex + 1) % titleArr.length);
    }

    private updateActiveTab(nextActiveIndexCb: (titleArr: any[], lastIndex: number) => number) {
        const titleArray = this.titles.toArray();
        const contentArray = this.contents.toArray();
        const lastIndex = titleArray.indexOf(this.activeTitle);
        const nextIndex = nextActiveIndexCb(titleArray, lastIndex);
        this.activeTitle = titleArray[nextIndex];

        if (lastIndex !== -1) {
            titleArray[lastIndex].deactivate();
            contentArray[lastIndex].deactivate();
        }
        titleArray[nextIndex].activate();
        contentArray[nextIndex].activate();
    }
}