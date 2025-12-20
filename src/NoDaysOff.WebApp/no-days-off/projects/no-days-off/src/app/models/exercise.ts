export interface Exercise {
  exerciseId: number;
  name: string;
  description: string;
  bodyPartId: number;
  videoUrl?: string;
}

export interface ScheduledExercise {
  scheduledExerciseId: number;
  exerciseId: number;
  dayId: number;
  sets: number;
  reps: number;
  isCompleted: boolean;
}

export interface BodyPart {
  bodyPartId: number;
  name: string;
}

export interface Day {
  dayId: number;
  name: string;
  date: Date;
  scheduledExercises: ScheduledExercise[];
}
