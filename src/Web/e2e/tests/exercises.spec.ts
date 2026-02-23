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

  test('should create a new exercise', async ({ authedPage }) => {
    const exercises = new ExercisesPage(authedPage);
    await exercises.goto();

    const name = `Exercise ${Date.now()}`;
    await exercises.clickAdd();
    await exercises.fillDialog(name, 3, 12);
    await exercises.saveDialog();

    await authedPage.waitForTimeout(2000);
    await expect(await exercises.hasExerciseNamed(name)).toBe(true);
  });

  test('should show filter chips', async ({ authedPage }) => {
    const exercises = new ExercisesPage(authedPage);
    await exercises.goto();
    await expect(exercises.filters).toBeVisible();
  });
});
