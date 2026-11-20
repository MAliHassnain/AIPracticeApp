import { Location } from '@angular/common';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed, fakeAsync, tick } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { AppComponent } from './app.component';
import { HeaderComponent } from './layout/header/header.component';
import { SidebarComponent } from './layout/sidebar/sidebar.component';
import { AgentComponent } from './pages/agent/agent.component';
import { RecipeDetailComponent } from './pages/agent/recipe-detail/recipe-detail.component';
import { RecipeHistoryComponent } from './pages/agent/recipe-history/recipe-history.component';
import { RecipeRunComponent } from './pages/agent/recipe-run/recipe-run.component';
import { AgentActivityComponent } from './shared/agent-activity/agent-activity.component';
import { ChatBotComponent } from './pages/chat-bot/chat-bot.component';
import { HomeComponent } from './pages/home/home.component';
import { MultiAgentComponent } from './pages/multi-agent/multi-agent.component';
import { RagComponent } from './pages/rag/rag.component';
import { routes } from './app-routing.module';

describe('App routing', () => {
  let router: Router;
  let location: Location;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        AppComponent,
        HeaderComponent,
        SidebarComponent,
        HomeComponent,
        AgentComponent,
        RecipeRunComponent,
        RecipeHistoryComponent,
        RecipeDetailComponent,
        AgentActivityComponent,
        MultiAgentComponent,
        RagComponent,
        ChatBotComponent
      ],
      imports: [FormsModule, HttpClientTestingModule, RouterTestingModule.withRoutes(routes)]
    }).compileComponents();

    router = TestBed.inject(Router);
    location = TestBed.inject(Location);
    TestBed.createComponent(AppComponent);
  });

  it('should redirect the empty path to /home', fakeAsync(() => {
    router.navigate(['']);
    tick();
    expect(location.path()).toBe('/home');
  }));

  it('should redirect unknown paths to /home', fakeAsync(() => {
    router.navigate(['does-not-exist']);
    tick();
    expect(location.path()).toBe('/home');
  }));

  for (const path of ['agent', 'multi-agent', 'rag', 'chat-bot']) {
    it(`should navigate to /${path}`, fakeAsync(() => {
      router.navigate([path]);
      tick();
      expect(location.path()).toBe(`/${path}`);
    }));
  }
});
