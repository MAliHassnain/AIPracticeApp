import { Component, Input, OnChanges } from '@angular/core';
import { getStepDisplay, StepTone } from '../../helpers/agent-step.helper';
import { AgentStep } from '../../models/agent.models';

interface ActivityItem {
  turn: number;
  detail: string;
  label: string;
  tone: StepTone;
}

/** Collapsible timeline of what an agent did during a run. */
@Component({
  selector: 'app-agent-activity',
  templateUrl: './agent-activity.component.html'
})
export class AgentActivityComponent implements OnChanges {
  @Input() steps: AgentStep[] = [];
  @Input() turns = 0;

  public expanded = false;
  public items: ActivityItem[] = [];

  ngOnChanges() {
    this.items = this.steps.map((step) => ({
      turn: step.turn,
      detail: step.detail,
      ...getStepDisplay(step.kind)
    }));
  }
}
