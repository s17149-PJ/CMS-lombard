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
import { isNullOrUndefined } from 'util';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-user-panel',
  templateUrl: './user-panel.component.html',
  styleUrls: ['./user-panel.component.css']
})
export class UserPanelComponent implements OnInit {
  userProfile: FormGroup;
  loading = false;
  submitted = false;
  returnUrl: string;
  error: string;

  private _subscription = new Subscription();

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private auth: AuthService
  ) { }

  ngOnInit() {
    this.userProfile = new FormGroup({
      nick: new FormControl('', [Validators.required]),
      name: new FormControl('', [Validators.required]),
      surname: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
    });

    this._subscription.add(
      this.auth.currentUser.subscribe(user => {
        this.userProfile.patchValue(user);
      })
    )
  }

  onSubmitSave() {
    this.submitted = true;

    // stop here if form is invalid
    if (this.userProfile.invalid) {
      return;
    }

    this.loading = true;
    const updatedUser: {
      username: string;
      name: string;
      surname: string;
      password: string
    } = {
      ...this.userProfile.value,
    };
    this._subscription.add(
      this.auth.edit(updatedUser.username, updatedUser.name,
        updatedUser.surname, updatedUser.password).subscribe(
          (user) => {
            if (!isNullOrUndefined(user)) {
              this.router.navigate(['/']);
            } else {
              this.error = 'Edit failed!';
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
}
