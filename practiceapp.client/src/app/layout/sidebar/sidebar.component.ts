import { Component } from '@angular/core';

export interface NavItem {
  label: string;
  route: string;
  icon: string;
}

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html'
})
export class SidebarComponent {
  public navItems: NavItem[] = [
    { label: 'Home', route: '/home', icon: 'bi-grid' },
    { label: 'Agent', route: '/agent', icon: 'bi-robot' },
    { label: 'Multi Agent', route: '/multi-agent', icon: 'bi-diagram-3' },
    { label: 'RAG', route: '/rag', icon: 'bi-database' },
    { label: 'Chat Bot', route: '/chat-bot', icon: 'bi-chat-dots' }
  ];
}
