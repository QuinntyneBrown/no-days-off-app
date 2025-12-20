export interface Athlete {
  athleteId: number;
  name: string;
  email: string;
  imageUrl: string;
  createdAt: Date;
}

export interface AthleteWeight {
  athleteWeightId: number;
  athleteId: number;
  weight: number;
  recordedAt: Date;
}
