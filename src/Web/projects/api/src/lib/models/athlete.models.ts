export interface Athlete {
  athleteId: number;
  name: string;
  username: string;
  imageUrl?: string;
  currentWeight?: number;
  lastWeighedOn?: string;
  tenantId?: number;
  createdOn: string;
  createdBy: string;
}

export interface CreateAthleteRequest {
  name: string;
  username: string;
  tenantId?: number;
}

export interface UpdateAthleteRequest {
  athleteId: number;
  name: string;
  username: string;
}

export interface Profile {
  profileId: number;
  name: string;
  username: string;
  imageUrl?: string;
  tenantId?: number;
  createdOn: string;
}

export interface CreateProfileRequest {
  name: string;
  username: string;
  tenantId?: number;
}

export interface UpdateProfileRequest {
  profileId: number;
  name: string;
  username: string;
  imageUrl?: string;
}
