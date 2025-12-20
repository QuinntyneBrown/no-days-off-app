import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

export interface PagedList<T> {
  data: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface PagerEvent {
  pageNumber: number;
}

@Component({
  selector: 'ndo-pager',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatIconModule],
  templateUrl: './pager.html',
  styleUrl: './pager.scss'
})
export class Pager {
  pageNumber = input<number>(1);
  totalPages = input<number>(1);

  next = output<PagerEvent>();
  previous = output<PagerEvent>();

  emitNext(): void {
    const currentPage = this.pageNumber();
    const total = this.totalPages();
    const nextPage = currentPage < total ? currentPage + 1 : 1;
    this.next.emit({ pageNumber: nextPage });
  }

  emitPrevious(): void {
    const currentPage = this.pageNumber();
    const total = this.totalPages();
    const prevPage = currentPage > 1 ? currentPage - 1 : total;
    this.previous.emit({ pageNumber: prevPage });
  }
}
