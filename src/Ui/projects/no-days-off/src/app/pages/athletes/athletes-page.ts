import { Component, signal, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AthleteList } from './components/athlete-list/athlete-list';
import { AthleteEdit } from './components/athlete-edit/athlete-edit';
import { PlusButton } from '../../components/plus-button';
import { ModalWindow } from '../../components/modal-window';
import { SecondaryHeader } from '../../components/secondary-header';
import { Pager, PagedList } from '../../components/pager';
import { Athlete } from '../../models/athlete';
import { AthletesService, Athlete as AthleteDto, CreateAthleteRequest, UpdateAthleteRequest } from '../../../../../../src/app/core/services/athletes.service';

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
export class AthletesPage implements OnInit {
  private athletesService = inject(AthletesService);
  private router = inject(Router);

  athletes = signal<Athlete[]>([]);
  pagedAthletes = signal<PagedList<Athlete> | null>(null);
  currentPage = signal(1);
  pageSize = 10;
  isLoading = signal(false);
  error = signal<string | null>(null);

  isEditModalOpen = signal(false);
  selectedAthlete = signal<Athlete | null>(null);

  ngOnInit(): void {
    this.loadAthletes();
  }

  private loadAthletes(): void {
    this.isLoading.set(true);
    this.error.set(null);

    this.athletesService.getAll().subscribe({
      next: (athletes) => {
        const mappedAthletes: Athlete[] = athletes.map(a => ({
          athleteId: a.athleteId,
          name: a.name,
          username: a.username,
          imageUrl: a.imageUrl,
          currentWeight: a.currentWeight,
          lastWeighedOn: a.lastWeighedOn ? new Date(a.lastWeighedOn) : undefined,
          tenantId: a.tenantId,
          createdOn: new Date(a.createdOn),
          createdBy: a.createdBy
        }));
        this.athletes.set(mappedAthletes);
        this.updatePagedAthletes();
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to load athletes:', err);
        this.error.set('Failed to load athletes. Please try again.');
        this.isLoading.set(false);
      }
    });
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
    this.athletesService.delete(athlete.athleteId).subscribe({
      next: () => {
        this.athletes.update(a => a.filter(ath => ath.athleteId !== athlete.athleteId));
        this.updatePagedAthletes();
      },
      error: (err) => {
        console.error('Failed to delete athlete:', err);
        this.error.set('Failed to delete athlete. Please try again.');
      }
    });
  }

  onSaveAthlete(athlete: Athlete): void {
    if (athlete.athleteId === 0) {
      // Create new athlete
      const request: CreateAthleteRequest = {
        name: athlete.name,
        username: athlete.username
      };
      this.athletesService.create(request).subscribe({
        next: (created) => {
          const newAthlete: Athlete = {
            athleteId: created.athleteId,
            name: created.name,
            username: created.username,
            imageUrl: created.imageUrl,
            createdOn: new Date(created.createdOn),
            createdBy: created.createdBy
          };
          this.athletes.update(a => [...a, newAthlete]);
          this.updatePagedAthletes();
          this.isEditModalOpen.set(false);
        },
        error: (err) => {
          console.error('Failed to create athlete:', err);
          this.error.set('Failed to create athlete. Please try again.');
        }
      });
    } else {
      // Update existing athlete
      const request: UpdateAthleteRequest = {
        athleteId: athlete.athleteId,
        name: athlete.name,
        username: athlete.username
      };
      this.athletesService.update(athlete.athleteId, request).subscribe({
        next: (updated) => {
          this.athletes.update(a =>
            a.map(ath => ath.athleteId === athlete.athleteId ? {
              ...ath,
              name: updated.name,
              username: updated.username
            } : ath)
          );
          this.updatePagedAthletes();
          this.isEditModalOpen.set(false);
        },
        error: (err) => {
          console.error('Failed to update athlete:', err);
          this.error.set('Failed to update athlete. Please try again.');
        }
      });
    }
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
