export interface Athlete {
  athleteId: number;
  name: string;
  username: string;
  imageUrl?: string;
  currentWeight?: number;
  lastWeighedOn?: Date;
  tenantId?: number;
  createdOn: Date;
  createdBy: string;
}

export interface AthleteWeight {
  athleteWeightId: number;
  athleteId: number;
  weight: number;
  recordedAt: Date;
}
