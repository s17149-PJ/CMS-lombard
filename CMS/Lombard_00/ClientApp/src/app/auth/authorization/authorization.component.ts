import { Component, OnDestroy, OnInit } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import * as rx from 'rxjs/operators';
import { AuthService } from '../auth.service';
import { isNil } from 'lodash';

@Component({
  selector: 'app-authorization',
  templateUrl: './authorization.component.html',
  styleUrls: ['./authorization.component.css'],
})
export class AuthorizationComponent implements OnInit, OnDestroy {
  loginForm: FormGroup;
  registerForm: FormGroup;
  loading = false;
  submitted = false;
  returnUrl: string;
  error: string;
  signInPanel = true;

  private _subscription = new Subscription();

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private auth: AuthService
  ) {
    // redirect to home if already logged in
    if (this.auth.currentUserValue) {
      this.router.navigate(['/']);
    }
  }

  ngOnInit() {
    this.loginForm = new FormGroup({
      username: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
    });
    this.registerForm = new FormGroup({
      username: new FormControl('', [Validators.required]),
      name: new FormControl('', [Validators.required]),
      surname: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
    });
  }

  // convenience getter for easy access to form fields
  get f() {
    return this.loginForm.controls;
  }

  onSubmitLogin() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    const loginUser: { username: string; password: string } = {
      ...this.loginForm.value,
    };
    this._subscription.add(
      this.auth.login(loginUser.username, loginUser.password).subscribe(
        (user) => {
          if (!isNil(user)) {
            this.router.navigate(['/']);
          } else {
            this.error = 'Login failed!';
            this.loading = false;
          }
        },
        (errors) => {
          this.error = 'Login failed!';
          this.loading = false;
        }
      )
    );
  }

  onSubmitRegister() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.registerForm.invalid) {
      return;
    }

    this.loading = true;
    const registerUser: { username: string; name: string; surname: string; password: string } = {
      ...this.registerForm.value,
    };
    this._subscription.add(
      this.auth.register(registerUser.username, registerUser.name,
        registerUser.surname, registerUser.password).subscribe(
          (user) => {
            if (!isNil(user)) {
              this.router.navigate(['/']);
            } else {
              this.error = 'Register failed!';
              this.loading = false;
            }
          },
          (errors) => {
            this.error = errors;
            this.loading = false;
          }
        )
    );
  }

  ngOnDestroy() {
    this._subscription.unsubscribe();
  }

  toggleSignIn(signIn: boolean) {
    this.signInPanel = signIn;
    this.error = null;
  }
}
