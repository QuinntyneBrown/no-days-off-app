export interface Exercise {
  exerciseId: number;
  name: string;
  bodyPartId?: number;
  defaultSets?: number;
  defaultRepetitions?: number;
  tenantId?: number;
  createdOn: string;
}

export interface CreateExerciseRequest {
  name: string;
  bodyPartId?: number;
  defaultSets?: number;
  defaultRepetitions?: number;
  tenantId?: number;
}

export interface UpdateExerciseRequest {
  exerciseId: number;
  name: string;
  bodyPartId?: number;
  defaultSets?: number;
  defaultRepetitions?: number;
}

export interface BodyPart {
  bodyPartId: number;
  name: string;
  tenantId?: number;
  createdOn: string;
}

export interface CreateBodyPartRequest {
  name: string;
  tenantId?: number;
}

export interface UpdateBodyPartRequest {
  bodyPartId: number;
  name: string;
}
