import { Page } from '@playwright/test';
import {
  API_BASE_URL,
  mockAuthResponse,
  mockAthletes,
  mockWidgets,
  mockDashboardStats,
  mockExercises,
  mockBodyParts,
  mockWorkouts,
  mockNotifications,
  mockNewAthlete,
  mockNewWidget,
  mockErrorResponses
} from '../fixtures/mock-data';

/**
 * Helper class for setting up API mocks in Playwright tests
 */
export class ApiMocks {
  constructor(private page: Page) {}

  /**
   * Setup all default mocks for authenticated user
   */
  async setupDefaultMocks(): Promise<void> {
    await this.mockAuthLogin();
    await this.mockAthletes();
    await this.mockDashboard();
    await this.mockExercises();
    await this.mockWorkouts();
    await this.mockNotifications();
  }

  /**
   * Set authentication token in localStorage
   */
  async setAuthenticatedState(): Promise<void> {
    await this.page.evaluate((authData) => {
      localStorage.setItem('access_token', authData.accessToken);
      localStorage.setItem('refresh_token', authData.refreshToken);
      localStorage.setItem('current_user', JSON.stringify(authData.user));
    }, mockAuthResponse);
  }

  /**
   * Clear authentication state
   */
  async clearAuthState(): Promise<void> {
    await this.page.evaluate(() => {
      localStorage.removeItem('access_token');
      localStorage.removeItem('refresh_token');
      localStorage.removeItem('current_user');
    });
  }

  // ===== AUTH MOCKS =====

  async mockAuthLogin(success = true): Promise<void> {
    await this.page.route(`${API_BASE_URL}/auth/login`, async (route) => {
      if (success) {
        await route.fulfill({
          status: 200,
          contentType: 'application/json',
          body: JSON.stringify(mockAuthResponse)
        });
      } else {
        await route.fulfill({
          status: 401,
          contentType: 'application/json',
          body: JSON.stringify(mockErrorResponses.unauthorized.body)
        });
      }
    });
  }

  async mockAuthRegister(success = true): Promise<void> {
    await this.page.route(`${API_BASE_URL}/auth/register`, async (route) => {
      if (success) {
        await route.fulfill({
          status: 200,
          contentType: 'application/json',
          body: JSON.stringify(mockAuthResponse)
        });
      } else {
        await route.fulfill({
          status: 400,
          contentType: 'application/json',
          body: JSON.stringify({ message: 'Email already exists' })
        });
      }
    });
  }

  async mockAuthRefresh(success = true): Promise<void> {
    await this.page.route(`${API_BASE_URL}/auth/refresh`, async (route) => {
      if (success) {
        await route.fulfill({
          status: 200,
          contentType: 'application/json',
          body: JSON.stringify(mockAuthResponse)
        });
      } else {
        await route.fulfill({
          status: 401,
          contentType: 'application/json',
          body: JSON.stringify({ message: 'Invalid refresh token' })
        });
      }
    });
  }

  // ===== ATHLETES MOCKS =====

  async mockAthletes(athletes = mockAthletes): Promise<void> {
    // GET all athletes
    await this.page.route(`${API_BASE_URL}/athletes`, async (route, request) => {
      if (request.method() === 'GET') {
        await route.fulfill({
          status: 200,
          contentType: 'application/json',
          body: JSON.stringify(athletes)
        });
      } else if (request.method() === 'POST') {
        const body = request.postDataJSON();
        await route.fulfill({
          status: 201,
          contentType: 'application/json',
          body: JSON.stringify({
            ...mockNewAthlete,
            name: body.name,
            username: body.username
          })
        });
      }
    });

    // GET, PUT, DELETE single athlete
    await this.page.route(`${API_BASE_URL}/athletes/*`, async (route, request) => {
      const url = request.url();
      const idMatch = url.match(/\/athletes\/(\d+)/);
      const id = idMatch ? parseInt(idMatch[1]) : 0;
      const athlete = athletes.find(a => a.athleteId === id);

      if (request.method() === 'GET') {
        if (athlete) {
          await route.fulfill({
            status: 200,
            contentType: 'application/json',
            body: JSON.stringify(athlete)
          });
        } else {
          await route.fulfill({
            status: 404,
            contentType: 'application/json',
            body: JSON.stringify(mockErrorResponses.notFound.body)
          });
        }
      } else if (request.method() === 'PUT') {
        const body = request.postDataJSON();
        await route.fulfill({
          status: 200,
          contentType: 'application/json',
          body: JSON.stringify({
            ...athlete,
            name: body.name,
            username: body.username
          })
        });
      } else if (request.method() === 'DELETE') {
        await route.fulfill({
          status: 204,
          body: ''
        });
      }
    });
  }

