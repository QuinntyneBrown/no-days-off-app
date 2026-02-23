import { type Page, type Locator } from '@playwright/test';

export class NavigationPage {
  readonly page: Page;
  readonly sidebar: Locator;
  readonly toolbar: Locator;
  readonly logoutButton: Locator;

  constructor(page: Page) {
    this.page = page;
    this.sidebar = page.getByTestId('sidebar');
    this.toolbar = page.getByTestId('toolbar');
    this.logoutButton = page.getByTestId('logout-btn');
  }

  async navigateTo(route: string) {
    await this.page.getByTestId(`nav-${route}`).click();
  }

  async logout() {
    await this.logoutButton.click();
  }
}
