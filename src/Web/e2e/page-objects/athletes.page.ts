import { type Page, type Locator } from '@playwright/test';

export class AthletesPage {
  readonly page: Page;
  readonly container: Locator;
  readonly addButton: Locator;
  readonly table: Locator;

  constructor(page: Page) {
    this.page = page;
    this.container = page.getByTestId('athletes-page');
    this.addButton = page.getByTestId('add-athlete-btn');
    this.table = page.getByTestId('athletes-table');
  }

  async goto() {
    await this.page.goto('/athletes');
    await this.page.waitForLoadState('domcontentloaded');
    await this.container.waitFor({ state: 'visible', timeout: 15000 });
  }

  async clickAdd() {
    await this.addButton.click();
  }

  async fillDialog(name: string, username: string) {
    await this.page.getByTestId('athlete-name-input').fill(name);
    await this.page.getByTestId('athlete-username-input').fill(username);
  }

  async saveDialog() {
    await this.page.getByTestId('athlete-save-btn').click();
  }

  async getTableRows() {
    return this.table.locator('tr[mat-row]').all();
  }

  async hasAthleteNamed(name: string): Promise<boolean> {
    const cell = this.table.getByText(name);
    return cell.isVisible({ timeout: 5000 }).catch(() => false);
  }

  async deleteAthlete(athleteId: number) {
    await this.page.getByTestId(`delete-athlete-${athleteId}`).click();
    await this.page.getByTestId('confirm-ok').click();
  }
}
