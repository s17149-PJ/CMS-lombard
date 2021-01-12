import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { User } from '../model/auth.model';
import * as rx from 'rxjs/operators';
import * as moment from 'moment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private currentUserSubject: BehaviorSubject<User>;

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<User>(
      JSON.parse(localStorage.getItem('currentUser'))
    );


    this.currentUser.subscribe(user => {
      if (user && user.validUntil) {
        const expTime = moment(user.validUntil);
        const now = moment.now();
        if (expTime.isBefore(now)) {
          this.logout();
        } else {
          this.login(user.nick, user.password);
        }
      }
    });
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
      );
  }

  edit(nick: string, name: string, surname: string, password: string): Observable<User> {
    var user = { id: this.currentUserValue.id, nick: nick.trim(), name: name.trim(), surname: surname.trim(), password: this.currentUserValue.token };
    var token = password.trim();
    return this.http.post<any>('api/user/edit',
      { TokenUser: user, NewPassword: token})
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

  private keepAlive(): Observable<User> {
    return this.http.post<any>('api/user/keepAlive', {
      success: this.currentUserValue.success,
      id: this.currentUserValue.id,
      nick: this.currentUserValue.nick,
      name: this.currentUserValue.name,
      surname: this.currentUserValue.surname,
      roles: this.currentUserValue.roles,
      token: this.currentUserValue.token
    }).pipe(user => user);
  }

  getRole(): string {
    return this.currentUserValue.roles.length > 0 ? this.currentUserValue.roles[0].name.toUpperCase() : 'USER';
  }

  get fetchUsers(): Observable<User[]> {
    return this.http
      .post<User[]>('api/admin/users', {
        success: this.currentUserValue.success,
        id: this.currentUserValue.id,
        nick: this.currentUserValue.nick,
        name: this.currentUserValue.name,
        surname: this.currentUserValue.surname,
        roles: this.currentUserValue.roles,
        token: this.currentUserValue.token
      })
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
