import { AuthService } from 'src/app/auth/auth.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../model/auth.model';
import * as rx from 'rxjs/operators';
import { AdminStatistics } from './admin.model';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private authService: AuthService, private http: HttpClient) { }

  get fetchStatistics(): Observable<AdminStatistics> {
    return this.http
      .post<AdminStatistics>('api/admin/stats', {
        success: this.authService.currentUserValue.success,
        id: this.authService.currentUserValue.id,
        nick: this.authService.currentUserValue.nick,
        name: this.authService.currentUserValue.name,
        surname: this.authService.currentUserValue.surname,
        roles: this.authService.currentUserValue.roles,
        token: this.authService.currentUserValue.token
      })
      .pipe(
        rx.map((stats) => stats),
        rx.tap(x => console.log(x))
      );
  }
}
