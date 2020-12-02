import { Route } from '@angular/router';
import { AdminPanelDashboardComponent } from './sec-admin/admin-panel-dashboard/admin-panel-dashboard.component';
import { AdminPanelUsersComponent } from './sec-admin/admin-panel-users/admin-panel-users.component';
import { AdminPanelComponent } from './sec-admin/admin-panel.component';
import { AuthGuard } from './auth/auth.guard';
import { AuthorizationComponent } from './auth/authorization/authorization.component';
import { CounterComponent } from './counter/counter.component';
import { HomeComponent } from './home/home.component';
import { LombardDetailsComponent } from './sec-lombard/lombard-details/lombard-details.component';
import { LombardComponent } from './sec-lombard/lombard.component';

export const routes: Route[] = [
  {
    path: '',
    component: HomeComponent,
    pathMatch: 'full',
    // canActivate: [AuthGuard],
  },
  {
    path: 'counter',
    component: CounterComponent,
    canActivate: [AuthGuard],
  },
  { path: 'login', component: AuthorizationComponent },
  {
    path: 'admin',
    canActivate: [AuthGuard],
    children: [
      { path: 'panel', component: AdminPanelDashboardComponent },
      { path: 'users', component: AdminPanelUsersComponent },
    ],
  },
  {
    path: 'lombard',
    component: LombardComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'details', component: LombardDetailsComponent },
    ],
  },
];
