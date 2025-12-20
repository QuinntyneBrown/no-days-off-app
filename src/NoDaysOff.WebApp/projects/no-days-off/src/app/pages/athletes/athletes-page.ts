import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AthleteList } from './components/athlete-list/athlete-list';
import { AthleteEdit } from './components/athlete-edit/athlete-edit';
import { PlusButton } from '../../components/plus-button';
import { ModalWindow } from '../../components/modal-window';
import { SecondaryHeader } from '../../components/secondary-header';
import { Pager, PagedList } from '../../components/pager';
import { Athlete } from '../../models/athlete';

@Component({
  selector: 'ndo-athletes-page',
  standalone: true,
  imports: [
    CommonModule,
    AthleteList,
    AthleteEdit,
    PlusButton,
    ModalWindow,
    SecondaryHeader,
    Pager
  ],
  templateUrl: './athletes-page.html',
  styleUrl: './athletes-page.scss'
})
export class AthletesPage {
  athletes = signal<Athlete[]>([]);
  pagedAthletes = signal<PagedList<Athlete> | null>(null);
  currentPage = signal(1);
  pageSize = 10;

  isEditModalOpen = signal(false);
  selectedAthlete = signal<Athlete | null>(null);

  constructor(private router: Router) {
    this.loadAthletes();
  }

  private loadAthletes(): void {
    // TODO: Replace with actual service call
    const mockAthletes: Athlete[] = [
      { athleteId: 1, name: 'John Doe', email: 'john@example.com', imageUrl: '', createdAt: new Date() },
      { athleteId: 2, name: 'Jane Smith', email: 'jane@example.com', imageUrl: '', createdAt: new Date() }
    ];
    this.athletes.set(mockAthletes);
    this.updatePagedAthletes();
  }

  private updatePagedAthletes(): void {
    const athletes = this.athletes();
    const start = (this.currentPage() - 1) * this.pageSize;
    const end = start + this.pageSize;
    const pagedData = athletes.slice(start, end);

    this.pagedAthletes.set({
      data: pagedData,
      page: this.currentPage(),
      pageSize: this.pageSize,
      totalCount: athletes.length,
      totalPages: Math.ceil(athletes.length / this.pageSize) || 1
    });
  }

  onAddAthlete(): void {
    this.selectedAthlete.set(null);
    this.isEditModalOpen.set(true);
  }

  onEditAthlete(athlete: Athlete): void {
    this.selectedAthlete.set(athlete);
    this.isEditModalOpen.set(true);
  }

  onDeleteAthlete(athlete: Athlete): void {
    this.athletes.update(a => a.filter(ath => ath.athleteId !== athlete.athleteId));
    this.updatePagedAthletes();
  }

  onSaveAthlete(athlete: Athlete): void {
    if (athlete.athleteId === 0) {
      const newAthlete = { ...athlete, athleteId: Date.now() };
      this.athletes.update(a => [...a, newAthlete]);
    } else {
      this.athletes.update(a =>
        a.map(ath => ath.athleteId === athlete.athleteId ? athlete : ath)
      );
    }
    this.updatePagedAthletes();
    this.isEditModalOpen.set(false);
  }

  onCloseModal(): void {
    this.isEditModalOpen.set(false);
    this.selectedAthlete.set(null);
  }

  onPageChange(page: number): void {
    this.currentPage.set(page);
    this.updatePagedAthletes();
  }
}
