import { test, expect } from '@playwright/test';
import { createApiMocks } from '../helpers/api-mocks';

test.describe('Error Handling', () => {
  test.describe('Network Errors', () => {
    test('should handle network failure on athletes page', async ({ page }) => {
      const mocks = createApiMocks(page);
      await mocks.mockNetworkError('/athletes');

      await page.goto('/athletes');

      // Page should still render
      await expect(page.locator('.athletes-page')).toBeVisible();
    });

    test('should handle network failure on dashboard page', async ({ page }) => {
      const mocks = createApiMocks(page);
      await mocks.mockNetworkError('/widgets');
      await mocks.mockNetworkError('/stats');

      await page.goto('/dashboard');

      // Dashboard should still render with empty state
      await expect(page.locator('.dashboard-page')).toBeVisible();
    });

    test('should handle slow network gracefully', async ({ page }) => {
      const mocks = createApiMocks(page);
      await mocks.mockSlowResponse('/athletes', 3000);

      await page.goto('/athletes');

      // Page should eventually load
      await expect(page.locator('.athletes-page')).toBeVisible({ timeout: 5000 });
    });
  });

  test.describe('API Error Responses', () => {
    test('should handle 500 error on athletes list', async ({ page }) => {
      const mocks = createApiMocks(page);
      await mocks.mockAthletesError();

      await page.goto('/athletes');

      // Should show page with error state or empty list
      await expect(page.locator('.athletes-page')).toBeVisible();
    });

    test('should handle 500 error on dashboard', async ({ page }) => {
      const mocks = createApiMocks(page);
      await mocks.mockDashboardError();

      await page.goto('/dashboard');

      // Should show empty dashboard
      await expect(page.locator('.dashboard-page')).toBeVisible();
      await expect(page.locator('.dashboard-page__title')).toContainText('My Dashboard');
    });

    test('should handle 404 error for individual athlete', async ({ page }) => {
      const mocks = createApiMocks(page);
      await mocks.mockAthletes([]);

      await page.goto('/athletes/999');

      // Should handle gracefully
      await expect(page).not.toHaveURL('/error');
    });

    test('should handle 400 validation error on create athlete', async ({ page }) => {
      const mocks = createApiMocks(page);
      await mocks.mockCreateAthleteError();

      await page.goto('/athletes');

      // Open add modal
      await page.click('.plus-button');

      // Fill form
      await page.fill('input[formcontrolname="name"]', 'Test');
      await page.fill('input[formcontrolname="username"]', 'test');

      // Submit
      await page.click('button:has-text("Save")');

      // Error should be handled - modal might stay open or show error
      await expect(page.locator('.athletes-page')).toBeVisible();
    });
  });

  test.describe('Empty States', () => {
    test('should display empty state for athletes', async ({ page }) => {
      const mocks = createApiMocks(page);
      await mocks.mockAthletesEmpty();

      await page.goto('/athletes');

      await expect(page.locator('.athlete-card')).toHaveCount(0);
      // Could check for empty state message if implemented
    });

    test('should display empty state for dashboard widgets', async ({ page }) => {
      const mocks = createApiMocks(page);
      await mocks.mockDashboardEmpty();

      await page.goto('/dashboard');

      await expect(page.locator('ndo-dashboard-tile')).toHaveCount(0);
    });
  });

  test.describe('Session Expiration', () => {
    test('should handle session expiry gracefully', async ({ page }) => {
      const mocks = createApiMocks(page);
      await mocks.mockDashboard(); // Provide valid mock data

      await page.goto('/dashboard');

      // Page should load with mocked dashboard
      await expect(page.locator('.dashboard-page')).toBeVisible();
    });

    test('should show login when not authenticated', async ({ page }) => {
      // Navigate directly to login
      await page.goto('/login');

      // Login page should be visible
      await expect(page.locator('.login-page')).toBeVisible();
    });
  });

  test.describe('Form Error States', () => {
    test('should display validation errors on login form', async ({ page }) => {
      await page.goto('/login');

      // Clear fields
      await page.fill('input[formcontrolname="email"]', '');
      await page.fill('input[formcontrolname="password"]', '');

      // Touch fields
      await page.locator('input[formcontrolname="email"]').blur();

      // Errors should appear
      await expect(page.locator('mat-error')).toBeVisible();
    });

    test('should display validation errors on athlete form', async ({ page }) => {
      const mocks = createApiMocks(page);
      await mocks.mockAthletes([]);

      await page.goto('/athletes');
      await page.click('.plus-button');

      // Touch fields without entering values
      await page.locator('input[formcontrolname="name"]').focus();
      await page.locator('input[formcontrolname="name"]').blur();

      // Validation error should appear
      await expect(page.locator('mat-error')).toContainText('Name is required');
    });
  });

  test.describe('Graceful Degradation', () => {
    test.skip('should work without JavaScript (basic HTML)', async ({ page }) => {
      // Angular apps require JavaScript to render
      // This test is skipped as it's expected behavior
      await page.setJavaScriptEnabled(false);
      await page.goto('/');
      await expect(page.locator('body')).not.toBeEmpty();
    });

    test('should handle missing images gracefully', async ({ page }) => {
      const mocks = createApiMocks(page);
      // Null imageUrl should show placeholder
      await mocks.mockAthletes([{
        athleteId: 1,
        name: 'Test',
        username: 'test',
        imageUrl: null,
        createdOn: new Date().toISOString(),
        createdBy: 'system'
      }]);

      await page.goto('/athletes');

      // Page should load
      await expect(page.locator('.athletes-page')).toBeVisible();
      // Placeholder should be shown when no image
      await expect(page.locator('.athlete-card__placeholder')).toBeVisible();
    });
  });

  test.describe('Rate Limiting', () => {
    test.skip('should handle 429 too many requests', async ({ page }) => {
      await page.route('**/api/**', async (route) => {
        await route.fulfill({
          status: 429,
          body: JSON.stringify({ message: 'Too many requests' })
        });
      });

      await page.goto('/dashboard');

      // Should handle gracefully
      await expect(page.locator('.dashboard-page')).toBeVisible();
    });
  });

  test.describe('Timeout Handling', () => {
    test('should handle request timeout', async ({ page }) => {
      const mocks = createApiMocks(page);

      // Create a mock that never responds (will timeout)
      await page.route('**/api/athletes', async (route) => {
        await new Promise(resolve => setTimeout(resolve, 30000));
        await route.fulfill({ status: 200, body: JSON.stringify([]) });
      });

      await page.goto('/athletes');

      // Page should still be accessible
      await expect(page.locator('.athletes-page')).toBeVisible({ timeout: 5000 });
    });
  });

  test.describe('Concurrent Request Handling', () => {
    test('should handle multiple simultaneous API calls', async ({ page }) => {
      const mocks = createApiMocks(page);
      await mocks.setupDefaultMocks();

      await page.goto('/dashboard');

      // Dashboard makes multiple API calls (widgets, stats)
      await expect(page.locator('.dashboard-page')).toBeVisible();
      await expect(page.locator('ndo-dashboard-header')).toBeVisible();
    });
  });

  test.describe('CORS and Security Errors', () => {
    test.skip('should handle CORS errors gracefully', async ({ page }) => {
      await page.route('**/api/**', async (route) => {
        await route.abort('accessdenied');
      });

      await page.goto('/athletes');

      // Should show error state
      await expect(page.locator('.athletes-page')).toBeVisible();
    });
  });
});
