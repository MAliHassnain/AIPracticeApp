import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { shareReplay } from 'rxjs/operators';
import { Recipe, RecipeAgentInfo, RecipeRunResult } from '../models/recipe.models';

@Injectable({
  providedIn: 'root'
})
export class RecipeAgentService {
  private readonly baseUrl = '/api/agents/recipe';
  private info$: Observable<RecipeAgentInfo> | null = null;

  constructor(private http: HttpClient) {}

  getInfo(): Observable<RecipeAgentInfo> {
    if (!this.info$) {
      this.info$ = this.http.get<RecipeAgentInfo>(`${this.baseUrl}/info`).pipe(
        shareReplay(1)
      );
    }
    return this.info$;
  }

  run(goal: string): Observable<RecipeRunResult> {
    return this.http.post<RecipeRunResult>(`${this.baseUrl}/run`, { goal });
  }

  getHistory(): Observable<Recipe[]> {
    return this.http.get<Recipe[]>(`${this.baseUrl}/history`);
  }

  deleteHistoryItem(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/history/${id}`);
  }

  clearHistory(): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/history`);
  }
}
