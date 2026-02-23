export interface Workout {
  workoutId: number;
  name: string;
  bodyPartIds: number[];
  tenantId?: number;
  createdOn: string;
}

export interface CreateWorkoutRequest {
  name: string;
  bodyPartIds?: number[];
  tenantId?: number;
}

export interface UpdateWorkoutRequest {
  workoutId: number;
  name: string;
  bodyPartIds?: number[];
}

export interface Day {
  dayId: number;
  name: string;
  bodyPartIds: number[];
  tenantId?: number;
  createdOn: string;
}

export interface CreateDayRequest {
  name: string;
  bodyPartIds?: number[];
  tenantId?: number;
}

export interface UpdateDayRequest {
  dayId: number;
  name: string;
  bodyPartIds?: number[];
}

export interface ScheduledExercise {
  scheduledExerciseId: number;
  name: string;
  dayId: number;
  exerciseId: number;
  sort: number;
  repetitions?: number;
  weightInKgs?: number;
  sets?: number;
  distance?: number;
  timeInSeconds?: number;
  tenantId?: number;
  createdOn: string;
}

export interface CreateScheduledExerciseRequest {
  name: string;
  dayId: number;
  exerciseId: number;
  sort?: number;
  repetitions?: number;
  weightInKgs?: number;
  sets?: number;
  distance?: number;
  timeInSeconds?: number;
  tenantId?: number;
}

export interface UpdateScheduledExerciseRequest {
  scheduledExerciseId: number;
  name: string;
  sort: number;
  repetitions?: number;
  weightInKgs?: number;
  sets?: number;
  distance?: number;
  timeInSeconds?: number;
}
