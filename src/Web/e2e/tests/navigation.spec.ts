import { test, expect } from '../fixtures/auth.fixture';
import { NavigationPage } from '../page-objects/navigation.page';

test.describe('Navigation', () => {
  test('should navigate to athletes page', async ({ authedPage }) => {
    await authedPage.goto('/athletes');
    await expect(authedPage).toHaveURL(/athletes/);
    await expect(authedPage.getByTestId('athletes-page')).toBeVisible();
  });

  test('should navigate to exercises page', async ({ authedPage }) => {
    await authedPage.goto('/exercises');
    await expect(authedPage).toHaveURL(/exercises/);
    await expect(authedPage.getByTestId('exercises-page')).toBeVisible();
  });

  test('should navigate to weekly planner page', async ({ authedPage }) => {
    await authedPage.goto('/weekly-planner');
    await expect(authedPage).toHaveURL(/weekly-planner/);
    await expect(authedPage.getByTestId('weekly-planner-page')).toBeVisible();
  });

  test('should navigate to workout days page', async ({ authedPage }) => {
    await authedPage.goto('/workout-days');
    await expect(authedPage).toHaveURL(/workout-days/);
    await expect(authedPage.getByTestId('workout-days-page')).toBeVisible();
  });

  test('should navigate to videos page', async ({ authedPage }) => {
    await authedPage.goto('/videos');
    await expect(authedPage).toHaveURL(/videos/);
    await expect(authedPage.getByTestId('videos-page')).toBeVisible();
  });

  test('should navigate to messages page', async ({ authedPage }) => {
    await authedPage.goto('/messages');
    await expect(authedPage).toHaveURL(/messages/);
    await expect(authedPage.getByTestId('messages-page')).toBeVisible();
  });

  test('should navigate to notifications page', async ({ authedPage }) => {
    await authedPage.goto('/notifications');
    await expect(authedPage).toHaveURL(/notifications/);
    await expect(authedPage.getByTestId('notifications-page')).toBeVisible();
  });

  test('should navigate to body parts page', async ({ authedPage }) => {
    await authedPage.goto('/body-parts');
    await expect(authedPage).toHaveURL(/body-parts/);
    await expect(authedPage.getByTestId('body-parts-page')).toBeVisible();
  });

  test('should navigate to settings page', async ({ authedPage }) => {
    await authedPage.goto('/settings');
    await expect(authedPage).toHaveURL(/settings/);
    await expect(authedPage.getByTestId('settings-page')).toBeVisible();
  });

  test('should show sidebar with nav items', async ({ authedPage }) => {
    const nav = new NavigationPage(authedPage);
    await expect(nav.sidebar).toBeVisible();
    await expect(nav.toolbar).toBeVisible();
  });

  test('should redirect unauthenticated users to login', async ({ page }) => {
    await page.goto('/dashboard');
    await expect(page).toHaveURL(/login/);
  });
});
