import { Component, input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'ndo-flip-card-front',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="flip-card-front"><ng-content></ng-content></div>`,
  styles: [`
    .flip-card-front {
      position: absolute;
      width: 100%;
      height: 100%;
      backface-visibility: hidden;
    }
  `]
})
export class FlipCardFront {}

@Component({
  selector: 'ndo-flip-card-back',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="flip-card-back"><ng-content></ng-content></div>`,
  styles: [`
    .flip-card-back {
      position: absolute;
      width: 100%;
      height: 100%;
      backface-visibility: hidden;
      transform: rotateY(180deg);
    }
  `]
})
export class FlipCardBack {}

@Component({
  selector: 'ndo-flip-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './flip-card.html',
  styleUrl: './flip-card.scss'
})
export class FlipCard {
  width = input<string>('300px');
  height = input<string>('200px');

  isFlipped = signal<boolean>(false);

  flip(): void {
    this.isFlipped.update(v => !v);
  }
}
