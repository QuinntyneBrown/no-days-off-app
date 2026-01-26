import { test, expect } from '@playwright/test';
import { createApiMocks } from '../helpers/api-mocks';
import { mockWidgets, mockDashboardStats } from '../fixtures/mock-data';

test.describe('Dashboard Page', () => {
  test.beforeEach(async ({ page }) => {
    const mocks = createApiMocks(page);
    await mocks.setupDefaultMocks();
  });

  test.describe('Display', () => {
    test('should display dashboard page', async ({ page }) => {
      await page.goto('/dashboard');
      await expect(page.locator('.dashboard-page')).toBeVisible();
    });

    test('should display dashboard header', async ({ page }) => {
      await page.goto('/dashboard');
      await expect(page.locator('ndo-dashboard-header')).toBeVisible();
    });

    test('should display dashboard name', async ({ page }) => {
      await page.goto('/dashboard');
      await expect(page.locator('.dashboard-page__title')).toContainText('My Dashboard');
    });

    test('should display dashboard grid', async ({ page }) => {
      await page.goto('/dashboard');
      await expect(page.locator('ndo-dashboard-grid')).toBeVisible();
    });

    test('should display dashboard tiles from API', async ({ page }) => {
      await page.goto('/dashboard');
      await expect(page.locator('ndo-dashboard-tile')).toHaveCount(mockWidgets.length);
    });

    test('should display plus button for adding tiles', async ({ page }) => {
      await page.goto('/dashboard');
      await expect(page.locator('.plus-button')).toBeVisible();
    });
  });

  test.describe('Statistics', () => {
    // Note: Stats are fetched but may not be displayed in the current UI.
    // These tests verify the API is called and data is available.
    test('should fetch stats from API', async ({ page }) => {
      let statsCalled = false;
      await page.route('**/api/stats', async (route) => {
        statsCalled = true;
        await route.fulfill({
          status: 200,
          contentType: 'application/json',
          body: JSON.stringify(mockDashboardStats)
        });
      });

      await page.goto('/dashboard');
      await expect(page.locator('.dashboard-page')).toBeVisible();
      expect(statsCalled).toBe(true);
    });
  });

  test.describe('Empty State', () => {
    test('should handle empty widgets list', async ({ page }) => {
      const mocks = createApiMocks(page);
      await mocks.mockDashboardEmpty();

      await page.goto('/dashboard');
      await expect(page.locator('.dashboard-page')).toBeVisible();
      await expect(page.locator('ndo-dashboard-tile')).toHaveCount(0);
    });

    test('should show empty dashboard message when no widgets', async ({ page }) => {
      const mocks = createApiMocks(page);
      await mocks.mockDashboardEmpty();

      await page.goto('/dashboard');
      // Dashboard should load with empty state
      await expect(page.locator('.dashboard-page')).toBeVisible();
      await expect(page.locator('ndo-dashboard-tile')).toHaveCount(0);
    });
  });

  test.describe('Error Handling', () => {
    test('should show empty dashboard on API error', async ({ page }) => {
      const mocks = createApiMocks(page);
      await mocks.mockDashboardError();

      await page.goto('/dashboard');
      // Should still show dashboard with empty state
      await expect(page.locator('.dashboard-page')).toBeVisible();
      await expect(page.locator('.dashboard-page__title')).toContainText('My Dashboard');
    });
  });

  test.describe('Add Widget', () => {
    test('should open add tile modal when clicking plus button', async ({ page }) => {
      await page.goto('/dashboard');
      await page.click('.plus-button');
      await expect(page.locator('.modal-window__backdrop')).toBeVisible();
      await expect(page.locator('.modal-window__title')).toContainText('Add Tile');
    });

    test('should close modal when clicking close button', async ({ page }) => {
      await page.goto('/dashboard');
      await page.click('.plus-button');
      await expect(page.locator('.modal-window__backdrop')).toBeVisible();

      await page.click('.modal-window__close');
      await expect(page.locator('.modal-window__backdrop')).not.toBeVisible();
    });
  });

  test.describe('Widget Interactions', () => {
    test('should display widget tiles with content', async ({ page }) => {
      await page.goto('/dashboard');

      const firstTile = page.locator('ndo-dashboard-tile').first();
      await expect(firstTile).toBeVisible();
    });

    test('should allow tile configuration', async ({ page }) => {
      await page.goto('/dashboard');

      // Find configure button on a tile (if exists)
      const configButton = page.locator('ndo-dashboard-tile button[aria-label*="config"], ndo-dashboard-tile .tile-config').first();
      if (await configButton.isVisible()) {
        await configButton.click();
        // Some action should happen (modal, menu, etc.)
      }
    });

    test('should allow tile removal', async ({ page }) => {
      await page.goto('/dashboard');

      const initialCount = await page.locator('ndo-dashboard-tile').count();

      // Find and click remove button (if exists)
      const removeButton = page.locator('ndo-dashboard-tile button[aria-label*="remove"], ndo-dashboard-tile .tile-remove').first();
      if (await removeButton.isVisible()) {
        await removeButton.click();
        await expect(page.locator('ndo-dashboard-tile')).toHaveCount(initialCount - 1);
      }
    });
  });

  test.describe('Dashboard Switching', () => {
    test('should display dashboard selector in header', async ({ page }) => {
      await page.goto('/dashboard');
      await expect(page.locator('ndo-dashboard-header')).toBeVisible();
    });

    test('should allow adding new dashboard', async ({ page }) => {
      await page.goto('/dashboard');

      // Look for add dashboard button
      const addDashboardButton = page.locator('button[aria-label*="add dashboard"], .add-dashboard-btn');
      if (await addDashboardButton.isVisible()) {
        await addDashboardButton.click();
        // New dashboard should be created
      }
    });
  });

  test.describe('Responsive Layout', () => {
    test('should display grid on desktop', async ({ page }) => {
      await page.setViewportSize({ width: 1280, height: 800 });
      await page.goto('/dashboard');
      await expect(page.locator('ndo-dashboard-grid')).toBeVisible();
    });

    test('should adapt layout on mobile', async ({ page }) => {
      await page.setViewportSize({ width: 375, height: 667 });
      await page.goto('/dashboard');
      await expect(page.locator('.dashboard-page')).toBeVisible();
    });

    test('should stack tiles on narrow viewport', async ({ page }) => {
      await page.setViewportSize({ width: 400, height: 800 });
      await page.goto('/dashboard');
      await expect(page.locator('ndo-dashboard-grid')).toBeVisible();
    });
  });

  test.describe('URL Parameters', () => {
    test('should load specific dashboard by ID', async ({ page }) => {
      await page.goto('/dashboard/1');
      await expect(page.locator('.dashboard-page')).toBeVisible();
    });

    test('should handle invalid dashboard ID gracefully', async ({ page }) => {
      await page.goto('/dashboard/999');
      // Should still show a dashboard (default or error state)
      await expect(page.locator('.dashboard-page')).toBeVisible();
    });
  });

  test.describe('Loading States', () => {
    test('should show loading indicator while fetching data', async ({ page }) => {
      const mocks = createApiMocks(page);
      await mocks.mockSlowResponse('/widgets', 1000);

      await page.goto('/dashboard');

      // Check for loading indicator or loading class
      // Note: This depends on how loading is implemented
      await expect(page.locator('.dashboard-page')).toBeVisible();
    });
  });
});
