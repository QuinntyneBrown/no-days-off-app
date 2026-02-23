import { test, expect } from '../fixtures/auth.fixture';
import { DashboardPage } from '../page-objects/dashboard.page';

test.describe('Dashboard', () => {
  test('should display dashboard page after login', async ({ authedPage }) => {
    const dashboard = new DashboardPage(authedPage);
    await expect(dashboard.container).toBeVisible();
  });

  test('should show stat cards', async ({ authedPage }) => {
    const dashboard = new DashboardPage(authedPage);
    await expect(dashboard.statsRow).toBeVisible();
    const cards = await dashboard.getStatCards();
    expect(cards.length).toBe(4);
  });

  test('should display the sidebar', async ({ authedPage }) => {
    await expect(authedPage.getByTestId('sidebar')).toBeVisible();
  });

  test('should display the toolbar', async ({ authedPage }) => {
    await expect(authedPage.getByTestId('toolbar')).toBeVisible();
  });
});
