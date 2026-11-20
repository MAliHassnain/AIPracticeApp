import { AgentStepKind } from '../models/agent.models';

export type StepTone = 'thought' | 'search' | 'tool' | 'info';

interface StepDisplay {
  label: string;
  tone: StepTone;
}

const STEP_DISPLAY: Record<AgentStepKind, StepDisplay> = {
  text: { label: 'Thought', tone: 'thought' },
  web_search: { label: 'Search', tone: 'search' },
  web_search_result: { label: 'Results', tone: 'search' },
  tool_request: { label: 'Tool call', tone: 'tool' },
  tool_result: { label: 'Tool result', tone: 'tool' },
  info: { label: 'Info', tone: 'info' }
};

export function getStepDisplay(kind: string): StepDisplay {
  return STEP_DISPLAY[kind as AgentStepKind] ?? STEP_DISPLAY.info;
}
