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

  // i put it here cuz i can't get angular paths to work. will need to ask today in a few hours.

  edit(nick: string, name: string, surname: string, password: string)/*: Observable<User>*/ {
    //return this.http.post<any>('api/user/edit',
    //  { nick: nick.trim(), name: name.trim(), surname: surname.trim(), password: password.trim() })
    //  .pipe(
    //    rx.map((user: User) => {
    //      if (user.success) {
    //        user.authdata = window.btoa(user.token);
    //        localStorage.setItem('currentUser', JSON.stringify(user));
    //        this.currentUserSubject.next(user);
    //        return user;
    //      } else {
    //        return null;
    //      }
    //    })
    //  );
  }
}
