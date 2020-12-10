import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import * as rx from 'rxjs/operators';
import { isNil } from 'lodash';
import { User } from '../model/auth.model';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  currentUser: Observable<User>;
  currentUserName: Observable<string>;
  isAdmin: Observable<boolean>;

  constructor(public auth: AuthService, private router: Router) { }

  ngOnInit() {
    // check for logged user - menu visibility
    this.currentUser = this.auth.currentUser.pipe(
      rx.shareReplay(1)
    );
    this.currentUserName = this.currentUser.pipe(
      rx.map(user => user ? user.nick : null),
      rx.take(1)
    );

  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  myData() {
    this.router.navigate(['/userPanel']);
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
