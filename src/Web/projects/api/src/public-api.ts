/*
 * Public API Surface of api
 */

// Models
export type {
  LoginRequest,
  RegisterRequest,
  RefreshTokenRequest,
  AuthResponse,
  User,
  Tenant,
  CreateTenantRequest,
} from './lib/models/auth.models';

export type {
  Athlete,
  CreateAthleteRequest,
  UpdateAthleteRequest,
  AthleteWeight,
  RecordWeightRequest,
  Profile,
  CreateProfileRequest,
  UpdateProfileRequest,
} from './lib/models/athlete.models';

export type {
  Exercise,
  CreateExerciseRequest,
  UpdateExerciseRequest,
  BodyPart,
  CreateBodyPartRequest,
  UpdateBodyPartRequest,
} from './lib/models/exercise.models';

export type {
  Workout,
  CreateWorkoutRequest,
  UpdateWorkoutRequest,
  Day,
  CreateDayRequest,
  UpdateDayRequest,
  ScheduledExercise,
  CreateScheduledExerciseRequest,
  UpdateScheduledExerciseRequest,
} from './lib/models/workout.models';

export type {
  DashboardStats,
  Widget,
  CreateWidgetRequest,
  Dashboard,
  DashboardTile,
  CreateDashboardRequest,
  UpdateDashboardRequest,
  Tile,
  CreateTileRequest,
  UpdateTileRequest,
} from './lib/models/dashboard.models';

export type {
  Conversation,
  Message,
  CreateConversationRequest,
  SendMessageRequest,
  Notification,
  CreateNotificationRequest,
} from './lib/models/communication.models';

export type {
  MediaFile,
  DigitalAsset,
  CreateDigitalAssetRequest,
  UpdateDigitalAssetRequest,
  Video,
  CreateVideoRequest,
  UpdateVideoRequest,
} from './lib/models/media.models';

// Services
export { API_BASE_URL } from './lib/services/api-config';
export { AuthService } from './lib/services/auth.service';
export { UsersService } from './lib/services/users.service';
export { TenantsService } from './lib/services/tenants.service';
export { AthletesService } from './lib/services/athletes.service';
export { ExercisesService } from './lib/services/exercises.service';
export { BodyPartsService } from './lib/services/body-parts.service';
export { WorkoutsService } from './lib/services/workouts.service';
export { DaysService } from './lib/services/days.service';
export { ScheduledExercisesService } from './lib/services/scheduled-exercises.service';
export { DashboardService } from './lib/services/dashboard.service';
export { MediaService } from './lib/services/media.service';
export { ConversationsService } from './lib/services/conversations.service';
export { NotificationsService } from './lib/services/notifications.service';
export { VideosService } from './lib/services/videos.service';

// Interceptors
export { authInterceptor } from './lib/interceptors/auth.interceptor';
