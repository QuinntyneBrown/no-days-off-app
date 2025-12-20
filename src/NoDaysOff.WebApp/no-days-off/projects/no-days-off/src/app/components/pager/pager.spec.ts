import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Pager } from './pager';
import { provideAnimations } from '@angular/platform-browser/animations';

describe('Pager', () => {
  let component: Pager;
  let fixture: ComponentFixture<Pager>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Pager],
      providers: [provideAnimations()]
    }).compileComponents();

    fixture = TestBed.createComponent(Pager);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display current page and total pages', () => {
    fixture.componentRef.setInput('pageNumber', 3);
    fixture.componentRef.setInput('totalPages', 10);
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('.pager__current')?.textContent).toBe('3');
    expect(compiled.querySelector('.pager__total')?.textContent).toBe('10');
  });

  it('should emit next page number when clicking next', () => {
    fixture.componentRef.setInput('pageNumber', 2);
    fixture.componentRef.setInput('totalPages', 5);
    fixture.detectChanges();

    const nextSpy = jest.fn();
    component.next.subscribe(nextSpy);

    component.emitNext();

    expect(nextSpy).toHaveBeenCalledWith({ pageNumber: 3 });
  });

  it('should wrap to first page when at last page and clicking next', () => {
    fixture.componentRef.setInput('pageNumber', 5);
    fixture.componentRef.setInput('totalPages', 5);
    fixture.detectChanges();

    const nextSpy = jest.fn();
    component.next.subscribe(nextSpy);

    component.emitNext();

    expect(nextSpy).toHaveBeenCalledWith({ pageNumber: 1 });
  });

  it('should emit previous page number when clicking previous', () => {
    fixture.componentRef.setInput('pageNumber', 3);
    fixture.componentRef.setInput('totalPages', 5);
    fixture.detectChanges();

    const previousSpy = jest.fn();
    component.previous.subscribe(previousSpy);

    component.emitPrevious();

    expect(previousSpy).toHaveBeenCalledWith({ pageNumber: 2 });
  });

  it('should wrap to last page when at first page and clicking previous', () => {
    fixture.componentRef.setInput('pageNumber', 1);
    fixture.componentRef.setInput('totalPages', 5);
    fixture.detectChanges();

    const previousSpy = jest.fn();
    component.previous.subscribe(previousSpy);

    component.emitPrevious();

    expect(previousSpy).toHaveBeenCalledWith({ pageNumber: 5 });
  });
});
