import { Page, expect } from '@playwright/test';
import { createApiMocks } from './api-mocks';

/**
 * Common test actions for e2e tests
 */
export class TestActions {
  private mocks;

  constructor(private page: Page) {
    this.mocks = createApiMocks(page);
  }

  /**
   * Setup authenticated state and navigate to page
   */
  async navigateAsAuthenticated(path: string): Promise<void> {
    await this.mocks.setAuthenticatedState();
    await this.mocks.setupDefaultMocks();
    await this.page.goto(path);
  }

  /**
   * Login via the login page
   */
  async login(email = 'test@example.com', password = 'Test12345'): Promise<void> {
    await this.mocks.mockAuthLogin(true);
    await this.mocks.mockDashboard();

    await this.page.goto('/login');

    await this.page.fill('input[formcontrolname="email"]', email);
    await this.page.fill('input[formcontrolname="password"]', password);
    await this.page.click('button[type="submit"]');

    await expect(this.page).toHaveURL('/dashboard');
  }

  /**
   * Open navigation menu
   */
  async openMenu(): Promise<void> {
    await this.page.click('.hamburger-button');
    await expect(this.page.locator('.left-nav--open')).toBeVisible();
  }

  /**
   * Close navigation menu
   */
  async closeMenu(): Promise<void> {
    await this.page.click('.hamburger-button');
    await expect(this.page.locator('.left-nav--open')).not.toBeVisible();
  }

  /**
   * Navigate to page via menu
   */
  async navigateViaMenu(linkText: string): Promise<void> {
    await this.openMenu();
    await this.page.click(`text=${linkText}`);
  }

  /**
   * Open modal by clicking plus button
   */
  async openAddModal(): Promise<void> {
    await this.page.click('.plus-button');
    await expect(this.page.locator('.modal-window__backdrop')).toBeVisible();
  }

  /**
   * Close modal
   */
  async closeModal(): Promise<void> {
    await this.page.click('.modal-window__close');
    await expect(this.page.locator('.modal-window__backdrop')).not.toBeVisible();
  }

  /**
   * Fill athlete form
   */
  async fillAthleteForm(name: string, username: string, imageUrl = ''): Promise<void> {
    await this.page.fill('input[formcontrolname="name"]', name);
    await this.page.fill('input[formcontrolname="username"]', username);
    if (imageUrl) {
      await this.page.fill('input[formcontrolname="imageUrl"]', imageUrl);
    }
  }

  /**
   * Submit form by clicking save button
   */
  async submitForm(): Promise<void> {
    await this.page.click('button:has-text("Save")');
  }

  /**
   * Cancel form by clicking cancel button
   */
  async cancelForm(): Promise<void> {
    await this.page.click('button:has-text("Cancel")');
  }

  /**
   * Wait for loading to complete
   */
  async waitForLoading(): Promise<void> {
    // Wait for loading indicators to disappear
    const loadingIndicator = this.page.locator('.loading, .spinner, [aria-busy="true"]');
    if (await loadingIndicator.isVisible()) {
      await expect(loadingIndicator).not.toBeVisible({ timeout: 10000 });
    }
  }

  /**
   * Assert snackbar message
   */
  async expectSnackbarMessage(message: string): Promise<void> {
    const snackbar = this.page.locator('.mat-mdc-snack-bar-container, .mat-snack-bar-container');
    await expect(snackbar).toContainText(message);
  }

  /**
   * Get athlete card by name
   */
  getAthleteCard(name: string) {
    return this.page.locator('.athlete-card', { hasText: name });
  }

  /**
   * Edit athlete by name
   */
  async editAthlete(name: string): Promise<void> {
    const card = this.getAthleteCard(name);
    await card.locator('button[aria-label="Edit athlete"]').click();
    await expect(this.page.locator('.modal-window__backdrop')).toBeVisible();
  }

  /**
   * Delete athlete by name
   */
  async deleteAthlete(name: string): Promise<void> {
    const card = this.getAthleteCard(name);
    await card.locator('button[aria-label="Delete athlete"]').click();
  }

  /**
   * Assert page title
   */
  async expectPageTitle(title: string): Promise<void> {
    await expect(this.page.locator('h1, .page-title, .secondary-header__title')).toContainText(title);
  }

  /**
   * Clear local storage
   */
  async clearStorage(): Promise<void> {
    await this.page.evaluate(() => {
      localStorage.clear();
      sessionStorage.clear();
    });
  }

  /**
   * Get local storage value
   */
  async getLocalStorageValue(key: string): Promise<string | null> {
    return await this.page.evaluate((k) => localStorage.getItem(k), key);
  }

  /**
   * Set local storage value
   */
  async setLocalStorageValue(key: string, value: string): Promise<void> {
    await this.page.evaluate(({ k, v }) => localStorage.setItem(k, v), { k: key, v: value });
  }

  /**
   * Take screenshot with descriptive name
   */
  async screenshot(name: string): Promise<void> {
    await this.page.screenshot({ path: `./screenshots/${name}.png` });
  }

  /**
   * Wait for API call to complete
   */
  async waitForApiCall(endpoint: string): Promise<void> {
    await this.page.waitForResponse((response) =>
      response.url().includes(endpoint) && response.status() === 200
    );
  }

  /**
   * Verify current URL
   */
  async expectUrl(path: string): Promise<void> {
    await expect(this.page).toHaveURL(new RegExp(path));
  }
}

/**
 * Create TestActions instance for a page
 */
export function createTestActions(page: Page): TestActions {
  return new TestActions(page);
}
