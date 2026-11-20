import { Component, OnInit } from '@angular/core';
import { HomeService } from '../../services/home.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {
  public message = '';
  public loaded = false;
  public error = '';

  public features = [
    { title: 'Agent', route: '/agent', icon: 'bi-robot', description: 'A single agent that reasons and uses tools to complete a task.' },
    { title: 'Multi Agent', route: '/multi-agent', icon: 'bi-diagram-3', description: 'Several agents that collaborate on one goal.' },
    { title: 'RAG', route: '/rag', icon: 'bi-database', description: 'Answers grounded in your own documents.' },
    { title: 'Chat Bot', route: '/chat-bot', icon: 'bi-chat-dots', description: 'A conversational assistant with message history.' }
  ];

  constructor(private homeService: HomeService) {}

  ngOnInit() {
    this.homeService.getStatus().subscribe({
      next: (result) => {
        this.message = result.message;
        this.loaded = true;
      },
      error: () => {
        this.loaded = true;
        this.error = 'The practice service is unavailable right now.';
      }
    });
  }
}
