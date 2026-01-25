# Media Management User Guide

## Overview

The Media microservice handles file uploads, digital asset management, and video content for the No Days Off application.

---

## Behaviors

### 1. Upload Media File

**Purpose**: Upload a file to the media storage system.

**Command**: `UploadMediaFileCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| File | Stream | Yes | File content stream |
| FileName | string | Yes | Original file name |
| ContentType | string | Yes | MIME type |
| EntityType | string | No | Related entity type |
| EntityId | int | No | Related entity ID |
| TenantId | int | Yes | Tenant identifier |

#### API Endpoint
```
POST /api/media/upload
Content-Type: multipart/form-data
Authorization: Bearer {accessToken}

--boundary
Content-Disposition: form-data; name="file"; filename="exercise-demo.mp4"
Content-Type: video/mp4

[binary file content]
--boundary
Content-Disposition: form-data; name="entityType"

Exercise
--boundary
Content-Disposition: form-data; name="entityId"

1
--boundary--
```

#### Response
```json
{
    "mediaFileId": 1,
    "fileName": "exercise-demo.mp4",
    "originalFileName": "exercise-demo.mp4",
    "contentType": "video/mp4",
    "mediaType": "Video",
    "size": 15728640,
    "url": "https://cdn.example.com/media/tenant-1/exercise-demo-abc123.mp4",
    "thumbnailUrl": "https://cdn.example.com/media/tenant-1/thumbnails/exercise-demo-abc123.jpg",
    "createdOn": "2024-01-16T10:30:00Z"
}
```

#### Business Rules
- File is stored in cloud storage (Azure Blob, AWS S3, etc.)
- Unique filename is generated to prevent collisions
- Thumbnails are generated for images and videos
- A `MediaUploadedMessage` is published to the message bus

#### Message Bus Event
```json
{
    "topic": "media.uploaded",
    "payload": {
        "mediaFileId": 1,
        "fileName": "exercise-demo.mp4",
        "mediaType": "Video",
        "entityType": "Exercise",
        "entityId": 1,
        "tenantId": 1
    }
}
```

#### Supported Media Types
| Type | Extensions | Max Size |
|------|-----------|----------|
| Image | jpg, jpeg, png, gif, webp | 10MB |
| Video | mp4, webm, mov | 100MB |
| Document | pdf, doc, docx | 25MB |
| Other | * | 50MB |

---

### 2. Get Media Files

**Purpose**: Retrieve list of uploaded media files.

**Query**: `GetMediaFilesQuery`

#### API Endpoint
```
GET /api/media
Authorization: Bearer {accessToken}
```

#### Query Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| mediaType | string | Filter by type (Image, Video, Document) |
| entityType | string | Filter by related entity type |
| entityId | int | Filter by related entity ID |
| page | int | Page number |
| pageSize | int | Items per page |

#### Response
```json
{
    "mediaFiles": [
        {
            "mediaFileId": 1,
            "fileName": "exercise-demo.mp4",
            "mediaType": "Video",
            "size": 15728640,
            "url": "https://cdn.example.com/media/exercise-demo.mp4",
            "thumbnailUrl": "https://cdn.example.com/thumbnails/exercise-demo.jpg",
            "createdOn": "2024-01-16T10:30:00Z"
        },
        {
            "mediaFileId": 2,
            "fileName": "profile-photo.jpg",
            "mediaType": "Image",
            "size": 524288,
            "url": "https://cdn.example.com/media/profile-photo.jpg",
            "thumbnailUrl": "https://cdn.example.com/thumbnails/profile-photo.jpg",
            "createdOn": "2024-01-15T09:00:00Z"
        }
    ],
    "totalCount": 25,
    "page": 1,
    "pageSize": 20
}
```

---

### 3. Get Media File by ID

**Purpose**: Retrieve a specific media file's details.

**Query**: `GetMediaFileByIdQuery`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| MediaFileId | int | Yes | Media file identifier |

#### API Endpoint
```
GET /api/media/{mediaFileId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "mediaFileId": 1,
    "fileName": "exercise-demo.mp4",
    "originalFileName": "bench-press-tutorial.mp4",
    "contentType": "video/mp4",
    "mediaType": "Video",
    "size": 15728640,
    "url": "https://cdn.example.com/media/exercise-demo.mp4",
    "thumbnailUrl": "https://cdn.example.com/thumbnails/exercise-demo.jpg",
    "storagePath": "tenant-1/videos/exercise-demo-abc123.mp4",
    "entityType": "Exercise",
    "entityId": 1,
    "metadata": {
        "duration": 120,
        "width": 1920,
        "height": 1080,
        "codec": "h264"
    },
    "createdOn": "2024-01-16T10:30:00Z",
    "createdBy": "admin"
}
```

---

### 4. Delete Media File

**Purpose**: Remove a media file from storage.

**Command**: `DeleteMediaFileCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| MediaFileId | int | Yes | Media file identifier |

