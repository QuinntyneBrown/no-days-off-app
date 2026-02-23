import { Component, inject, OnInit, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { NdoSearchBarComponent } from 'components';
import { VideosService } from 'api';
import type { Video } from 'api';

@Component({
  selector: 'app-videos',
  imports: [MatButtonModule, MatIconModule, MatCardModule, MatChipsModule, NdoSearchBarComponent],
  template: `
    <div class="page" data-testid="videos-page">
      <div class="page-header">
        <div>
          <h2 class="page-title">Videos</h2>
          <p class="page-sub">{{ videos().length }} videos</p>
        </div>
        <div class="header-actions">
          <ndo-search-bar placeholder="Search videos..." (valueChange)="onSearch($event)" />
        </div>
      </div>

      <mat-chip-listbox (change)="onFilter($event.value)" data-testid="video-filters">
        <mat-chip-option value="all" selected>All</mat-chip-option>
        <mat-chip-option value="youtube">YouTube</mat-chip-option>
      </mat-chip-listbox>

      <div class="video-grid" data-testid="video-grid">
        @for (v of filtered(); track v.videoId) {
          <mat-card class="video-card" [attr.data-testid]="'video-' + v.videoId">
            <div class="thumb">
              <mat-icon fontSet="material-symbols-rounded" class="play-icon">play_circle</mat-icon>
              @if (v.durationInSeconds) {
                <span class="duration">{{ formatDuration(v.durationInSeconds) }}</span>
              }
            </div>
            <mat-card-content>
              <h3 class="video-title">{{ v.title }}</h3>
              @if (v.category) { <span class="video-cat">{{ v.category }}</span> }
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
    .video-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(280px, 1fr)); gap: 16px; }
    .video-card { --mdc-elevated-card-container-color: var(--ndo-bg-card); --mdc-elevated-card-container-elevation: none; border: 1px solid var(--ndo-border); cursor: pointer; }
    .thumb { height: 160px; background: var(--ndo-bg-elevated); display: flex; align-items: center; justify-content: center; position: relative; }
    .play-icon { font-size: 48px; width: 48px; height: 48px; color: var(--ndo-text-tertiary); }
    .duration { position: absolute; bottom: 8px; right: 8px; background: #000000cc; color: #fff; padding: 2px 6px; font-size: 12px; }
    .video-title { font-size: 14px; font-weight: 600; margin: 12px 0 4px; }
    .video-cat { font-size: 12px; color: var(--ndo-text-secondary); }
  `,
})
export class VideosPage implements OnInit {
  private readonly videosService = inject(VideosService);
  videos = signal<Video[]>([]);
  filtered = signal<Video[]>([]);
  private searchTerm = '';
  private filterType: string | null = null;

  ngOnInit() { this.load(); }

  load() {
    this.videosService.getVideos().subscribe({
      next: (list) => { this.videos.set(list); this.applyFilter(); },
      error: () => {},
    });
  }

  onSearch(q: string) { this.searchTerm = q; this.applyFilter(); }
  onFilter(val: string) { this.filterType = val === 'all' ? null : val; this.applyFilter(); }

  private applyFilter() {
    let list = this.videos();
    if (this.filterType === 'youtube') list = list.filter(v => v.youTubeVideoId);
    if (this.searchTerm) {
      const t = this.searchTerm.toLowerCase();
      list = list.filter(v => v.title.toLowerCase().includes(t));
    }
    this.filtered.set(list);
  }

  formatDuration(s: number): string {
    const m = Math.floor(s / 60);
    const sec = s % 60;
    return `${m}:${sec.toString().padStart(2, '0')}`;
  }
}
