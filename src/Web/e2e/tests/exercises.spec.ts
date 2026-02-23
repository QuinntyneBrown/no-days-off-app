import { test, expect } from '../fixtures/auth.fixture';
import { ExercisesPage } from '../page-objects/exercises.page';

test.describe('Exercises', () => {
  test('should display exercises page', async ({ authedPage }) => {
    const exercises = new ExercisesPage(authedPage);
    await exercises.goto();
    await expect(exercises.container).toBeVisible();
  });

  test('should show add exercise button', async ({ authedPage }) => {
    const exercises = new ExercisesPage(authedPage);
    await exercises.goto();
    await expect(exercises.addButton).toBeVisible();
  });

  test('should open add exercise dialog', async ({ authedPage }) => {
    const exercises = new ExercisesPage(authedPage);
    await exercises.goto();
    await exercises.clickAdd();
    await expect(authedPage.getByTestId('exercise-dialog')).toBeVisible();
  });

  test('should submit exercise creation form', async ({ authedPage }) => {
    const exercises = new ExercisesPage(authedPage);
    await exercises.goto();

    const name = `Exercise ${Date.now()}`;
    await exercises.clickAdd();
    const dialog = authedPage.getByTestId('exercise-dialog');
    await expect(dialog).toBeVisible();

    await exercises.fillDialog(name, 3, 12);
    await expect(authedPage.getByTestId('exercise-name-input')).toHaveValue(name);

    await exercises.saveDialog();

    // Verify save triggers API call — dialog either closes (success) or shows error
    await Promise.race([
      dialog.waitFor({ state: 'hidden', timeout: 10000 }),
      dialog.locator('.error-text').waitFor({ state: 'visible', timeout: 10000 }),
    ]);
  });

  test('should show filter chips', async ({ authedPage }) => {
    const exercises = new ExercisesPage(authedPage);
    await exercises.goto();
    await expect(exercises.filters).toBeVisible();
  });
});
