import { Component, inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { NdoSearchBarComponent, NdoSnackbarService } from 'components';
import { BodyPartsService } from 'api';
import type { BodyPart } from 'api';
import { BodyPartDialog } from '../../dialogs/body-part-dialog';

const BODY_PART_COLORS = ['#00E5FF', '#FF6B35', '#4ADE80', '#60A5FA', '#FBBF24', '#F87171', '#A78BFA', '#F472B6', '#34D399'];

@Component({
  selector: 'app-body-parts',
  imports: [MatButtonModule, MatIconModule, MatCardModule, MatDialogModule, NdoSearchBarComponent],
  template: `
    <div class="page" data-testid="body-parts-page">
      <div class="page-header">
        <div>
          <h2 class="page-title">Body Parts</h2>
          <p class="page-sub">{{ bodyParts().length }} body parts</p>
        </div>
        <div class="header-actions">
          <ndo-search-bar placeholder="Search body parts..." (valueChange)="onSearch($event)" />
          <button mat-flat-button class="primary-btn" (click)="openAdd()" data-testid="add-body-part-btn">
            <mat-icon fontSet="material-symbols-rounded">add</mat-icon>
            Add Body Part
          </button>
        </div>
      </div>
      <div class="bp-grid" data-testid="body-parts-grid">
        @for (bp of filtered(); track bp.bodyPartId; let i = $index) {
          <mat-card class="bp-card" [attr.data-testid]="'body-part-' + bp.bodyPartId">
            <mat-card-content>
              <div class="bp-icon" [style.background]="getColor(i) + '18'" [style.color]="getColor(i)">
                <mat-icon fontSet="material-symbols-rounded">accessibility_new</mat-icon>
              </div>
              <h3 class="bp-name">{{ bp.name }}</h3>
            </mat-card-content>
          </mat-card>
        }
      </div>
    </div>
  `,
  styles: `
    .page { display: flex; flex-direction: column; gap: 20px; }
    .page-header { display: flex; justify-content: space-between; align-items: flex-start; flex-wrap: wrap; gap: 16px; }
    .page-title { font-family: 'Plus Jakarta Sans', sans-serif; font-size: 20px; font-weight: 700; margin: 0; }
    .page-sub { color: var(--ndo-text-secondary); font-size: 14px; margin: 4px 0 0; }
    .header-actions { display: flex; gap: 12px; align-items: center; }
    .primary-btn { --mdc-filled-button-container-color: var(--ndo-primary); --mdc-filled-button-label-text-color: var(--ndo-text-on-primary); }
    .bp-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(200px, 1fr)); gap: 16px; }
    .bp-card { --mdc-elevated-card-container-color: var(--ndo-bg-card); --mdc-elevated-card-container-elevation: none; border: 1px solid var(--ndo-border); text-align: center; cursor: pointer; }
    .bp-card:hover { border-color: var(--ndo-border-light); }
    .bp-icon { width: 56px; height: 56px; border-radius: 50%; display: inline-flex; align-items: center; justify-content: center; margin: 8px auto 12px; }
    .bp-name { font-size: 14px; font-weight: 600; margin: 0; }
  `,
})
export class BodyPartsPage implements OnInit {
  private readonly bodyPartsService = inject(BodyPartsService);
  private readonly dialog = inject(MatDialog);
  private readonly snackbar = inject(NdoSnackbarService);

  bodyParts = signal<BodyPart[]>([]);
  filtered = signal<BodyPart[]>([]);

  ngOnInit() { this.load(); }

  load() {
    this.bodyPartsService.getBodyParts().subscribe({
      next: (list) => { this.bodyParts.set(list); this.filtered.set(list); },
      error: () => {},
    });
  }

  onSearch(q: string) {
    const t = q.toLowerCase();
    this.filtered.set(this.bodyParts().filter(bp => bp.name.toLowerCase().includes(t)));
  }

  getColor(i: number) { return BODY_PART_COLORS[i % BODY_PART_COLORS.length]; }

  openAdd() {
    const ref = this.dialog.open(BodyPartDialog, { width: '400px' });
    ref.afterClosed().subscribe(r => { if (r) this.load(); });
  }
}
