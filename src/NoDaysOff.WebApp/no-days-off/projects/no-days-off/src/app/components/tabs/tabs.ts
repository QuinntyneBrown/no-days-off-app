import { Component, input, output, signal, contentChildren } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTabsModule } from '@angular/material/tabs';

@Component({
  selector: 'ndo-tab',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="tab" [class.tab--active]="isActive()">
      <ng-content></ng-content>
    </div>
  `,
  styles: [`
    .tab {
      display: none;
      &--active { display: block; }
    }
  `]
})
export class Tab {
  label = input.required<string>();
  isActive = signal<boolean>(false);
}

@Component({
  selector: 'ndo-tabs',
  standalone: true,
  imports: [CommonModule, MatTabsModule],
  templateUrl: './tabs.html',
  styleUrl: './tabs.scss'
})
export class Tabs {
  tabs = contentChildren(Tab);
  selectedIndex = signal<number>(0);

  tabChanged = output<number>();

  selectTab(index: number): void {
    this.selectedIndex.set(index);
    this.tabs().forEach((tab, i) => tab.isActive.set(i === index));
    this.tabChanged.emit(index);
  }

  ngAfterContentInit(): void {
    if (this.tabs().length > 0) {
      this.selectTab(0);
    }
  }
}
