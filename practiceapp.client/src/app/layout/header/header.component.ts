import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { RecipeAgentService } from '../../services/recipe-agent.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent implements OnInit {
  @Output() toggleSidebar = new EventEmitter<void>();

  public appName = 'AI Practice App';
  public model = '';

  constructor(private recipeAgent: RecipeAgentService) {}

  ngOnInit() {
    this.recipeAgent.getInfo().subscribe({
      next: (info) => (this.model = info.model),
      error: () => { /* gracefully leave empty if not reachable */ }
    });
  }
}
