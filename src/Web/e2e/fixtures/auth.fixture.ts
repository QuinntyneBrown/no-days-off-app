import { test as base, type Page } from '@playwright/test';

const API_URL = 'http://localhost:5000/api';
const TEST_PASSWORD = 'TestPassword123A';

function uniqueEmail(): string {
  return `e2e_${Date.now()}_${Math.random().toString(36).substring(2, 7)}@test.com`;
}

export interface TestUser {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  accessToken: string;
}

async function registerTestUser(): Promise<TestUser> {
  const email = uniqueEmail();
  const user = {
    email,
    password: TEST_PASSWORD,
    firstName: 'E2E',
    lastName: 'Tester',
  };

  const res = await fetch(`${API_URL}/auth/register`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(user),
  });

  if (!res.ok) {
    const body = await res.text();
    throw new Error(`Registration failed (${res.status}): ${body}`);
  }

  const data = await res.json();
  return { ...user, accessToken: data.accessToken };
}

async function loginViaUI(page: Page, email: string, password: string) {
  await page.goto('/login');
  await page.getByTestId('login-email').fill(email);
  await page.getByTestId('login-password').fill(password);
  await page.getByTestId('login-submit').click();
  await page.waitForURL('**/dashboard', { timeout: 15000 });
}

export const test = base.extend<{ testUser: TestUser; authedPage: Page }>({
  testUser: async ({}, use) => {
    const user = await registerTestUser();
    await use(user);
  },
  authedPage: async ({ page, testUser }, use) => {
    await loginViaUI(page, testUser.email, testUser.password);
    await use(page);
  },
});

export { expect } from '@playwright/test';
export { loginViaUI, registerTestUser, API_URL, TEST_PASSWORD };
