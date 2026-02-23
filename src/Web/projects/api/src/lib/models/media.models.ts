export interface MediaFile {
  mediaFileId: number;
  fileName: string;
  originalFileName: string;
  contentType: string;
  size: number;
  type: number;
  tenantId: number;
  entityId?: number;
  entityType?: string;
  uploadedAt: string;
  uploadedBy: string;
  url?: string;
}

export interface DigitalAsset {
  digitalAssetId: number;
  name: string;
  fileName: string;
  description?: string;
  folder?: string;
  contentType: string;
  size: number;
  uniqueId: string;
  relativePath: string;
  tenantId?: number;
  createdOn: string;
}

export interface CreateDigitalAssetRequest {
  name: string;
  fileName: string;
  contentType: string;
  size: number;
  description?: string;
  folder?: string;
  tenantId?: number;
}

export interface UpdateDigitalAssetRequest {
  digitalAssetId: number;
  name: string;
  description?: string;
  folder?: string;
}

export interface Video {
  videoId: number;
  title: string;
  category?: string;
  subTitle?: string;
  slug?: string;
  youTubeVideoId?: string;
  abstract?: string;
  description?: string;
  durationInSeconds: number;
  rating: number;
  publishedOn?: string;
  publishedBy?: string;
  tenantId?: number;
  createdOn: string;
}

export interface CreateVideoRequest {
  title: string;
  category?: string;
  subTitle?: string;
  youTubeVideoId?: string;
  abstract?: string;
  description?: string;
  durationInSeconds?: number;
  tenantId?: number;
}

export interface UpdateVideoRequest {
  videoId: number;
  title: string;
  category?: string;
  subTitle?: string;
  youTubeVideoId?: string;
  abstract?: string;
  description?: string;
  durationInSeconds: number;
}
