import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';

import { Recipe, RecipeRunResult } from '../../models/recipe.models';
import { AgentActivityComponent } from '../../shared/agent-activity/agent-activity.component';
import { AgentComponent } from './agent.component';
import { RecipeDetailComponent } from './recipe-detail/recipe-detail.component';
import { RecipeHistoryComponent } from './recipe-history/recipe-history.component';
import { RecipeRunComponent } from './recipe-run/recipe-run.component';

const recipe: Recipe = {
  id: 'r1',
  title: 'Chicken Karahi',
  description: 'A classic wok curry.',
  servings: 4,
  totalTime: '45 minutes',
  ingredients: [{ item: 'Chicken', quantity: '1 kg' }],
  method: ['Cook the chicken.'],
  sourceUrl: '',
  createdUtc: '2026-11-18T10:00:00Z'
};

describe('AgentComponent', () => {
  let fixture: ComponentFixture<AgentComponent>;
  let component: AgentComponent;
  let http: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AgentComponent, RecipeRunComponent, RecipeHistoryComponent, RecipeDetailComponent, AgentActivityComponent],
      imports: [FormsModule, HttpClientTestingModule]
    }).compileComponents();

    http = TestBed.inject(HttpTestingController);
    fixture = TestBed.createComponent(AgentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();

    http.expectOne('/api/agents/recipe/info').flush({ model: 'test-model', maxTurns: 8, defaultGoal: 'Pick dinner.' });
    http.expectOne('/api/agents/recipe/history').flush([recipe]);
    fixture.detectChanges();
  });

  afterEach(() => http.verify());

  it('should load agent info and history', () => {
    expect(component.info?.model).toBe('test-model');
    expect(component.history.length).toBe(1);
    expect(fixture.nativeElement.querySelector('.tab-count').textContent).toContain('1');
  });

  it('should show the saved recipe after a run and refresh history', () => {
    const result: RecipeRunResult = { finalAnswer: 'Enjoy!', recipe, steps: [], stopReason: 'end_turn', turns: 3 };

    component.run('spicy');
    const request = http.expectOne('/api/agents/recipe/run');
    expect(request.request.body).toEqual({ goal: 'spicy' });
    request.flush(result);
    http.expectOne('/api/agents/recipe/history').flush([recipe]);
    fixture.detectChanges();

    expect(component.running).toBeFalse();
    expect(fixture.nativeElement.querySelector('.recipe-title').textContent).toContain('Chicken Karahi');
  });

  it('should show the server error detail when a run fails', () => {
    component.run('');
    http.expectOne('/api/agents/recipe/run').flush(
      { title: 'Configuration error', detail: 'API key is not set.' },
      { status: 500, statusText: 'Server Error' });
    fixture.detectChanges();

    expect(component.error).toBe('API key is not set.');
    expect(fixture.nativeElement.querySelector('.alert-soft').textContent).toContain('API key is not set.');
  });

  it('should switch to the History tab', () => {
    const tabs = fixture.nativeElement.querySelectorAll('.tab') as NodeListOf<HTMLButtonElement>;
    tabs[1].click();
    fixture.detectChanges();

    expect(fixture.nativeElement.querySelector('.history-title').textContent).toContain('Chicken Karahi');
  });
});
