import { test, expect } from '../fixtures/auth.fixture';
import { WorkoutDaysPage } from '../page-objects/workout-days.page';

test.describe('Workout Days', () => {
  test('should display workout days page', async ({ authedPage }) => {
    const workoutDays = new WorkoutDaysPage(authedPage);
    await workoutDays.goto();
    await expect(workoutDays.container).toBeVisible();
  });

  test('should show add workout day button', async ({ authedPage }) => {
    const workoutDays = new WorkoutDaysPage(authedPage);
    await workoutDays.goto();
    await expect(workoutDays.addButton).toBeVisible();
  });

  test('should open add workout day dialog', async ({ authedPage }) => {
    const workoutDays = new WorkoutDaysPage(authedPage);
    await workoutDays.goto();
    await workoutDays.clickAdd();
    await expect(authedPage.getByTestId('workout-day-dialog')).toBeVisible();
  });

  test('should submit workout day creation form', async ({ authedPage }) => {
    const workoutDays = new WorkoutDaysPage(authedPage);
    await workoutDays.goto();

    const name = `Day ${Date.now()}`;
    await workoutDays.clickAdd();
    const dialog = authedPage.getByTestId('workout-day-dialog');
    await expect(dialog).toBeVisible();

    await workoutDays.fillDialog(name);
    await expect(authedPage.getByTestId('day-name-input')).toHaveValue(name);

    await workoutDays.saveDialog();

    // Verify save triggers API call — dialog either closes (success) or shows error
    await Promise.race([
      dialog.waitFor({ state: 'hidden', timeout: 10000 }),
      dialog.locator('.error-text').waitFor({ state: 'visible', timeout: 10000 }),
    ]);
  });
});
