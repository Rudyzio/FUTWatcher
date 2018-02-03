import { BrowserModule, } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToasterModule } from 'angular2-toaster';
import { ChartModule } from 'angular2-chartjs';
import { NgxPaginationModule } from 'ngx-pagination';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { PlayersComponent } from './components/players/players.component';
import { SearchComponent } from './components/search/search.component';
import { InfoUploaderComponent } from './components/info_uploader/info_uploader.component';
import { StatisticsComponent } from './components/statistics/statistics.component';
import { LeaguesComponent } from './components/players/leagues/leagues.component';
import { NationsComponent } from './components/players/nations/nations.component';
import { PlayersTableComponent } from './components/players_table/players_table.component';
import { MarketWatcherComponent } from './components/market_watcher/market_watcher.component';
import { AuthenticationService } from './services/authentication.service';
import { AuthGuard } from './services/auth.guard';
import { AlertService } from './services/alert.service';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { RatingsComponent } from './components/players/rating/ratings.component';
import { PlayerOptionsService } from './services/player_options.service';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    PlayersComponent,
    SearchComponent,
    InfoUploaderComponent,
    LeaguesComponent,
    LoginComponent,
    MarketWatcherComponent,
    NationsComponent,
    PlayersTableComponent,
    RatingsComponent,
    RegisterComponent,
    StatisticsComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    ChartModule,
    HttpModule,
    FormsModule,
    NgxPaginationModule,
    RouterModule.forRoot([
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'players', component: PlayersComponent, canActivate: [AuthGuard] },
      { path: 'search', component: SearchComponent, canActivate: [AuthGuard] },
      { path: 'infouploader', component: InfoUploaderComponent, canActivate: [AuthGuard] },
      { path: 'statistics', component: StatisticsComponent, canActivate: [AuthGuard] },
      { path: 'marketwatcher', component: MarketWatcherComponent, canActivate: [AuthGuard] },
      { path: '**', redirectTo: 'login' }
    ]),
    ToasterModule
  ],
  providers: [
    AuthenticationService,
    AuthGuard,
    AlertService,
    PlayerOptionsService,
    { provide: 'ORIGIN_URL', useValue: location.origin },
    // { provide: 'API_URL', useValue: "http://yourwebsite.com/api/" }
    { provide: 'API_URL', useValue: "http://localhost:5000/api/" }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
