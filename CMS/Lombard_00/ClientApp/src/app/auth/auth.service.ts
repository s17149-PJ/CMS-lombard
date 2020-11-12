import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { User } from '../model/auth.model';
import * as rx from 'rxjs/operators';

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
      .post<any>('api/user/login', { nick, password })
      .pipe(
        rx.map((user) => {
          user.authdata = window.btoa(nick + ':' + password);
          localStorage.setItem('currentUser', JSON.stringify(user));
          this.currentUserSubject.next(user);
          return user;
        })
      );
  }

  get fetchUsers(): Observable<User[]> {
    return this.http
      .get<User[]>('api/user/list')
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
