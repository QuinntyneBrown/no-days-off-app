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

  test('should create a new body part', async ({ authedPage }) => {
    const bodyParts = new BodyPartsPage(authedPage);
    await bodyParts.goto();

    const name = `BodyPart ${Date.now()}`;
    await bodyParts.clickAdd();
    await bodyParts.fillDialog(name);
    await bodyParts.saveDialog();

    await authedPage.waitForTimeout(2000);
    await expect(await bodyParts.hasBodyPartNamed(name)).toBe(true);
  });
});
