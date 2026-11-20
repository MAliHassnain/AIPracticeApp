import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';

import { AppComponent } from './app.component';
import { HeaderComponent } from './layout/header/header.component';
import { SidebarComponent } from './layout/sidebar/sidebar.component';

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AppComponent, HeaderComponent, SidebarComponent],
      imports: [HttpClientTestingModule, RouterTestingModule]
    }).compileComponents();
  });

  it('should render the header with the app name', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();

    const header = fixture.nativeElement.querySelector('.app-header') as HTMLElement;
    expect(header.textContent).toContain('AI Practice App');
  });

  it('should render all sidebar menu items', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();

    const labels = Array.from(fixture.nativeElement.querySelectorAll('.app-sidebar .nav-link') as NodeListOf<HTMLElement>)
      .map((el) => el.textContent?.trim());

    expect(labels).toEqual(['Home', 'Agent', 'Multi Agent', 'RAG', 'Chat Bot']);
  });
});
