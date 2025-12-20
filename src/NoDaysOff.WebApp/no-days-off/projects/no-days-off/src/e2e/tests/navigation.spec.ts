import { test, expect } from '@playwright/test';

test.describe('Navigation', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/');
  });

  test('should display home page', async ({ page }) => {
    await expect(page.locator('.home-page__title')).toContainText('Welcome to No Days Off');
  });

  test('should navigate to dashboard', async ({ page }) => {
    await page.click('text=Get Started');
    await expect(page).toHaveURL('/dashboard');
    await expect(page.locator('.dashboard-page')).toBeVisible();
  });

  test('should navigate to athletes', async ({ page }) => {
    await page.click('text=View Athletes');
    await expect(page).toHaveURL('/athletes');
    await expect(page.locator('.athletes-page')).toBeVisible();
  });

  test('should open and close navigation menu', async ({ page }) => {
    // Open menu
    await page.click('.hamburger-button');
    await expect(page.locator('.left-nav--open')).toBeVisible();

    // Navigate to weekly planner
    await page.click('text=Weekly Planner');
    await expect(page).toHaveURL('/weekly-planner');
    await expect(page.locator('.left-nav--open')).not.toBeVisible();
  });
});
