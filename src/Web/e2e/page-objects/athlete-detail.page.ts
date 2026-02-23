import { type Page, type Locator } from '@playwright/test';

export class AthleteDetailPage {
  readonly page: Page;
  readonly container: Locator;
  readonly logWeightBtn: Locator;
  readonly weightHistory: Locator;
  readonly backBtn: Locator;

  constructor(page: Page) {
    this.page = page;
    this.container = page.getByTestId('athlete-detail-page');
    this.logWeightBtn = page.getByTestId('log-weight-btn');
    this.weightHistory = page.getByTestId('weight-history');
    this.backBtn = page.getByTestId('back-btn');
  }

  async goto(athleteId: number) {
    await this.page.goto(`/athletes/${athleteId}`);
    await this.page.waitForLoadState('domcontentloaded');
    await this.container.waitFor({ state: 'visible', timeout: 15000 });
  }

  async fillLogWeightDialog(weight: number, date?: string) {
    await this.page.getByTestId('log-weight-input').fill(String(weight));
    if (date) {
      await this.page.getByTestId('log-weight-date').fill(date);
    }
  }

  async saveLogWeight() {
    await this.page.getByTestId('log-weight-save-btn').click();
  }

  async getWeightHistoryRows() {
    return this.page.getByTestId('weight-history-row').all();
  }
}
