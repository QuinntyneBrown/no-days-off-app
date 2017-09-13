import {Component,EventEmitter,Output,Input} from "@angular/core";

@Component({
    templateUrl: "./pager.component.html",
    styleUrls: ["./pager.component.css"],
    selector: "ce-pager"
})
export class PagerComponent {
    constructor() {
        this.next = new EventEmitter();
        this.previous = new EventEmitter();
    }

    @Output()
    public next: EventEmitter<any>;

    @Output()
    public previous: EventEmitter<any>;

    @Input()
    pageNumber: number;

    @Input()
    totalPages: number;

    public emitNext() {
        let value = 1;
        if (this.pageNumber < this.totalPages) { value = this.pageNumber + 1; }
        this.next.emit({ detail: { pageNumber: value } });
    }

    public emitPrevious() {
        let value = this.totalPages;
        if (this.pageNumber != 1) { value = this.pageNumber - 1; }
        this.next.emit({ detail: { pageNumber: value } });
    }
}

export interface IPagedList<T> {
    data: Array<T>;
    _data: Array<T>;
    page: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
}

export class PagedList<T> implements IPagedList<T> {
    constructor(public _data: Array<T>, private _page: number, private _pageSize: number, private _totalCount: number) { }
    get data(): Array<T> { return this._data; }
    get page(): number { return this._page; }
    get pageSize(): number { return this._pageSize; }
    get totalCount(): number { return this._totalCount; }
    get totalPages(): number { return Math.ceil(this._totalCount / this._pageSize); }
}

export class PagingConfig {
    constructor(public page: number, public pageSize: number) { }
}

export function validatePagePropertiesAndGetSkipCount(pagingConfig: PagingConfig) {

    if (pagingConfig.page < 1) {
        pagingConfig.page = 1;
    }

    if (pagingConfig.pageSize < 1) {
        pagingConfig.pageSize = 1;
    }

    if (pagingConfig.pageSize > 100) {
        pagingConfig.pageSize = 100;
    }

    return pagingConfig.pageSize * (pagingConfig.page - 1);
}


export function toPageListFromInMemory<T>(entities: Array<T>, page: number, pageSize: number): IPagedList<T> {
    try {
        if (entities == null)
            throw new Error("entities");
        var pagingConfig = new PagingConfig(page, pageSize);
        var skipCount = validatePagePropertiesAndGetSkipCount(pagingConfig);
        var data = entities.slice(skipCount, pageSize + skipCount);
        return new PagedList(data, page, pageSize, entities.length);
    } catch (e) {
        alert(JSON.stringify(entities));
    }
}

