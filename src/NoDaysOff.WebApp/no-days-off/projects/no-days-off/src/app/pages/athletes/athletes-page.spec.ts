import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { AthletesPage } from './athletes-page';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('AthletesPage', () => {
  let component: AthletesPage;
  let fixture: ComponentFixture<AthletesPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AthletesPage],
      providers: [provideRouter([]), provideAnimations()]
    }).compileComponents();

    fixture = TestBed.createComponent(AthletesPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load athletes on init', () => {
    expect(component.athletes().length).toBeGreaterThan(0);
  });

  it('should open add modal when adding athlete', () => {
    expect(component.isEditModalOpen()).toBe(false);
    component.onAddAthlete();
    expect(component.isEditModalOpen()).toBe(true);
    expect(component.selectedAthlete()).toBeNull();
  });

  it('should open edit modal with selected athlete', () => {
    const athlete = component.athletes()[0];
    component.onEditAthlete(athlete);
    expect(component.isEditModalOpen()).toBe(true);
    expect(component.selectedAthlete()).toEqual(athlete);
  });

  it('should close modal and reset selected athlete', () => {
    component.onAddAthlete();
    component.onCloseModal();
    expect(component.isEditModalOpen()).toBe(false);
    expect(component.selectedAthlete()).toBeNull();
  });

  it('should delete athlete from list', () => {
    const initialCount = component.athletes().length;
    const athleteToDelete = component.athletes()[0];
    component.onDeleteAthlete(athleteToDelete);
    expect(component.athletes().length).toBe(initialCount - 1);
  });
});
