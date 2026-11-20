import { Component, EventEmitter, Input, Output } from '@angular/core';
import { RecipeAgentInfo, RecipeRunResult } from '../../../models/recipe.models';

@Component({
  selector: 'app-recipe-run',
  templateUrl: './recipe-run.component.html'
})
export class RecipeRunComponent {
  @Input() info: RecipeAgentInfo | null = null;
  @Input() running = false;
  @Input() result: RecipeRunResult | null = null;
  @Output() run = new EventEmitter<string>();

  public goal = '';

  submit() {
    if (!this.running) {
      this.run.emit(this.goal);
    }
  }
}
