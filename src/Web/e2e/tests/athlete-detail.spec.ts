import { test, expect, API_URL } from '../fixtures/auth.fixture';
import { AthleteDetailPage } from '../page-objects/athlete-detail.page';

test.describe('Athlete Detail', () => {
  let athleteId: number;
  let athleteName: string;

  test.beforeEach(async ({ testUser }) => {
    athleteName = `Detail Athlete ${Date.now()}`;
    const res = await fetch(`${API_URL}/athletes`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${testUser.accessToken}`,
      },
      body: JSON.stringify({
        name: athleteName,
        username: `detail_${Date.now()}`,
      }),
    });
    const data = await res.json();
    athleteId = data.athleteId;
  });

  test('should display athlete detail page', async ({ authedPage }) => {
    const detail = new AthleteDetailPage(authedPage);
    await detail.goto(athleteId);
    await expect(detail.container).toBeVisible();
    await expect(authedPage.getByTestId('athlete-name')).toHaveText(athleteName);
  });

  test('should show log weight button', async ({ authedPage }) => {
    const detail = new AthleteDetailPage(authedPage);
    await detail.goto(athleteId);
    await expect(detail.logWeightBtn).toBeVisible();
  });

  test('should open log weight dialog', async ({ authedPage }) => {
    const detail = new AthleteDetailPage(authedPage);
    await detail.goto(athleteId);
    await detail.logWeightBtn.click();

    const dialog = authedPage.getByTestId('log-weight-dialog');
    await expect(
      await Promise.race([
        dialog.waitFor({ state: 'visible', timeout: 5000 }).then(() => true),
        new Promise(resolve => setTimeout(() => resolve(false), 5000)),
      ])
    ).toBe(true);
    await expect(authedPage.getByTestId('log-weight-athlete-name')).toHaveText(athleteName);
  });

  test('should log weight and verify update', async ({ authedPage }) => {
    const detail = new AthleteDetailPage(authedPage);
    await detail.goto(athleteId);
    await detail.logWeightBtn.click();

    const dialog = authedPage.getByTestId('log-weight-dialog');
    await dialog.waitFor({ state: 'visible', timeout: 5000 });

    await detail.fillLogWeightDialog(75);
    await detail.saveLogWeight();

    await authedPage.waitForTimeout(2000);
    await expect(authedPage.getByTestId('weight-tracking')).toContainText('75 kg');
  });

  test('should display weight history after logging', async ({ authedPage, testUser }) => {
    // Log a weight via API first
    await fetch(`${API_URL}/athletes/${athleteId}/weights`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${testUser.accessToken}`,
      },
      body: JSON.stringify({
        weightInKgs: 80,
        weighedOn: new Date().toISOString(),
      }),
    });

    const detail = new AthleteDetailPage(authedPage);
    await detail.goto(athleteId);

    await expect(detail.weightHistory).toBeVisible({ timeout: 5000 });
    const rows = await detail.getWeightHistoryRows();
    expect(rows.length).toBeGreaterThanOrEqual(1);
  });

  test('should navigate back to athletes list', async ({ authedPage }) => {
    const detail = new AthleteDetailPage(authedPage);
    await detail.goto(athleteId);
    await detail.backBtn.click();
    await authedPage.waitForURL('**/athletes', { timeout: 5000 });
  });
});
