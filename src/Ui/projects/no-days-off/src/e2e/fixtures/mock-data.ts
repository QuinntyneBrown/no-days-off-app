/**
 * Mock data for e2e tests
 */

export const API_BASE_URL = 'http://localhost:5007/api';

// Auth Mock Data
export const mockUser = {
  userId: 1,
  email: 'test@example.com',
  firstName: 'Test',
  lastName: 'User',
  tenantId: 1,
  roles: ['user']
};

export const mockAuthResponse = {
  accessToken: 'mock-access-token-12345',
  refreshToken: 'mock-refresh-token-67890',
  expiresAt: new Date(Date.now() + 3600000).toISOString(),
  user: mockUser
};

export const mockLoginRequest = {
  email: 'test@example.com',
  password: 'Test12345'
};

// Athletes Mock Data
export const mockAthletes = [
  {
    athleteId: 1,
    name: 'John Doe',
    username: 'johndoe',
    imageUrl: 'https://example.com/john.jpg',
    currentWeight: 180,
    lastWeighedOn: '2024-01-15T10:00:00Z',
    tenantId: 1,
    createdOn: '2024-01-01T00:00:00Z',
    createdBy: 'admin'
  },
  {
    athleteId: 2,
    name: 'Jane Smith',
    username: 'janesmith',
    imageUrl: null,
    currentWeight: 140,
    lastWeighedOn: '2024-01-14T08:00:00Z',
    tenantId: 1,
    createdOn: '2024-01-02T00:00:00Z',
    createdBy: 'admin'
  },
  {
    athleteId: 3,
    name: 'Bob Johnson',
    username: 'bobjohnson',
    imageUrl: 'https://example.com/bob.jpg',
    currentWeight: 200,
    lastWeighedOn: '2024-01-13T09:00:00Z',
    tenantId: 1,
    createdOn: '2024-01-03T00:00:00Z',
    createdBy: 'admin'
  }
];

export const mockNewAthlete = {
  athleteId: 4,
  name: 'New Athlete',
  username: 'newathlete',
  imageUrl: null,
  currentWeight: null,
  lastWeighedOn: null,
  tenantId: 1,
  createdOn: new Date().toISOString(),
  createdBy: 'test@example.com'
};

// Dashboard Mock Data
export const mockWidgets = [
  {
    widgetId: 1,
    name: 'Workout Stats',
    type: 1,
    tenantId: 1,
    userId: 1,
    position: 0,
    width: 2,
    height: 1,
    configuration: null,
    isVisible: true
  },
  {
    widgetId: 2,
    name: 'Recent Activity',
    type: 2,
    tenantId: 1,
    userId: 1,
    position: 1,
    width: 1,
    height: 2,
    configuration: null,
    isVisible: true
  },
  {
    widgetId: 3,
    name: 'Goals Progress',
    type: 3,
    tenantId: 1,
    userId: 1,
    position: 2,
    width: 1,
    height: 1,
    configuration: null,
    isVisible: true
  }
];

export const mockDashboardStats = {
  tenantId: 1,
  userId: 1,
  totalWorkouts: 42,
  totalExercises: 156,
  totalAthletes: 3,
  workoutsThisWeek: 5,
  workoutsThisMonth: 18,
  lastUpdated: new Date().toISOString()
};

export const mockNewWidget = {
  widgetId: 4,
  name: 'New Widget',
  type: 1,
  tenantId: 1,
  userId: 1,
  position: 3,
  width: 1,
  height: 1,
  configuration: null,
  isVisible: true
};

// Exercises Mock Data
export const mockBodyParts = [
  { bodyPartId: 1, name: 'Chest', description: 'Chest muscles', tenantId: 1 },
  { bodyPartId: 2, name: 'Back', description: 'Back muscles', tenantId: 1 },
  { bodyPartId: 3, name: 'Legs', description: 'Leg muscles', tenantId: 1 },
  { bodyPartId: 4, name: 'Arms', description: 'Arm muscles', tenantId: 1 },
  { bodyPartId: 5, name: 'Core', description: 'Core muscles', tenantId: 1 }
];

export const mockExercises = [
  {
    exerciseId: 1,
    name: 'Bench Press',
    description: 'Classic chest exercise',
    tenantId: 1,
    bodyPartId: 1,
    videoUrl: 'https://example.com/bench-press.mp4',
    imageUrl: 'https://example.com/bench-press.jpg',
    instructions: 'Lie on bench, lower bar to chest, push up',
    type: 1
  },
  {
    exerciseId: 2,
    name: 'Deadlift',
    description: 'Full body compound movement',
    tenantId: 1,
    bodyPartId: 2,
    videoUrl: 'https://example.com/deadlift.mp4',
    imageUrl: 'https://example.com/deadlift.jpg',
    instructions: 'Stand with feet hip-width, bend and grip bar, lift',
    type: 1
  },
  {
    exerciseId: 3,
    name: 'Squats',
    description: 'Lower body compound exercise',
    tenantId: 1,
    bodyPartId: 3,
    videoUrl: 'https://example.com/squats.mp4',
    imageUrl: 'https://example.com/squats.jpg',
    instructions: 'Stand with bar on shoulders, squat down, stand up',
    type: 1
  }
];

// Workouts Mock Data
export const mockWorkouts = [
  {
    workoutId: 1,
    name: 'Push Day',
    bodyPartIds: [1, 4],
    tenantId: 1,
    createdOn: '2024-01-10T00:00:00Z'
  },
  {
    workoutId: 2,
    name: 'Pull Day',
    bodyPartIds: [2, 4],
    tenantId: 1,
    createdOn: '2024-01-11T00:00:00Z'
  },
  {
    workoutId: 3,
    name: 'Leg Day',
    bodyPartIds: [3, 5],
    tenantId: 1,
    createdOn: '2024-01-12T00:00:00Z'
  }
];

// Notifications Mock Data
export const mockNotifications = [
  {
    notificationId: 1,
    title: 'Workout Reminder',
    message: 'Time for your scheduled workout!',
    type: 'reminder',
    isRead: false,
    createdAt: new Date().toISOString()
  },
  {
    notificationId: 2,
    title: 'Goal Achieved',
    message: 'Congratulations! You reached your weekly goal.',
    type: 'achievement',
    isRead: true,
    createdAt: new Date(Date.now() - 86400000).toISOString()
  }
];

// Error responses
export const mockErrorResponses = {
  unauthorized: {
    status: 401,
    body: { message: 'Invalid credentials' }
  },
  forbidden: {
    status: 403,
    body: { message: 'Access denied' }
  },
  notFound: {
    status: 404,
    body: { message: 'Resource not found' }
  },
  serverError: {
    status: 500,
    body: { message: 'Internal server error' }
  },
  validationError: {
    status: 400,
    body: { message: 'Validation failed', errors: ['Name is required'] }
  }
};
