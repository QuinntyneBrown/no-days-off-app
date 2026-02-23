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
    await this.page.goto(route);
    await this.page.waitForLoadState('domcontentloaded');
  }

  async clickSidebarLink(route: string) {
    const link = this.sidebar.locator(`a[href="${route}"]`);
    await link.click();
  }

  async logout() {
    await this.logoutButton.click();
  }
}
