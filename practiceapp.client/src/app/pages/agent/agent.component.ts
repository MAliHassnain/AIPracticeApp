import { Component, OnInit } from '@angular/core';
import { describeHttpError } from '../../helpers/http-error.helper';
import { Recipe, RecipeAgentInfo, RecipeRunResult } from '../../models/recipe.models';
import { RecipeAgentService } from '../../services/recipe-agent.service';

export type AgentTab = 'main' | 'history';

@Component({
  selector: 'app-agent',
  templateUrl: './agent.component.html'
})
export class AgentComponent implements OnInit {
  public activeTab: AgentTab = 'main';
  public info: RecipeAgentInfo | null = null;
  public history: Recipe[] = [];
  public result: RecipeRunResult | null = null;
  public running = false;
  public error = '';

  constructor(private recipeAgent: RecipeAgentService) {}

  ngOnInit() {
    this.recipeAgent.getInfo().subscribe({
      next: (info) => (this.info = info),
      error: (e) => this.showError(e)
    });
    this.loadHistory();
  }

  run(goal: string) {
    if (this.running) return;

    this.running = true;
    this.error = '';
    this.result = null;

    this.recipeAgent.run(goal).subscribe({
      next: (result) => {
        this.result = result;
        this.running = false;
        this.loadHistory();
      },
      error: (e) => {
        this.running = false;
        this.showError(e);
      }
    });
  }

  removeRecipe(recipe: Recipe) {
    this.recipeAgent.deleteHistoryItem(recipe.id).subscribe({
      next: () => this.loadHistory(),
      error: (e) => this.showError(e)
    });
  }

  clearHistory() {
    this.recipeAgent.clearHistory().subscribe({
      next: () => this.loadHistory(),
      error: (e) => this.showError(e)
    });
  }

  private loadHistory() {
    this.recipeAgent.getHistory().subscribe({
      next: (recipes) => (this.history = recipes),
      error: (e) => this.showError(e)
    });
  }

  private showError(error: unknown) {
    this.error = describeHttpError(error);
  }
}
