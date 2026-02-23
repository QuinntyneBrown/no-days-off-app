import { test, expect, registerTestUser, TEST_PASSWORD } from '../fixtures/auth.fixture';
import { LoginPage } from '../page-objects/login.page';
import { RegisterPage } from '../page-objects/register.page';

test.describe('Authentication', () => {
  test('should show login page by default', async ({ page }) => {
    await page.goto('/');
    await expect(page).toHaveURL(/login/);
  });

  test('should show validation error for empty fields', async ({ page }) => {
    const loginPage = new LoginPage(page);
    await loginPage.goto();
    await loginPage.submitButton.click();
    await expect(loginPage.errorText).toBeVisible();
  });

  test('should show error for invalid credentials', async ({ page }) => {
    const loginPage = new LoginPage(page);
    await loginPage.goto();
    await loginPage.login('wrong@email.com', 'wrongpass');
    await expect(loginPage.errorText).toBeVisible({ timeout: 10000 });
  });

  test('should login successfully and redirect to dashboard', async ({ page }) => {
    const user = await registerTestUser();
    const loginPage = new LoginPage(page);
    await loginPage.goto();
    await loginPage.login(user.email, user.password);
    await expect(page).toHaveURL(/dashboard/, { timeout: 15000 });
  });

  test('should register a new user and redirect to dashboard', async ({ page }) => {
    const registerPage = new RegisterPage(page);
    await registerPage.goto();

    const email = `reg_${Date.now()}@test.com`;
    await registerPage.register('New', 'User', email, TEST_PASSWORD);
    await expect(page).toHaveURL(/dashboard/, { timeout: 15000 });
  });

  test('should navigate from login to register page', async ({ page }) => {
    const loginPage = new LoginPage(page);
    await loginPage.goto();
    await loginPage.registerLink.click();
    await expect(page).toHaveURL(/register/);
  });

  test('should navigate from register to login page', async ({ page }) => {
    const registerPage = new RegisterPage(page);
    await registerPage.goto();
    await registerPage.loginLink.click();
    await expect(page).toHaveURL(/login/);
  });

  test('should navigate to forgot password page', async ({ page }) => {
    const loginPage = new LoginPage(page);
    await loginPage.goto();
    await loginPage.forgotPasswordLink.click();
    await expect(page).toHaveURL(/forgot-password/);
  });

  test('should show registration validation errors', async ({ page }) => {
    const registerPage = new RegisterPage(page);
    await registerPage.goto();
    await registerPage.submitButton.click();
    await expect(registerPage.errorText).toBeVisible();
  });

  test('should logout and redirect to login', async ({ authedPage }) => {
    await authedPage.getByTestId('logout-btn').click();
    await expect(authedPage).toHaveURL(/login/, { timeout: 10000 });
  });
});
