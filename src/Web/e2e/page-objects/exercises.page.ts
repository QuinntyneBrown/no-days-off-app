import { type Page, type Locator } from '@playwright/test';

export class ExercisesPage {
  readonly page: Page;
  readonly container: Locator;
  readonly addButton: Locator;
  readonly grid: Locator;
  readonly filters: Locator;

  constructor(page: Page) {
    this.page = page;
    this.container = page.getByTestId('exercises-page');
    this.addButton = page.getByTestId('add-exercise-btn');
    this.grid = page.getByTestId('exercise-grid');
    this.filters = page.getByTestId('exercise-filters');
  }

  async goto() {
    await this.page.goto('/exercises');
    await this.page.waitForLoadState('domcontentloaded');
    await this.container.waitFor({ state: 'visible', timeout: 15000 });
  }

  async clickAdd() {
    await this.addButton.click();
  }

  async fillDialog(name: string, sets?: number, reps?: number) {
    await this.page.getByTestId('exercise-name-input').fill(name);
    if (sets) await this.page.getByTestId('exercise-sets-input').fill(sets.toString());
    if (reps) await this.page.getByTestId('exercise-reps-input').fill(reps.toString());
  }

  async saveDialog() {
    await this.page.getByTestId('exercise-save-btn').click();
  }

  async getExerciseCards() {
    return this.grid.locator('mat-card').all();
  }

  async hasExerciseNamed(name: string): Promise<boolean> {
    const card = this.grid.getByText(name);
    return card.isVisible({ timeout: 5000 }).catch(() => false);
  }
}