#### API Endpoint
```
DELETE /api/media/{mediaFileId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "message": "Media file deleted successfully"
}
```

#### Business Rules
- File is removed from cloud storage
- Database record is deleted
- Associated entity references are cleaned up

---

## Digital Asset Management

### 5. Create Digital Asset

**Purpose**: Create a digital asset record linked to a media file.

**Command**: `CreateDigitalAssetCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| Name | string | Yes | Asset name |
| MediaFileId | int | Yes | Associated media file |
| Description | string | No | Asset description |
| Tags | string[] | No | Searchable tags |
| TenantId | int | Yes | Tenant identifier |

#### API Endpoint
```
POST /api/digital-assets
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "name": "Bench Press Tutorial",
    "mediaFileId": 1,
    "description": "Proper form demonstration for bench press",
    "tags": ["chest", "tutorial", "bench-press"]
}
```

#### Response
```json
{
    "digitalAssetId": 1,
    "name": "Bench Press Tutorial",
    "mediaFileId": 1,
    "url": "https://cdn.example.com/media/exercise-demo.mp4",
    "thumbnailUrl": "https://cdn.example.com/thumbnails/exercise-demo.jpg",
    "mediaType": "Video",
    "tags": ["chest", "tutorial", "bench-press"],
    "createdOn": "2024-01-16T10:35:00Z"
}
```

---

### 6. Get Digital Assets

**Purpose**: Retrieve list of digital assets.

**Query**: `GetDigitalAssetsQuery`

#### API Endpoint
```
GET /api/digital-assets
Authorization: Bearer {accessToken}
```

#### Query Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| mediaType | string | Filter by media type |
| tags | string | Filter by tags (comma-separated) |
| search | string | Search by name/description |
| page | int | Page number |
| pageSize | int | Items per page |

#### Response
```json
{
    "digitalAssets": [
        {
            "digitalAssetId": 1,
            "name": "Bench Press Tutorial",
            "mediaType": "Video",
            "url": "https://cdn.example.com/media/exercise-demo.mp4",
            "thumbnailUrl": "https://cdn.example.com/thumbnails/exercise-demo.jpg",
            "tags": ["chest", "tutorial"]
        },
        {
            "digitalAssetId": 2,
            "name": "Squat Form Guide",
            "mediaType": "Image",
            "url": "https://cdn.example.com/media/squat-form.jpg",
            "thumbnailUrl": "https://cdn.example.com/thumbnails/squat-form.jpg",
            "tags": ["legs", "form-guide"]
        }
    ],
    "totalCount": 50,
    "page": 1,
    "pageSize": 20
}
```

---

### 7. Get Digital Asset by ID

**Purpose**: Retrieve a specific digital asset's details.

**Query**: `GetDigitalAssetByIdQuery`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| DigitalAssetId | int | Yes | Digital asset identifier |

#### API Endpoint
```
GET /api/digital-assets/{digitalAssetId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "digitalAssetId": 1,
    "name": "Bench Press Tutorial",
    "description": "Proper form demonstration for bench press",
    "mediaFileId": 1,
    "mediaType": "Video",
    "url": "https://cdn.example.com/media/exercise-demo.mp4",
    "thumbnailUrl": "https://cdn.example.com/thumbnails/exercise-demo.jpg",
    "tags": ["chest", "tutorial", "bench-press"],
    "linkedExercises": [
        {
            "exerciseId": 1,
            "name": "Bench Press"
        }
    ],
    "metadata": {
        "duration": 120,
        "resolution": "1920x1080"
    },
    "createdOn": "2024-01-16T10:35:00Z"
}
```

---

### 8. Update Digital Asset

**Purpose**: Modify a digital asset's metadata.

**Command**: `UpdateDigitalAssetCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| DigitalAssetId | int | Yes | Digital asset identifier |
| Name | string | Yes | Updated name |
| Description | string | No | Updated description |
| Tags | string[] | No | Updated tags |

