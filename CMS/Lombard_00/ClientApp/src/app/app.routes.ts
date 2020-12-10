import { Route } from '@angular/router';
import { AdminPanelDashboardComponent } from './sec-admin/admin-panel-dashboard/admin-panel-dashboard.component';
import { AdminPanelUsersComponent } from './sec-admin/admin-panel-users/admin-panel-users.component';
import { AdminPanelComponent } from './sec-admin/admin-panel.component';
import { AuthGuard } from './auth/auth.guard';
import { AuthorizationComponent } from './auth/authorization/authorization.component';
import { CounterComponent } from './counter/counter.component';
import { HomeComponent } from './home/home.component';
import { LombardDetailsComponent } from './lombard/lombard-details/lombard-details.component';
import { LombardComponent } from './lombard/lombard.component';
import { UserPanelComponent } from './sec-user/user-panel.component';
import { LombardNewComponent } from './lombard/lombard-new/lombard-new.component';

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
  { path: 'userPanel', component: UserPanelComponent },
  {
    path: 'admin',
    canActivate: [AuthGuard],
    data: {
      role: 'ADMIN',
    },
    children: [
      { path: 'panel', component: AdminPanelDashboardComponent },
      { path: 'users', component: AdminPanelUsersComponent },
    ],
  },
  {
    path: 'lombard',
    component: LombardComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'lombard/new',
    component: LombardNewComponent,
    canActivate: [AuthGuard],
  },
  {
    path: 'product-details/:id',
    component: LombardDetailsComponent,
  },
];
