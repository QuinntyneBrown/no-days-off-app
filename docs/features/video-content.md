# Video Content Feature

## Overview

The Video Content feature manages exercise tutorial videos, workout guides, and other educational video content linked to YouTube.

## Domain Model

### Video Aggregate

```
Video
├── Title: string (max 256 chars)
├── Category: string
├── SubTitle: string
├── Slug: string (auto-generated)
├── YouTubeVideoId: string
├── Abstract: string
├── Description: string
├── DurationInSeconds: int
├── Rating: decimal (0-5)
├── PublishedOn: DateTime?
└── PublishedBy: string
```

## Features

### Video Management

- Create and manage video entries
- Auto-generate URL-friendly slugs
- Link to YouTube videos
- Categorize videos

### Publishing Workflow

- Draft and published states
- Track who published and when
- Unpublish videos

### Content Metadata

- Duration tracking
- Rating system (0-5 stars)
- Rich text description

## Usage

### Creating a Video

```csharp
var video = Video.Create(
    tenantId: 1,
    title: "Proper Bench Press Form",
    youTubeVideoId: "dQw4w9WgXcQ",
    createdBy: "admin"
);

// Auto-generated slug: "proper-bench-press-form"
```

### Adding Content Details

```csharp
video.UpdateContent(
    subTitle: "Master the king of chest exercises",
    @abstract: "Learn proper bench press technique to maximize gains and prevent injury.",
    description: "In this video, we cover grip width, bar path, breathing...",
    modifiedBy: "admin"
);

video.UpdateCategory("Chest Exercises", modifiedBy: "admin");
video.UpdateDuration(durationInSeconds: 720, modifiedBy: "admin"); // 12 minutes
```

### Publishing

```csharp
// Publish the video
video.Publish(publishedBy: "admin");

// Check published status
bool isPublished = video.IsPublished; // true
DateTime? publishedOn = video.PublishedOn;
string publishedBy = video.PublishedBy;

// Unpublish if needed
video.Unpublish(modifiedBy: "admin");
```

### Rating Videos

```csharp
video.UpdateRating(rating: 4.5m, modifiedBy: "admin");
```

## Validation Rules

| Field | Validation |
|-------|------------|
| Title | Required, max 256 characters |
| YouTubeVideoId | Required |
| DurationInSeconds | Cannot be negative |
| Rating | Must be between 0 and 5 |

## Slug Generation

Slugs are automatically generated from the title:
- Converted to lowercase
- Spaces replaced with hyphens
- Double hyphens collapsed

Examples:
- "Proper Bench Press Form" → "proper-bench-press-form"
- "HIIT  Cardio  Workout" → "hiit-cardio-workout"

## Integration with YouTube

Videos are linked to YouTube via the `YouTubeVideoId` property. This can be used to:
- Embed videos in the application
- Fetch video thumbnails
- Link to YouTube for playback

Example embed URL construction:
```
https://www.youtube.com/embed/{YouTubeVideoId}
```

## Categories

Suggested video categories:
- Chest Exercises
- Back Exercises
- Leg Exercises
- Shoulder Exercises
- Arm Exercises
- Core Exercises
- Cardio Workouts
- Stretching & Mobility
- Nutrition Tips
- Recovery & Rest