#### API Endpoint
```
PUT /api/digital-assets/{digitalAssetId}
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "name": "Barbell Bench Press - Complete Tutorial",
    "description": "Full guide including warm-up, proper form, and common mistakes",
    "tags": ["chest", "tutorial", "bench-press", "beginner-friendly"]
}
```

#### Response
```json
{
    "digitalAssetId": 1,
    "name": "Barbell Bench Press - Complete Tutorial",
    "description": "Full guide including warm-up, proper form, and common mistakes",
    "tags": ["chest", "tutorial", "bench-press", "beginner-friendly"],
    "lastModifiedOn": "2024-01-16T14:20:00Z"
}
```

---

### 9. Delete Digital Asset

**Purpose**: Remove a digital asset.

**Command**: `DeleteDigitalAssetCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| DigitalAssetId | int | Yes | Digital asset identifier |

#### API Endpoint
```
DELETE /api/digital-assets/{digitalAssetId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "message": "Digital asset deleted successfully"
}
```

---

## Video Content Management

### 10. Create Video

**Purpose**: Create a video entry (e.g., YouTube integration).

**Command**: `CreateVideoCommand`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| Title | string | Yes | Video title |
| Description | string | No | Video description |
| Url | string | Yes | Video URL (YouTube, Vimeo, etc.) |
| ThumbnailUrl | string | No | Custom thumbnail |
| Duration | int | No | Duration in seconds |
| Category | string | No | Video category |
| TenantId | int | Yes | Tenant identifier |

#### API Endpoint
```
POST /api/videos
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "title": "Ultimate Push Day Workout",
    "description": "Complete chest, shoulders, and triceps routine",
    "url": "https://www.youtube.com/watch?v=abc123",
    "duration": 1800,
    "category": "Workout"
}
```

#### Response
```json
{
    "videoId": 1,
    "title": "Ultimate Push Day Workout",
    "description": "Complete chest, shoulders, and triceps routine",
    "url": "https://www.youtube.com/watch?v=abc123",
    "embedUrl": "https://www.youtube.com/embed/abc123",
    "thumbnailUrl": "https://img.youtube.com/vi/abc123/maxresdefault.jpg",
    "duration": 1800,
    "durationFormatted": "30:00",
    "category": "Workout",
    "createdOn": "2024-01-16T10:30:00Z"
}
```

---

### 11. Get Videos

**Purpose**: Retrieve list of video content.

**Query**: `GetVideosQuery`

#### API Endpoint
```
GET /api/videos
Authorization: Bearer {accessToken}
```

#### Query Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| category | string | Filter by category |
| search | string | Search by title/description |
| page | int | Page number |
| pageSize | int | Items per page |

#### Response
```json
{
    "videos": [
        {
            "videoId": 1,
            "title": "Ultimate Push Day Workout",
            "thumbnailUrl": "https://img.youtube.com/vi/abc123/default.jpg",
            "duration": 1800,
            "durationFormatted": "30:00",
            "category": "Workout"
        },
        {
            "videoId": 2,
            "title": "Proper Squat Form",
            "thumbnailUrl": "https://img.youtube.com/vi/xyz789/default.jpg",
            "duration": 600,
            "durationFormatted": "10:00",
            "category": "Tutorial"
        }
    ],
    "totalCount": 25,
    "page": 1,
    "pageSize": 20
}
```

---

### 12. Get Video by ID

**Purpose**: Retrieve a specific video's details.

**Query**: `GetVideoByIdQuery`

#### Input Parameters
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| VideoId | int | Yes | Video identifier |

#### API Endpoint
```
GET /api/videos/{videoId}
Authorization: Bearer {accessToken}
```

#### Response
```json
{
    "videoId": 1,
    "title": "Ultimate Push Day Workout",
    "description": "Complete chest, shoulders, and triceps routine",
    "url": "https://www.youtube.com/watch?v=abc123",
    "embedUrl": "https://www.youtube.com/embed/abc123",
    "thumbnailUrl": "https://img.youtube.com/vi/abc123/maxresdefault.jpg",
    "duration": 1800,
    "durationFormatted": "30:00",
    "category": "Workout",
    "linkedExercises": [
        { "exerciseId": 1, "name": "Bench Press" },
        { "exerciseId": 2, "name": "Overhead Press" }
    ],
    "views": 1500,
    "createdOn": "2024-01-16T10:30:00Z"
}
```

---

### 13. Update Video

**Purpose**: Modify video metadata.

**Command**: `UpdateVideoCommand`

#### API Endpoint
```
PUT /api/videos/{videoId}
Content-Type: application/json
Authorization: Bearer {accessToken}

