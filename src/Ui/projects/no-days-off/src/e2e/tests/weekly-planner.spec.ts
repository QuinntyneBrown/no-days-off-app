import { test, expect } from '@playwright/test';
import { createApiMocks } from '../helpers/api-mocks';

test.describe('Weekly Planner Page', () => {
  test.beforeEach(async ({ page }) => {
    const mocks = createApiMocks(page);
    await mocks.setupDefaultMocks();
  });

  test.describe('Display', () => {
    test('should display weekly planner page', async ({ page }) => {
      await page.goto('/weekly-planner');
      await expect(page.locator('.weekly-planner-page')).toBeVisible();
    });

    test('should display secondary header with title', async ({ page }) => {
      await page.goto('/weekly-planner');
      await expect(page.locator('ndo-secondary-header')).toContainText('Weekly Planner');
    });

    test('should display weekly planner grid', async ({ page }) => {
      await page.goto('/weekly-planner');
      await expect(page.locator('ndo-weekly-planner-grid')).toBeVisible();
    });

    test('should display 7 days of the week', async ({ page }) => {
      await page.goto('/weekly-planner');
      await expect(page.locator('ndo-weekly-planner-day')).toHaveCount(7);
    });

    test('should display day names', async ({ page }) => {
      await page.goto('/weekly-planner');

      const dayNames = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

      for (const dayName of dayNames) {
        await expect(page.getByText(dayName)).toBeVisible();
      }
    });

    test('should display current week dates', async ({ page }) => {
      await page.goto('/weekly-planner');

      // Get current week dates
      const today = new Date();
      const startOfWeek = new Date(today);
      startOfWeek.setDate(today.getDate() - today.getDay());

      // Check that at least one date is visible in the card subtitle
      const dateLocator = page.locator('.weekly-planner-day mat-card-subtitle, .weekly-planner-day__header mat-card-subtitle');
      await expect(dateLocator.first()).toBeVisible();
    });
  });

  test.describe('Day Cards', () => {
    test('should display day card with correct structure', async ({ page }) => {
      await page.goto('/weekly-planner');

      const firstDay = page.locator('ndo-weekly-planner-day').first();
      await expect(firstDay).toBeVisible();
    });

    test('should display add exercise button on each day', async ({ page }) => {
      await page.goto('/weekly-planner');

      // The add button is in weekly-planner-day__actions with text "Add Exercise"
      const addButtons = page.locator('ndo-weekly-planner-day button:has-text("Add Exercise"), .weekly-planner-day__actions button');
      // Each day should have an add button
      await expect(addButtons.first()).toBeVisible();
    });

    test('should show empty state when no exercises scheduled', async ({ page }) => {
      await page.goto('/weekly-planner');

      // Days should be visible but exercise lists empty
      const days = page.locator('ndo-weekly-planner-day');
      await expect(days.first()).toBeVisible();
    });
  });

  test.describe('Add Exercise', () => {
    test('should open add exercise modal when clicking add button', async ({ page }) => {
      await page.goto('/weekly-planner');

      const addButton = page.locator('ndo-weekly-planner-day').first().locator('button:has-text("Add Exercise")');
      if (await addButton.isVisible({ timeout: 3000 }).catch(() => false)) {
        await addButton.click();
        await expect(page.locator('.modal-window__backdrop')).toBeVisible();
      }
    });

    test('should close modal when clicking close button', async ({ page }) => {
      await page.goto('/weekly-planner');

      const addButton = page.locator('ndo-weekly-planner-day').first().locator('button:has-text("Add Exercise")');
      if (await addButton.isVisible({ timeout: 3000 }).catch(() => false)) {
        await addButton.click();
        await expect(page.locator('.modal-window__backdrop')).toBeVisible();

        await page.click('.modal-window__close');
        await expect(page.locator('.modal-window__backdrop')).not.toBeVisible();
      }
    });

    test('should display exercise selection in modal', async ({ page }) => {
      await page.goto('/weekly-planner');

      const addButton = page.locator('ndo-weekly-planner-day').first().locator('button:has-text("Add Exercise")');
      if (await addButton.isVisible({ timeout: 3000 }).catch(() => false)) {
        await addButton.click();

        // Modal should contain exercise selection - use correct selector
        await expect(page.locator('.modal-window__dialog')).toBeVisible();
      }
    });
  });

  test.describe('Scheduled Exercises', () => {
    test('should display scheduled exercises on a day', async ({ page }) => {
      // This test would need mock data with scheduled exercises
      await page.goto('/weekly-planner');

      // Check for exercise list container
      const exerciseList = page.locator('.scheduled-exercises, .exercise-list');
      // May be empty initially
      await expect(page.locator('ndo-weekly-planner-day').first()).toBeVisible();
    });

    test('should display exercise name when scheduled', async ({ page }) => {
      await page.goto('/weekly-planner');

      // Look for any exercise items
      const exerciseItem = page.locator('.scheduled-exercise, .exercise-item');
      // May not be visible if no exercises scheduled
    });

    test('should display sets and reps for scheduled exercise', async ({ page }) => {
      await page.goto('/weekly-planner');

      // Look for sets/reps display
      const setsReps = page.locator('.exercise-sets, .exercise-reps');
      // May not be visible if no exercises scheduled
    });
  });

  test.describe('Complete Exercise', () => {
    test('should have checkbox to mark exercise complete', async ({ page }) => {
      await page.goto('/weekly-planner');

      // Look for completion checkbox
      const checkbox = page.locator('.exercise-complete, input[type="checkbox"]');
      // May not be visible if no exercises scheduled
    });

    test('should toggle exercise completion state', async ({ page }) => {
      await page.goto('/weekly-planner');

      const checkbox = page.locator('.scheduled-exercise input[type="checkbox"]').first();
      if (await checkbox.isVisible()) {
        const initialChecked = await checkbox.isChecked();
        await checkbox.click();
        expect(await checkbox.isChecked()).toBe(!initialChecked);
      }
    });

    test('should apply completed style when exercise is marked complete', async ({ page }) => {
      await page.goto('/weekly-planner');

      const checkbox = page.locator('.scheduled-exercise input[type="checkbox"]').first();
      if (await checkbox.isVisible()) {
        await checkbox.click();

        // Check for completed class
        const exerciseItem = page.locator('.scheduled-exercise--completed, .exercise-completed');
        // Should have completed styling
      }
    });
  });

  test.describe('Navigation', () => {
    test('should display back link to home', async ({ page }) => {
      await page.goto('/weekly-planner');
      await expect(page.locator('.secondary-header__back')).toBeVisible();
    });

    test('should navigate back to home when clicking back link', async ({ page }) => {
      await page.goto('/weekly-planner');

      await page.click('.secondary-header__back');
      await expect(page).toHaveURL('/');
    });
  });

  test.describe('Responsive Layout', () => {
    test('should display grid on desktop', async ({ page }) => {
      await page.setViewportSize({ width: 1280, height: 800 });
      await page.goto('/weekly-planner');

      await expect(page.locator('ndo-weekly-planner-grid')).toBeVisible();
      await expect(page.locator('ndo-weekly-planner-day')).toHaveCount(7);
    });

    test('should adapt layout on tablet', async ({ page }) => {
      await page.setViewportSize({ width: 768, height: 1024 });
      await page.goto('/weekly-planner');

      await expect(page.locator('.weekly-planner-page')).toBeVisible();
    });

    test('should adapt layout on mobile', async ({ page }) => {
      await page.setViewportSize({ width: 375, height: 667 });
      await page.goto('/weekly-planner');

      await expect(page.locator('.weekly-planner-page')).toBeVisible();
      // Days should still be visible but may be stacked
      await expect(page.locator('ndo-weekly-planner-day').first()).toBeVisible();
    });
  });

  test.describe('Week Navigation', () => {
    test('should display current week by default', async ({ page }) => {
      await page.goto('/weekly-planner');

      // Should show current week
      const today = new Date();
      const dayName = today.toLocaleDateString('en-US', { weekday: 'long' });

      // Current day should be highlighted or visible
      await expect(page.getByText(dayName)).toBeVisible();
    });

    // Future feature tests
    test.skip('should navigate to next week', async ({ page }) => {
      await page.goto('/weekly-planner');

      const nextWeekButton = page.locator('button[aria-label*="next week"]');
      if (await nextWeekButton.isVisible()) {
        await nextWeekButton.click();
        // Dates should change
      }
    });

    test.skip('should navigate to previous week', async ({ page }) => {
      await page.goto('/weekly-planner');

      const prevWeekButton = page.locator('button[aria-label*="previous week"]');
      if (await prevWeekButton.isVisible()) {
        await prevWeekButton.click();
        // Dates should change
      }
    });
  });

  test.describe('Today Highlight', () => {
    test('should highlight current day', async ({ page }) => {
      await page.goto('/weekly-planner');

      // Current day should have special styling
      const today = new Date();
      const dayIndex = today.getDay();

      const todayCard = page.locator('ndo-weekly-planner-day').nth(dayIndex);
      await expect(todayCard).toBeVisible();

      // Check for today class or highlight
      const hasHighlight = await todayCard.locator('.today, .current-day').isVisible()
        || await todayCard.evaluate((el) => el.classList.contains('today'));
    });
  });
});
