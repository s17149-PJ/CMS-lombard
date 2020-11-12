import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import {
  MatButtonModule,
  MatCardModule,
  MatChipsModule,
  MatDividerModule,
  MatFormFieldModule,
  MatGridListModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatPaginatorModule,
  MatSidenavModule,
  MatSortModule,
  MatTableModule,
  MatToolbarModule,
} from '@angular/material';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { ErrorInterceptor } from './auth/auth-error.interceptor';
import { AuthInterceptor } from './auth/auth.interceptor';
import { AuthorizationComponent } from './auth/authorization/authorization.component';
import { AuthGuard } from './auth/auth.guard';
import { routes } from './app.routes';
import { AdminPanelComponent } from './admin/admin-panel.component';
import { AdminPanelUsersComponent } from './admin/admin-panel-users/admin-panel-users.component';
import { LombardComponent } from './lombard/lombard.component';
import { AdminPanelDashboardComponent } from './admin/admin-panel-dashboard/admin-panel-dashboard.component';

@NgModule({
  declarations: [
    AppComponent,
    AuthorizationComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    AdminPanelComponent,
    AdminPanelUsersComponent,
    AdminPanelDashboardComponent,
    LombardComponent,
  ],
  imports: [
    ReactiveFormsModule,
    HttpClientModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatDividerModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatListModule,
    MatMenuModule,
    MatPaginatorModule,
    MatSidenavModule,
    MatSortModule,
    MatToolbarModule,
    MatTableModule,
    RouterModule.forRoot(routes),
    NoopAnimationsModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    AuthGuard,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
