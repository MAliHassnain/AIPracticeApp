import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface HomeStatus {
  title: string;
  message: string;
}

@Injectable({ providedIn: 'root' })
export class HomeService {
  constructor(private http: HttpClient) {}

  getStatus(): Observable<HomeStatus> {
    return this.http.get<HomeStatus>('/api/home');
  }
}
