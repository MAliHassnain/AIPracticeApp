import { AgentStep } from './agent.models';

export interface Ingredient {
  item: string;
  quantity: string;
}

export interface Recipe {
  id: string;
  title: string;
  description: string;
  servings: number | null;
  totalTime: string;
  ingredients: Ingredient[];
  method: string[];
  sourceUrl: string;
  createdUtc: string;
}

export interface RecipeAgentInfo {
  model: string;
  maxTurns: number;
  defaultGoal: string;
}

export interface RecipeRunResult {
  finalAnswer: string;
  recipe: Recipe | null;
  steps: AgentStep[];
  stopReason: string;
  turns: number;
}