  async mockAthletesEmpty(): Promise<void> {
    await this.mockAthletes([]);
  }

  async mockAthletesError(): Promise<void> {
    await this.page.route(`${API_BASE_URL}/athletes`, async (route) => {
      await route.fulfill({
        status: 500,
        contentType: 'application/json',
        body: JSON.stringify(mockErrorResponses.serverError.body)
      });
    });
  }

  async mockCreateAthleteError(): Promise<void> {
    await this.page.route(`${API_BASE_URL}/athletes`, async (route, request) => {
      if (request.method() === 'POST') {
        await route.fulfill({
          status: 400,
          contentType: 'application/json',
          body: JSON.stringify(mockErrorResponses.validationError.body)
        });
      } else {
        await route.fulfill({
          status: 200,
          contentType: 'application/json',
          body: JSON.stringify(mockAthletes)
        });
      }
    });
  }

  // ===== DASHBOARD MOCKS =====

  async mockDashboard(): Promise<void> {
    // GET widgets
    await this.page.route(`${API_BASE_URL}/widgets`, async (route, request) => {
      if (request.method() === 'GET') {
        await route.fulfill({
          status: 200,
          contentType: 'application/json',
          body: JSON.stringify(mockWidgets)
        });
      } else if (request.method() === 'POST') {
        const body = request.postDataJSON();
        await route.fulfill({
          status: 201,
          contentType: 'application/json',
          body: JSON.stringify({
            ...mockNewWidget,
            name: body.name,
            type: body.type
          })
        });
      }
    });

    // GET stats
    await this.page.route(`${API_BASE_URL}/stats`, async (route) => {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify(mockDashboardStats)
      });
    });
  }

  async mockDashboardEmpty(): Promise<void> {
    await this.page.route(`${API_BASE_URL}/widgets`, async (route) => {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify([])
      });
    });

    await this.page.route(`${API_BASE_URL}/stats`, async (route) => {
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify({
          ...mockDashboardStats,
          totalWorkouts: 0,
          totalExercises: 0,
          totalAthletes: 0,
          workoutsThisWeek: 0,
          workoutsThisMonth: 0
        })
      });
    });
  }

  async mockDashboardError(): Promise<void> {
    await this.page.route(`${API_BASE_URL}/widgets`, async (route) => {
      await route.fulfill({
        status: 500,
        contentType: 'application/json',
        body: JSON.stringify(mockErrorResponses.serverError.body)
      });
    });
  }

  // ===== EXERCISES MOCKS =====

  async mockExercises(): Promise<void> {
    // GET all exercises
    await this.page.route(`${API_BASE_URL}/exercises*`, async (route, request) => {
      if (request.method() === 'GET') {
        await route.fulfill({
          status: 200,
          contentType: 'application/json',
          body: JSON.stringify(mockExercises)
        });
      } else if (request.method() === 'POST') {
        const body = request.postDataJSON();
        await route.fulfill({
          status: 201,
          contentType: 'application/json',
          body: JSON.stringify({
            exerciseId: 4,
            ...body,
            tenantId: 1
          })
        });
      }
    });

    // Body parts
    await this.page.route(`${API_BASE_URL}/bodyparts*`, async (route, request) => {
      if (request.method() === 'GET') {
        await route.fulfill({
          status: 200,
          contentType: 'application/json',
          body: JSON.stringify(mockBodyParts)
        });
      } else if (request.method() === 'POST') {
        const body = request.postDataJSON();
        await route.fulfill({
          status: 201,
          contentType: 'application/json',
          body: JSON.stringify({
            bodyPartId: 6,
            ...body,
            tenantId: 1
          })
        });
      }
    });
  }

  // ===== WORKOUTS MOCKS =====

  async mockWorkouts(): Promise<void> {
    await this.page.route(`${API_BASE_URL}/workouts*`, async (route, request) => {
      if (request.method() === 'GET') {
        await route.fulfill({
          status: 200,
          contentType: 'application/json',
          body: JSON.stringify(mockWorkouts)
        });
      } else if (request.method() === 'POST') {
        const body = request.postDataJSON();
        await route.fulfill({
          status: 201,
          contentType: 'application/json',
          body: JSON.stringify({
            workoutId: 4,
            ...body,
            tenantId: 1,
            createdOn: new Date().toISOString()
          })
        });
      }
    });
  }

  // ===== NOTIFICATIONS MOCKS =====

  async mockNotifications(): Promise<void> {
    await this.page.route(`${API_BASE_URL}/notifications*`, async (route, request) => {
      if (request.method() === 'GET') {
        await route.fulfill({
          status: 200,
          contentType: 'application/json',
          body: JSON.stringify(mockNotifications)
        });
      } else if (request.method() === 'POST') {
        const url = request.url();
        if (url.includes('/read-all')) {
          await route.fulfill({ status: 204, body: '' });
        } else if (url.includes('/read')) {
          await route.fulfill({ status: 204, body: '' });
        } else {
          await route.fulfill({
            status: 201,
            contentType: 'application/json',
            body: JSON.stringify({ notificationId: 3 })
          });
        }
      }
    });
  }

  // ===== MEDIA MOCKS =====

  async mockMedia(): Promise<void> {
    await this.page.route(`${API_BASE_URL}/media*`, async (route, request) => {
      if (request.method() === 'GET') {
        await route.fulfill({
          status: 200,
          contentType: 'application/json',
          body: JSON.stringify([])
        });
      } else if (request.method() === 'POST') {
        await route.fulfill({
          status: 201,
          contentType: 'application/json',
          body: JSON.stringify({
            mediaFileId: 1,
            fileName: 'test.jpg',
            originalFileName: 'test.jpg',
            contentType: 'image/jpeg',
            size: 1024,
            url: 'https://example.com/test.jpg'
          })
        });
      } else if (request.method() === 'DELETE') {
        await route.fulfill({ status: 204, body: '' });
      }
    });
  }

  // ===== UTILITY METHODS =====

  /**
   * Mock a slow API response for testing loading states
   */
  async mockSlowResponse(endpoint: string, delayMs = 2000): Promise<void> {
    await this.page.route(`${API_BASE_URL}${endpoint}`, async (route) => {
      await new Promise(resolve => setTimeout(resolve, delayMs));
      await route.fulfill({
        status: 200,
        contentType: 'application/json',
        body: JSON.stringify([])
      });
    });
  }

  /**
   * Mock network error
   */
  async mockNetworkError(endpoint: string): Promise<void> {
    await this.page.route(`${API_BASE_URL}${endpoint}`, async (route) => {
      await route.abort('failed');
    });
  }

  /**
   * Mock 401 unauthorized for all API calls (session expired)
   */
  async mockSessionExpired(): Promise<void> {
    await this.page.route(`${API_BASE_URL}/**`, async (route) => {
      await route.fulfill({
        status: 401,
        contentType: 'application/json',
        body: JSON.stringify({ message: 'Session expired' })
      });
    });
  }
}

/**
 * Create ApiMocks instance for a page
 */
export function createApiMocks(page: Page): ApiMocks {
  return new ApiMocks(page);
}
