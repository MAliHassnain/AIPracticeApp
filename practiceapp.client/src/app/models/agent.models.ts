export type AgentStepKind = 'text' | 'web_search' | 'web_search_result' | 'tool_request' | 'tool_result' | 'info';

export interface AgentStep {
  turn: number;
  kind: AgentStepKind;
  detail: string;
}
