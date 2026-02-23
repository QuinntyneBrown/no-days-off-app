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

  test('should create a new workout day', async ({ authedPage }) => {
    const workoutDays = new WorkoutDaysPage(authedPage);
    await workoutDays.goto();

    const name = `Day ${Date.now()}`;
    await workoutDays.clickAdd();
    await workoutDays.fillDialog(name);
    await workoutDays.saveDialog();

    await authedPage.waitForTimeout(2000);
    await expect(await workoutDays.hasDayNamed(name)).toBe(true);
  });
});
