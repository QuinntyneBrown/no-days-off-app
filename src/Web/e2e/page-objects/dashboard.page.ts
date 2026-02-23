import { type Page, type Locator } from '@playwright/test';

export class DashboardPage {
  readonly page: Page;
  readonly container: Locator;
  readonly statsRow: Locator;

  constructor(page: Page) {
    this.page = page;
    this.container = page.getByTestId('dashboard-page');
    this.statsRow = page.getByTestId('stats-row');
  }

  async goto() {
    await this.page.goto('/dashboard');
  }

  async getStatCards() {
    return this.statsRow.locator('ndo-stat-card').all();
  }
}
