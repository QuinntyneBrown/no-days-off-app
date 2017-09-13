import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Exercise } from "./exercise.model";
import { Observable } from "rxjs/Observable";
import { ErrorService } from "../shared/services/error.service";

@Injectable()
export class ExercisesService {
    constructor(
        private _errorService: ErrorService,
        private _httpClient: HttpClient)
    { }

    public addOrUpdate(options: { exercise: Exercise, correlationId: string }) {
        return this._httpClient
            .post(`${this._baseUrl}/api/exercises/add`, options)
            .catch(this._errorService.catchErrorResponse);
    }

    public get(): Observable<{ exercises: Array<Exercise> }> {
        return this._httpClient
            .get<{ exercises: Array<Exercise> }>(`${this._baseUrl}/api/exercises/get`)
            .catch(this._errorService.catchErrorResponse);
    }

    public getById(options: { id: number }): Observable<{ exercise:Exercise}> {
        return this._httpClient
            .get<{exercise: Exercise}>(`${this._baseUrl}/api/exercises/getById?id=${options.id}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public remove(options: { exercise: Exercise, correlationId: string }) {
        return this._httpClient
            .delete(`${this._baseUrl}/api/exercises/remove?id=${options.exercise.id}&correlationId=${options.correlationId}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public get _baseUrl() { return ""; }
}
