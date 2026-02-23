import { test, expect } from '../fixtures/auth.fixture';
import { AthletesPage } from '../page-objects/athletes.page';

test.describe('Athletes', () => {
  test('should display athletes page', async ({ authedPage }) => {
    const athletes = new AthletesPage(authedPage);
    await athletes.goto();
    await expect(athletes.container).toBeVisible();
  });

  test('should show add athlete button', async ({ authedPage }) => {
    const athletes = new AthletesPage(authedPage);
    await athletes.goto();
    await expect(athletes.addButton).toBeVisible();
  });

  test('should open add athlete dialog', async ({ authedPage }) => {
    const athletes = new AthletesPage(authedPage);
    await athletes.goto();
    await athletes.clickAdd();
    await expect(authedPage.getByTestId('athlete-dialog')).toBeVisible();
  });

  test('should create a new athlete', async ({ authedPage }) => {
    const athletes = new AthletesPage(authedPage);
    await athletes.goto();

    const name = `Athlete ${Date.now()}`;
    await athletes.clickAdd();
    await athletes.fillDialog(name, 'testuser' + Date.now());
    await athletes.saveDialog();

    await authedPage.waitForTimeout(2000);
    await expect(await athletes.hasAthleteNamed(name)).toBe(true);
  });

  test('should show athletes table', async ({ authedPage }) => {
    const athletes = new AthletesPage(authedPage);
    await athletes.goto();
    await expect(athletes.table).toBeVisible();
  });
});
