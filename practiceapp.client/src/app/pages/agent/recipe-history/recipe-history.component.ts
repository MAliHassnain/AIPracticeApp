import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Recipe } from '../../../models/recipe.models';

@Component({
  selector: 'app-recipe-history',
  templateUrl: './recipe-history.component.html'
})
export class RecipeHistoryComponent {
  @Input() recipes: Recipe[] = [];
  @Output() remove = new EventEmitter<Recipe>();
  @Output() clearAll = new EventEmitter<void>();
  @Output() goToMain = new EventEmitter<void>();

  public openId: string | null = null;

  toggle(id: string) {
    this.openId = this.openId === id ? null : id;
  }

  confirmRemove(recipe: Recipe) {
    if (confirm(`Delete "${recipe.title}"?`)) {
      this.remove.emit(recipe);
    }
  }

  confirmClear() {
    if (confirm('Delete all saved recipes? The agent will forget them.')) {
      this.clearAll.emit();
    }
  }

  trackById(_: number, recipe: Recipe): string {
    return recipe.id;
  }
}
