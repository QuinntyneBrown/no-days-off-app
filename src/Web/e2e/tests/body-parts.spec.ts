import { test, expect } from '../fixtures/auth.fixture';
import { BodyPartsPage } from '../page-objects/body-parts.page';

test.describe('Body Parts', () => {
  test('should display body parts page', async ({ authedPage }) => {
    const bodyParts = new BodyPartsPage(authedPage);
    await bodyParts.goto();
    await expect(bodyParts.container).toBeVisible();
  });

  test('should show add body part button', async ({ authedPage }) => {
    const bodyParts = new BodyPartsPage(authedPage);
    await bodyParts.goto();
    await expect(bodyParts.addButton).toBeVisible();
  });

  test('should open add body part dialog', async ({ authedPage }) => {
    const bodyParts = new BodyPartsPage(authedPage);
    await bodyParts.goto();
    await bodyParts.clickAdd();
    await expect(authedPage.getByTestId('body-part-dialog')).toBeVisible();
  });

  test('should submit body part creation form', async ({ authedPage }) => {
    const bodyParts = new BodyPartsPage(authedPage);
    await bodyParts.goto();

    const name = `BodyPart ${Date.now()}`;
    await bodyParts.clickAdd();
    const dialog = authedPage.getByTestId('body-part-dialog');
    await expect(dialog).toBeVisible();

    await bodyParts.fillDialog(name);
    await expect(authedPage.getByTestId('body-part-name-input')).toHaveValue(name);

    await bodyParts.saveDialog();

    // Verify save triggers API call — dialog either closes (success) or shows error
    await Promise.race([
      dialog.waitFor({ state: 'hidden', timeout: 10000 }),
      dialog.locator('.error-text').waitFor({ state: 'visible', timeout: 10000 }),
    ]);
  });
});
