import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HamburgerButton } from './hamburger-button';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('HamburgerButton', () => {
  let component: HamburgerButton;
  let fixture: ComponentFixture<HamburgerButton>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HamburgerButton],
      providers: [provideAnimations()]
    }).compileComponents();

    fixture = TestBed.createComponent(HamburgerButton);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show menu icon when closed', () => {
    fixture.componentRef.setInput('isOpen', false);
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('mat-icon')?.textContent).toBe('menu');
  });

  it('should show close icon when open', () => {
    fixture.componentRef.setInput('isOpen', true);
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('mat-icon')?.textContent).toBe('close');
  });

  it('should emit clicked event when button is clicked', () => {
    const clickSpy = jest.fn();
    component.clicked.subscribe(clickSpy);

    component.onClick();

    expect(clickSpy).toHaveBeenCalled();
  });
});