{
    "title": "Ultimate Push Day Workout - Updated",
    "description": "Updated with new tips and techniques",
    "category": "Workout"
}
```

---

### 14. Delete Video

**Purpose**: Remove a video entry.

**Command**: `DeleteVideoCommand`

#### API Endpoint
```
DELETE /api/videos/{videoId}
Authorization: Bearer {accessToken}
```

---

## Media Upload Flow

```
+-------------+     +------------------+     +------------------+
|   Client    |---->| Multipart        |---->| Validate         |
|   Upload    |     | Form Parse       |     | File Type/Size   |
+-------------+     +------------------+     +------------------+
                                                     |
                                                     v
+------------------+     +------------------+     +------------------+
| Return           |<----| Store in DB      |<----| Upload to        |
| File URL         |     | (MediaFile)      |     | Cloud Storage    |
+------------------+     +------------------+     +------------------+
                                                     |
                                                     v
                    +------------------+     +------------------+
                    | Generate         |     | Publish Message  |
                    | Thumbnail        |     | to Bus           |
                    +------------------+     +------------------+
```

---

## Entity Relationships

```
+------------------+
|    MediaFile     |
+------------------+
        |
        | 1:1
        v
+------------------+    *:*    +------------------+
|  DigitalAsset    |<--------->|    Exercise      |
+------------------+           +------------------+

+------------------+
|      Video       |<--------> (External URL/YouTube)
+------------------+
```

---

## Storage Configuration

### Supported Providers
| Provider | Configuration Key |
|----------|------------------|
| Azure Blob Storage | `AzureStorage:ConnectionString` |
| AWS S3 | `AWS:S3:BucketName` |
| Local File System | `LocalStorage:BasePath` |

### File Organization
```
{storage-root}/
  └── {tenant-id}/
      ├── images/
      │   ├── {filename}-{guid}.jpg
      │   └── thumbnails/
      │       └── {filename}-{guid}-thumb.jpg
      ├── videos/
      │   ├── {filename}-{guid}.mp4
      │   └── thumbnails/
      │       └── {filename}-{guid}-thumb.jpg
      └── documents/
          └── {filename}-{guid}.pdf
```

---

## Validation Rules Summary

| Entity | Field | Validation |
|--------|-------|------------|
| MediaFile | FileName | Required |
| MediaFile | ContentType | Required, valid MIME type |
| MediaFile | Size | Must not exceed max for type |
| DigitalAsset | Name | Required |
| DigitalAsset | MediaFileId | Required, valid reference |
| Video | Title | Required |
| Video | Url | Required, valid URL |

---

## Error Codes

| Code | Description |
|------|-------------|
| 400 | Invalid file type or request |
| 401 | Unauthorized access |
| 404 | Media file not found |
| 413 | File size exceeds limit |
| 415 | Unsupported media type |
