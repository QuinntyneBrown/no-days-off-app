import { test, expect } from '@playwright/test';

test.describe('Dashboard Page', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/dashboard');
  });

  test('should display dashboard page', async ({ page }) => {
    await expect(page.locator('.dashboard-page')).toBeVisible();
  });

  test('should display dashboard header', async ({ page }) => {
    await expect(page.locator('ndo-dashboard-header')).toBeVisible();
  });

  test('should display dashboard tiles', async ({ page }) => {
    await expect(page.locator('ndo-dashboard-grid')).toBeVisible();
  });

  test('should open add tile modal when clicking plus button', async ({ page }) => {
    await page.click('.plus-button');
    await expect(page.locator('.modal-window__backdrop')).toBeVisible();
    await expect(page.locator('.modal-window__title')).toContainText('Add Tile');
  });

  test('should display dashboard name', async ({ page }) => {
    await expect(page.locator('.dashboard-page__title')).toContainText('My Dashboard');
  });
});
