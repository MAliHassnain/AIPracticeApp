import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AgentComponent } from './pages/agent/agent.component';
import { ChatBotComponent } from './pages/chat-bot/chat-bot.component';
import { HomeComponent } from './pages/home/home.component';
import { MultiAgentComponent } from './pages/multi-agent/multi-agent.component';
import { RagComponent } from './pages/rag/rag.component';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'home' },
  { path: 'home', component: HomeComponent, title: 'Home | AI Practice App' },
  { path: 'agent', component: AgentComponent, title: 'Agent | AI Practice App' },
  { path: 'multi-agent', component: MultiAgentComponent, title: 'Multi Agent | AI Practice App' },
  { path: 'rag', component: RagComponent, title: 'RAG | AI Practice App' },
  { path: 'chat-bot', component: ChatBotComponent, title: 'Chat Bot | AI Practice App' },
  { path: '**', redirectTo: 'home' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
