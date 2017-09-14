import { constants } from "../constants";
import { Storage } from "./storage.service";

export class fetchClient {
    
    static async get<T>(url): Promise<T>{
        var headers = new Headers();
        headers.append("Authorization", `Bearer ${this.storage.get({ name: constants.ACCESS_TOKEN_KEY })}`);
        headers.append("Tenant", `${this.storage.get({ name: constants.TENANT })}`);
        const response = await fetch(url, { headers });
        return await response.json() as T;
    }

    static get storage(): Storage { return Storage.instance; }
}