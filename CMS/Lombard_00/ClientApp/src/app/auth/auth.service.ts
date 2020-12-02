import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { User } from '../model/auth.model';
import * as rx from 'rxjs/operators';
import { trim } from 'src/app/sec-shared-nav-menu/node_modules/lodash';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<User>;

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<User>(
      JSON.parse(localStorage.getItem('currentUser'))
    );
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  login(nick: string, password: string): Observable<User> {
    return this.http
      .post<any>('api/user/login', { nick: nick.trim(), password: password.trim() })
      .pipe(
        rx.map((user: User) => {
          if (user.success) {
            user.authdata = window.btoa(user.token);
            localStorage.setItem('currentUser', JSON.stringify(user));
            this.currentUserSubject.next(user);
            return user;
          } else {
            return null;
          }
        })
      );
  }

  register(nick: string, name: string, surname: string, password: string): Observable<User> {
    return this.http.post<any>('api/user/register',
      { nick: nick.trim(), name: name.trim(), surname: surname.trim(), password: password.trim() })
      .pipe(
        rx.map((user: User) => {
          if (user.success) {
            user.authdata = window.btoa(user.token);
            localStorage.setItem('currentUser', JSON.stringify(user));
            this.currentUserSubject.next(user);
            return user;
          } else {
            return null;
          }
        })
      )
  }

  get fetchUsers(): Observable<User[]> {
    const httpParams: HttpParams = new HttpParams();
    httpParams.set('id', this.currentUserValue.id.toString());
    httpParams.set('name', this.currentUserValue.name);
    httpParams.set('token', this.currentUserValue.token);
    return this.http
      .get<User[]>('api/admin/users', { params: httpParams })
      .pipe(rx.map((users) => users));
  }

  logout() {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  get currentUser(): Observable<User> {
    return this.currentUserSubject.asObservable();
  }
}
