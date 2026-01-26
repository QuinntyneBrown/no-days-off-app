import { test, expect } from '@playwright/test';
import { createApiMocks } from '../helpers/api-mocks';

test.describe('Navigation', () => {
  test.beforeEach(async ({ page }) => {
    const mocks = createApiMocks(page);
    await mocks.setupDefaultMocks();
    await page.goto('/');
  });

  test('should display home page', async ({ page }) => {
    await expect(page.locator('.home-page__title')).toContainText('Welcome to No Days Off');
    await expect(page.locator('.home-page__subtitle')).toContainText('Your personal fitness tracking companion');
  });

  test('should display feature cards on home page', async ({ page }) => {
    await expect(page.locator('.home-page__feature-card')).toHaveCount(3);
    await expect(page.getByText('Track Progress')).toBeVisible();
    await expect(page.getByText('Weekly Planning')).toBeVisible();
    await expect(page.getByText('Video Guidance')).toBeVisible();
  });

  test('should navigate to dashboard via Get Started button', async ({ page }) => {
    await page.click('text=Get Started');
    await expect(page).toHaveURL('/dashboard');
    await expect(page.locator('.dashboard-page')).toBeVisible();
  });

  test('should navigate to athletes via View Athletes button', async ({ page }) => {
    await page.click('text=View Athletes');
    await expect(page).toHaveURL('/athletes');
    await expect(page.locator('.athletes-page')).toBeVisible();
  });

  test('should open and close navigation menu', async ({ page }) => {
    // Open menu
    await page.click('.hamburger-button');
    await expect(page.locator('.left-nav--open')).toBeVisible();

    // Close menu by clicking the backdrop (not hamburger - it's covered by the nav)
    await page.click('.left-nav__backdrop');
    await expect(page.locator('.left-nav--open')).not.toBeVisible();
  });

  test('should navigate to weekly planner via menu', async ({ page }) => {
    await page.click('.hamburger-button');
    await expect(page.locator('.left-nav--open')).toBeVisible();

    await page.click('text=Weekly Planner');
    await expect(page).toHaveURL('/weekly-planner');
    await expect(page.locator('.left-nav--open')).not.toBeVisible();
  });

  test('should navigate to dashboard via menu', async ({ page }) => {
    await page.click('.hamburger-button');
    await page.click('text=Dashboard');
    await expect(page).toHaveURL('/dashboard');
  });

  test('should navigate to athletes via menu', async ({ page }) => {
    await page.click('.hamburger-button');
    await page.click('text=Athletes');
    await expect(page).toHaveURL('/athletes');
  });

  test('should close menu when navigating', async ({ page }) => {
    await page.click('.hamburger-button');
    await expect(page.locator('.left-nav--open')).toBeVisible();

    await page.click('text=Dashboard');
    await expect(page.locator('.left-nav--open')).not.toBeVisible();
  });

  test('should handle direct URL navigation', async ({ page }) => {
    await page.goto('/dashboard');
    await expect(page.locator('.dashboard-page')).toBeVisible();

    await page.goto('/athletes');
    await expect(page.locator('.athletes-page')).toBeVisible();

    await page.goto('/weekly-planner');
    await expect(page.locator('.weekly-planner-page')).toBeVisible();
  });

  test('should redirect unknown routes to home', async ({ page }) => {
    await page.goto('/unknown-route');
    await expect(page).toHaveURL('/');
    await expect(page.locator('.home-page')).toBeVisible();
  });

  test('should display header on all pages', async ({ page }) => {
    await expect(page.locator('ndo-header')).toBeVisible();

    await page.goto('/dashboard');
    await expect(page.locator('ndo-header')).toBeVisible();

    await page.goto('/athletes');
    await expect(page.locator('ndo-header')).toBeVisible();
  });

  test('should use browser back navigation', async ({ page }) => {
    await page.click('text=Get Started');
    await expect(page).toHaveURL('/dashboard');

    await page.goBack();
    await expect(page).toHaveURL('/');
  });
});
