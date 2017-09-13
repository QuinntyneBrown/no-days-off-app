import {Injectable} from "@angular/core";
import {guid} from "../utilities/guid";

@Injectable()
export class CorrelationIdsList {   
    private _guids: Array<string> = [];

    public newId() {
        var newGuid = guid();
        this._guids.push(newGuid);
        return newGuid;
    }

    public hasId(guid: string): boolean {
        return this._guids.indexOf(guid) > -1;
    }
}