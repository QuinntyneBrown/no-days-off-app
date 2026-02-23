import { type Page, type Locator } from '@playwright/test';

export class WorkoutDaysPage {
  readonly page: Page;
  readonly container: Locator;
  readonly addButton: Locator;
  readonly list: Locator;

  constructor(page: Page) {
    this.page = page;
    this.container = page.getByTestId('workout-days-page');
    this.addButton = page.getByTestId('add-day-btn');
    this.list = page.getByTestId('days-list');
  }

  async goto() {
    await this.page.goto('/workout-days');
    await this.page.waitForLoadState('domcontentloaded');
    await this.container.waitFor({ state: 'visible', timeout: 15000 });
  }

  async clickAdd() {
    await this.addButton.click();
  }

  async fillDialog(name: string) {
    await this.page.getByTestId('day-name-input').fill(name);
  }

  async saveDialog() {
    await this.page.getByTestId('day-save-btn').click();
  }

  async hasDayNamed(name: string): Promise<boolean> {
    const card = this.list.getByText(name);
    return card.isVisible({ timeout: 5000 }).catch(() => false);
  }
}
