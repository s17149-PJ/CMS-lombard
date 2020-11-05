import { Route } from '@angular/router';
import { AuthGuard } from './auth/auth.guard';
import { AuthorizationComponent } from './auth/authorization/authorization.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { HomeComponent } from './home/home.component';

export const routes: Route[] = [
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full',
    canActivate: [AuthGuard],
  },
  {
    path: 'counter',
    component: CounterComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'fetch-data',
    component: FetchDataComponent,
    canActivate: [AuthGuard],
  },
  { path: 'login', component: AuthorizationComponent },
];
