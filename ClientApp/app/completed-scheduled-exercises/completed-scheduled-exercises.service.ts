import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { CompletedScheduledExercise } from "./completed-scheduled-exercise.model";
import { Observable } from "rxjs/Observable";
import { ErrorService } from "../shared/services/error.service";

@Injectable()
export class CompletedScheduledExercisesService {
    constructor(
        private _errorService: ErrorService,
        private _httpClient: HttpClient)
    { }

    public addOrUpdate(options: { completedScheduledExercise: CompletedScheduledExercise, correlationId: string }) {
        return this._httpClient
            .post(`${this._baseUrl}/api/completedScheduledExercises/add`, options)
            .catch(this._errorService.catchErrorResponse);
    }

    public get(): Observable<{ completedScheduledExercises: Array<CompletedScheduledExercise> }> {
        return this._httpClient
            .get<{ completedScheduledExercises: Array<CompletedScheduledExercise> }>(`${this._baseUrl}/api/completedScheduledExercises/get`)
            .catch(this._errorService.catchErrorResponse);
    }

    public getById(options: { id: number }): Observable<{ completedScheduledExercise:CompletedScheduledExercise}> {
        return this._httpClient
            .get<{completedScheduledExercise: CompletedScheduledExercise}>(`${this._baseUrl}/api/completedScheduledExercises/getById?id=${options.id}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public remove(options: { completedScheduledExercise: CompletedScheduledExercise, correlationId: string }) {
        return this._httpClient
            .delete(`${this._baseUrl}/api/completedScheduledExercises/remove?id=${options.completedScheduledExercise.id}&correlationId=${options.correlationId}`)
            .catch(this._errorService.catchErrorResponse);
    }

    public get _baseUrl() { return ""; }
}
