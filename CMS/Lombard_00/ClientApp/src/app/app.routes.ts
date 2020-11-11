import { Route } from '@angular/router';
import { AdminPanelUsersComponent } from './admin/admin-panel-users/admin-panel-users.component';
import { AdminPanelComponent } from './admin/admin-panel.component';
import { AuthGuard } from './auth/auth.guard';
import { AuthorizationComponent } from './auth/authorization/authorization.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { HomeComponent } from './home/home.component';
import { LombardComponent } from './lombard/lombard.component';

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
  {
    path: 'admin',
    canActivate: [AuthGuard],
    children: [
      { path: 'panel', component: AdminPanelComponent },
      { path: 'users', component: AdminPanelUsersComponent },
    ],
  },
  {
    path: 'lombard',
    component: LombardComponent,
    canActivate: [AuthGuard],
  },
];
