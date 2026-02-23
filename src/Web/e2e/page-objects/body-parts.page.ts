import { type Page, type Locator } from '@playwright/test';

export class BodyPartsPage {
  readonly page: Page;
  readonly container: Locator;
  readonly addButton: Locator;
  readonly grid: Locator;

  constructor(page: Page) {
    this.page = page;
    this.container = page.getByTestId('body-parts-page');
    this.addButton = page.getByTestId('add-body-part-btn');
    this.grid = page.getByTestId('body-parts-grid');
  }

  async goto() {
    await this.page.goto('/body-parts');
    await this.page.waitForLoadState('domcontentloaded');
    await this.container.waitFor({ state: 'visible', timeout: 15000 });
  }

  async clickAdd() {
    await this.addButton.click();
  }

  async fillDialog(name: string) {
    await this.page.getByTestId('body-part-name-input').fill(name);
  }

  async saveDialog() {
    await this.page.getByTestId('body-part-save-btn').click();
  }

  async hasBodyPartNamed(name: string): Promise<boolean> {
    const card = this.grid.getByText(name);
    return card.isVisible({ timeout: 5000 }).catch(() => false);
  }
}
