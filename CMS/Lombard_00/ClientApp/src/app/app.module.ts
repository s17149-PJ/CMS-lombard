import { UserPanelComponent } from './sec-user/user-panel.component';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import {
  MatButtonModule,
  MatCardModule,
  MatChipsModule,
  MatDatepicker,
  MatDatepickerModule,
  MatDividerModule,
  MatExpansionModule,
  MatFormFieldModule,
  MatGridListModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatMenuModule,
  MatNativeDateModule,
  MatPaginatorModule,
  MatProgressBarModule,
  MatSidenavModule,
  MatSortModule,
  MatTableModule,
  MatToolbarModule,
} from '@angular/material';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './sec-nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { ErrorInterceptor } from './auth/auth-error.interceptor';
import { AuthInterceptor } from './auth/auth.interceptor';
import { AuthorizationComponent } from './auth/authorization/authorization.component';
import { AuthGuard } from './auth/auth.guard';
import { routes } from './app.routes';
import { AdminPanelComponent } from './sec-admin/admin-panel.component';
import { AdminPanelUsersComponent } from './sec-admin/admin-panel-users/admin-panel-users.component';
import { LombardComponent } from './lombard/lombard.component';
import { AdminPanelDashboardComponent } from './sec-admin/admin-panel-dashboard/admin-panel-dashboard.component';
import { LombardDetailsComponent } from './lombard/lombard-details/lombard-details.component';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { StatsCardComponent } from './sec-admin/common/stats-card/stats-card.component';
import { StatsCard2Component } from './sec-admin/common/stats-card2/stats-card2.component';
import { LombardNewComponent } from './lombard/lombard-new/lombard-new.component';

@NgModule({
  declarations: [
    AppComponent,
    AuthorizationComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    AdminPanelComponent,
    AdminPanelUsersComponent,
    AdminPanelDashboardComponent,
    LombardComponent,
    LombardNewComponent,
    UserPanelComponent,
    LombardDetailsComponent,
    StatsCardComponent,
    StatsCard2Component,
  ],
  imports: [
    ReactiveFormsModule,
    HttpClientModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    MatExpansionModule,
    MatButtonModule,
    MatCardModule,
    MatChipsModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDividerModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatListModule,
    MatMenuModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatSidenavModule,
    MatSortModule,
    MatToolbarModule,
    MatTableModule,
    RouterModule.forRoot(routes),
    NoopAnimationsModule,
    MDBBootstrapModule.forRoot(),
    BrowserAnimationsModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    AuthGuard,
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
