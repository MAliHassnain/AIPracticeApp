import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './layout/header/header.component';
import { SidebarComponent } from './layout/sidebar/sidebar.component';
import { AgentComponent } from './pages/agent/agent.component';
import { RecipeDetailComponent } from './pages/agent/recipe-detail/recipe-detail.component';
import { RecipeHistoryComponent } from './pages/agent/recipe-history/recipe-history.component';
import { RecipeRunComponent } from './pages/agent/recipe-run/recipe-run.component';
import { ChatBotComponent } from './pages/chat-bot/chat-bot.component';
import { HomeComponent } from './pages/home/home.component';
import { MultiAgentComponent } from './pages/multi-agent/multi-agent.component';
import { RagComponent } from './pages/rag/rag.component';
import { AgentActivityComponent } from './shared/agent-activity/agent-activity.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    SidebarComponent,
    HomeComponent,
    AgentComponent,
    RecipeRunComponent,
    RecipeHistoryComponent,
    RecipeDetailComponent,
    AgentActivityComponent,
    MultiAgentComponent,
    RagComponent,
    ChatBotComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
