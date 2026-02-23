import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'ndo-avatar',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="ndo-avatar" [style.width.px]="size" [style.height.px]="size" [style.border-radius.px]="size / 2" [style.font-size.px]="size * 0.35">
      @if (src) {
        <img [src]="src" [alt]="initials" class="ndo-avatar-img">
      } @else {
        {{ initials }}
      }
    </div>
  `,
  styles: `
    :host { display: inline-block; }

    .ndo-avatar {
      display: flex;
      align-items: center;
      justify-content: center;
      background-color: var(--ndo-primary-dim, #00E5FF18);
      color: var(--ndo-primary, #00E5FF);
      font-family: 'DM Sans', sans-serif;
      font-weight: 600;
      overflow: hidden;
    }

    .ndo-avatar-img {
      width: 100%;
      height: 100%;
      object-fit: cover;
    }
  `
})
export class NdoAvatarComponent {
  @Input() initials = 'JD';
  @Input() src = '';
  @Input() size = 40;
}
