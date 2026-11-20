import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { HeaderComponent } from './header.component';

describe('HeaderComponent', () => {
  let fixture: ComponentFixture<HeaderComponent>;
  let component: HeaderComponent;
  let http: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HeaderComponent],
      imports: [HttpClientTestingModule, RouterTestingModule]
    }).compileComponents();

    http = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  afterEach(() => http.verify());

  it('should render the app name', () => {
    http.expectOne('/api/agents/recipe/info').flush({ model: 'claude-haiku-4-5', maxTurns: 8, defaultGoal: '' });
    fixture.detectChanges();

    expect(fixture.nativeElement.textContent).toContain('AI Practice App');
  });

  it('should render the AI model chip in the header when loaded', () => {
    http.expectOne('/api/agents/recipe/info').flush({ model: 'claude-haiku-4-5', maxTurns: 8, defaultGoal: '' });
    fixture.detectChanges();

    const modelBadge = fixture.nativeElement.querySelector('.header-model');
    expect(modelBadge).toBeTruthy();
    expect(modelBadge.textContent).toContain('claude-haiku-4-5');
  });

  it('should emit toggleSidebar when collapse button clicked', () => {
    http.expectOne('/api/agents/recipe/info').flush({ model: '', maxTurns: 8, defaultGoal: '' });

    spyOn(component.toggleSidebar, 'emit');
    const button = fixture.nativeElement.querySelector('.icon-btn') as HTMLButtonElement;
    button.click();

    expect(component.toggleSidebar.emit).toHaveBeenCalled();
  });
});
