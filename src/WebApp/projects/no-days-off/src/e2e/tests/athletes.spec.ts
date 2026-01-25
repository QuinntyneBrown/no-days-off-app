import { test, expect } from '@playwright/test';

test.describe('Athletes Page', () => {
  test.beforeEach(async ({ page }) => {
    await page.goto('/athletes');
  });

  test('should display athletes page', async ({ page }) => {
    await expect(page.locator('.athletes-page')).toBeVisible();
    await expect(page.locator('ndo-secondary-header')).toContainText('Athletes');
  });

  test('should display athlete cards', async ({ page }) => {
    await expect(page.locator('.athlete-card')).toHaveCount(2);
  });

  test('should open add athlete modal', async ({ page }) => {
    await page.click('.plus-button');
    await expect(page.locator('.modal-window__backdrop')).toBeVisible();
    await expect(page.locator('.modal-window__title')).toContainText('Add Athlete');
  });

  test('should close modal when clicking close button', async ({ page }) => {
    await page.click('.plus-button');
    await expect(page.locator('.modal-window__backdrop')).toBeVisible();

    await page.click('.modal-window__close');
    await expect(page.locator('.modal-window__backdrop')).not.toBeVisible();
  });

  test('should open edit modal when clicking edit on athlete card', async ({ page }) => {
    await page.locator('.athlete-card').first().locator('button[aria-label="Edit athlete"]').click();
    await expect(page.locator('.modal-window__backdrop')).toBeVisible();
    await expect(page.locator('.modal-window__title')).toContainText('Edit Athlete');
  });
});
