import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { ScheduledExercise } from "./scheduled-exercise.model";
import { Observable } from "rxjs/Observable";
import { ErrorService } from "../shared/services/error.service";

@Injectable()
export class ScheduledExercisesService {
    constructor(
        private _errorService: ErrorService,
        private _httpClient: HttpClient)
    { }

    public addOrUpdate(options: { scheduledExercise: ScheduledExercise, correlationId: string }) {
        return this._httpClient
            .post(`${this._baseUrl}/api/scheduledExercises/add`, options)
            .catch(this._errorService.catchErrorResponse);
    }

    public get(): Observable<{ scheduledExercises: Array<ScheduledExercise> }> {
        return this._httpClient
            .get<{ scheduledExercises: Array<ScheduledExercise> }>(`${this._baseUrl}/api/scheduledExercises/get`)
            .catch(this._errorService.catchErrorResponse);
    }

    public getById(options: { id: number }): Observable<{ scheduledExercise:ScheduledExercise}> {
        return this._httpClient
            .get<{scheduledExercise: ScheduledExercise}>(`${this._baseUrl}/api/scheduledExercises/getById?id=${options.id}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public remove(options: { scheduledExercise: ScheduledExercise, correlationId: string }) {
        return this._httpClient
            .delete(`${this._baseUrl}/api/scheduledExercises/remove?id=${options.scheduledExercise.id}&correlationId=${options.correlationId}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public get _baseUrl() { return ""; }
}
