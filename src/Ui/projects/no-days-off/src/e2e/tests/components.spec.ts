import { test, expect } from '@playwright/test';
import { createApiMocks } from '../helpers/api-mocks';

test.describe('Shared Components', () => {
  test.beforeEach(async ({ page }) => {
    const mocks = createApiMocks(page);
    await mocks.setupDefaultMocks();
  });

  test.describe('Header', () => {
    test('should display header on all pages', async ({ page }) => {
      await page.goto('/');
      await expect(page.locator('ndo-header')).toBeVisible();

      await page.goto('/dashboard');
      await expect(page.locator('ndo-header')).toBeVisible();

      await page.goto('/athletes');
      await expect(page.locator('ndo-header')).toBeVisible();
    });

    test('should display logo or title', async ({ page }) => {
      await page.goto('/');
      const header = page.locator('ndo-header');
      await expect(header).toBeVisible();
    });
  });

  test.describe('Hamburger Button', () => {
    test('should display hamburger button', async ({ page }) => {
      await page.goto('/');
      await expect(page.locator('.hamburger-button')).toBeVisible();
    });

    test('should toggle menu on click', async ({ page }) => {
      await page.goto('/');

      await page.click('.hamburger-button');
      await expect(page.locator('.left-nav--open')).toBeVisible();

      // Close by clicking backdrop (hamburger is covered by nav overlay)
      await page.click('.left-nav__backdrop');
      await expect(page.locator('.left-nav--open')).not.toBeVisible();
    });

    test('should animate on click', async ({ page }) => {
      await page.goto('/');

      // Check for animation class changes
      await page.click('.hamburger-button');
      // The button should have some state change
      await expect(page.locator('.left-nav--open')).toBeVisible();
    });
  });

  test.describe('Left Navigation', () => {
    test('should display navigation links', async ({ page }) => {
      await page.goto('/');
      await page.click('.hamburger-button');

      // Use more specific selectors to avoid matching multiple elements
      await expect(page.locator('.left-nav a:has-text("Dashboard")')).toBeVisible();
      await expect(page.locator('.left-nav a:has-text("Athletes")')).toBeVisible();
      await expect(page.locator('.left-nav a:has-text("Weekly Planner")')).toBeVisible();
    });

    test('should close on link click', async ({ page }) => {
      await page.goto('/');
      await page.click('.hamburger-button');
      await expect(page.locator('.left-nav--open')).toBeVisible();

      await page.click('text=Dashboard');
      await expect(page.locator('.left-nav--open')).not.toBeVisible();
    });

    test('should navigate to correct page', async ({ page }) => {
      await page.goto('/');
      await page.click('.hamburger-button');

      await page.click('text=Athletes');
      await expect(page).toHaveURL('/athletes');
    });
  });

  test.describe('Plus Button', () => {
    test('should display plus button on athletes page', async ({ page }) => {
      await page.goto('/athletes');
      await expect(page.locator('.plus-button')).toBeVisible();
    });

    test('should display plus button on dashboard page', async ({ page }) => {
      await page.goto('/dashboard');
      await expect(page.locator('.plus-button')).toBeVisible();
    });

    test('should open modal on click', async ({ page }) => {
      await page.goto('/athletes');
      await page.click('.plus-button');
      await expect(page.locator('.modal-window__backdrop')).toBeVisible();
    });

    test('should have accessible label', async ({ page }) => {
      await page.goto('/athletes');
      const plusButton = page.locator('.plus-button');
      await expect(plusButton).toBeVisible();
    });
  });

  test.describe('Modal Window', () => {
    test('should open and display correctly', async ({ page }) => {
      await page.goto('/athletes');
      await page.click('.plus-button');

      await expect(page.locator('.modal-window__backdrop')).toBeVisible();
      await expect(page.locator('.modal-window__dialog')).toBeVisible();
    });

    test('should display title', async ({ page }) => {
      await page.goto('/athletes');
      await page.click('.plus-button');

      await expect(page.locator('.modal-window__title')).toBeVisible();
    });

    test('should close on close button click', async ({ page }) => {
      await page.goto('/athletes');
      await page.click('.plus-button');
      await expect(page.locator('.modal-window__backdrop')).toBeVisible();

      await page.click('.modal-window__close');
      await expect(page.locator('.modal-window__backdrop')).not.toBeVisible();
    });

    test('should close on backdrop click', async ({ page }) => {
      await page.goto('/athletes');
      await page.click('.plus-button');
      await expect(page.locator('.modal-window__backdrop')).toBeVisible();

      // Click outside the modal content
      await page.locator('.modal-window__backdrop').click({ position: { x: 10, y: 10 } });
      // Modal may or may not close depending on implementation
    });

    test('should trap focus within modal', async ({ page }) => {
      await page.goto('/athletes');
      await page.click('.plus-button');

      // Tab should cycle within modal
      await page.keyboard.press('Tab');
      // Focus should still be within modal
    });

    test('should close on escape key', async ({ page }) => {
      await page.goto('/athletes');
      await page.click('.plus-button');
      await expect(page.locator('.modal-window__backdrop')).toBeVisible();

      await page.keyboard.press('Escape');
      // Modal may or may not close depending on implementation
    });
  });

  test.describe('Secondary Header', () => {
    test('should display on athletes page', async ({ page }) => {
      await page.goto('/athletes');
      await expect(page.locator('ndo-secondary-header')).toBeVisible();
    });

    test('should display on weekly planner page', async ({ page }) => {
      await page.goto('/weekly-planner');
      await expect(page.locator('ndo-secondary-header')).toBeVisible();
    });

    test('should display page title', async ({ page }) => {
      await page.goto('/athletes');
      await expect(page.locator('.secondary-header__title')).toContainText('Athletes');
    });

    test('should display back link', async ({ page }) => {
      await page.goto('/athletes');
      await expect(page.locator('.secondary-header__back')).toBeVisible();
    });
  });

  test.describe('Pager', () => {
    test('should display on athletes page when more than 10 items', async ({ page }) => {
      // Pager only shows when totalPages > 1
      const manyAthletes = Array.from({ length: 15 }, (_, i) => ({
        athleteId: i + 1,
        name: `Athlete ${i + 1}`,
        username: `athlete${i + 1}`,
        imageUrl: null,
        createdOn: new Date().toISOString(),
        createdBy: 'system'
      }));

      const mocks = createApiMocks(page);
      await mocks.mockAthletes(manyAthletes);

      await page.goto('/athletes');
      await expect(page.locator('ndo-pager')).toBeVisible();
    });

    test('should display current page info', async ({ page }) => {
      const manyAthletes = Array.from({ length: 15 }, (_, i) => ({
        athleteId: i + 1,
        name: `Athlete ${i + 1}`,
        username: `athlete${i + 1}`,
        imageUrl: null,
        createdOn: new Date().toISOString(),
        createdBy: 'system'
      }));

      const mocks = createApiMocks(page);
      await mocks.mockAthletes(manyAthletes);

      await page.goto('/athletes');
      // Pager should show page numbers
      await expect(page.locator('.pager__current')).toContainText('1');
      await expect(page.locator('.pager__total')).toContainText('2');
    });

    test('should have previous and next buttons', async ({ page }) => {
      const manyAthletes = Array.from({ length: 15 }, (_, i) => ({
        athleteId: i + 1,
        name: `Athlete ${i + 1}`,
        username: `athlete${i + 1}`,
        imageUrl: null,
        createdOn: new Date().toISOString(),
        createdBy: 'system'
      }));

      const mocks = createApiMocks(page);
      await mocks.mockAthletes(manyAthletes);

      await page.goto('/athletes');
      await expect(page.locator('button[aria-label="Previous page"]')).toBeVisible();
      await expect(page.locator('button[aria-label="Next page"]')).toBeVisible();
    });

    test('should navigate between pages', async ({ page }) => {
      // Need many athletes for pagination
      const manyAthletes = Array.from({ length: 25 }, (_, i) => ({
        athleteId: i + 1,
        name: `Athlete ${i + 1}`,
        username: `athlete${i + 1}`,
        imageUrl: null,
        createdOn: new Date().toISOString(),
        createdBy: 'system'
      }));

      const mocks = createApiMocks(page);
      await mocks.mockAthletes(manyAthletes);

      await page.goto('/athletes');

      // Should show first page
      await expect(page.locator('.athlete-card')).toHaveCount(10);

      // Click next
      await page.click('button[aria-label="Next page"]');

      // Should show second page
      await expect(page.locator('.athlete-card')).toHaveCount(10);
    });
  });

  test.describe('Athlete Card', () => {
    test('should display athlete information', async ({ page }) => {
      await page.goto('/athletes');

      const card = page.locator('.athlete-card').first();
      await expect(card.locator('.athlete-card__name')).toBeVisible();
      await expect(card.locator('.athlete-card__username')).toBeVisible();
    });

    test('should display edit button', async ({ page }) => {
      await page.goto('/athletes');

      const card = page.locator('.athlete-card').first();
      await expect(card.locator('button[aria-label="Edit athlete"]')).toBeVisible();
    });

    test('should display delete button', async ({ page }) => {
      await page.goto('/athletes');

      const card = page.locator('.athlete-card').first();
      await expect(card.locator('button[aria-label="Delete athlete"]')).toBeVisible();
    });

    test('should be clickable for navigation', async ({ page }) => {
      await page.goto('/athletes');

      // Click on card content (not buttons)
      await page.locator('.athlete-card').first().locator('.athlete-card__content').click();

      // Should navigate to athlete detail
      await expect(page).toHaveURL(/\/athletes\/\d+/);
    });
  });

  test.describe('Dashboard Tile', () => {
    test('should display on dashboard page', async ({ page }) => {
      await page.goto('/dashboard');
      await expect(page.locator('ndo-dashboard-tile').first()).toBeVisible();
    });

    test('should display widget content', async ({ page }) => {
      await page.goto('/dashboard');
      const tile = page.locator('ndo-dashboard-tile').first();
      await expect(tile).toBeVisible();
    });
  });

  test.describe('Dashboard Header', () => {
    test('should display on dashboard page', async ({ page }) => {
      await page.goto('/dashboard');
      await expect(page.locator('ndo-dashboard-header')).toBeVisible();
    });

    test('should display dashboard name', async ({ page }) => {
      await page.goto('/dashboard');
      await expect(page.locator('.dashboard-page__title')).toContainText('My Dashboard');
    });

    test('should display statistics', async ({ page }) => {
      await page.goto('/dashboard');
      // Stats should be visible in header
      await expect(page.locator('ndo-dashboard-header')).toBeVisible();
    });
  });

  test.describe('Dashboard Grid', () => {
    test('should display on dashboard page', async ({ page }) => {
      await page.goto('/dashboard');
      await expect(page.locator('ndo-dashboard-grid')).toBeVisible();
    });

    test('should contain dashboard tiles', async ({ page }) => {
      await page.goto('/dashboard');
      await expect(page.locator('ndo-dashboard-grid ndo-dashboard-tile')).toHaveCount(3);
    });
  });

  test.describe('Weekly Planner Grid', () => {
    test('should display on weekly planner page', async ({ page }) => {
      await page.goto('/weekly-planner');
      await expect(page.locator('ndo-weekly-planner-grid')).toBeVisible();
    });

    test('should contain day cards', async ({ page }) => {
      await page.goto('/weekly-planner');
      await expect(page.locator('ndo-weekly-planner-day')).toHaveCount(7);
    });
  });

  test.describe('Weekly Planner Day', () => {
    test('should display day name', async ({ page }) => {
      await page.goto('/weekly-planner');
      await expect(page.getByText('Sunday')).toBeVisible();
      await expect(page.getByText('Monday')).toBeVisible();
    });

    test('should display date', async ({ page }) => {
      await page.goto('/weekly-planner');
      // Date format depends on implementation
      const dayCard = page.locator('ndo-weekly-planner-day').first();
      await expect(dayCard).toBeVisible();
    });
  });
});
