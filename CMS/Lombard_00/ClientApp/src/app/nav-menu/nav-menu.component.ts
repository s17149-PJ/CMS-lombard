import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import * as rx from 'rxjs/operators';
import { isNil } from 'lodash';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  isLogged: Observable<boolean>;

  constructor(private auth: AuthService, private router: Router) {}

  ngOnInit() {
    this.isLogged = this.auth.currentUser.pipe(rx.map((user) => !isNil(user)));
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
