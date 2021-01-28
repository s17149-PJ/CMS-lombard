import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';
import { isNullOrUndefined } from 'util';
import * as rx from 'rxjs/operators';

@Component({
  selector: 'app-admin-user-edit',
  templateUrl: './admin-user-edit.component.html',
  styleUrls: ['./admin-user-edit.component.css']
})
export class AdminUserEditComponent implements OnInit {

  userProfile: FormGroup;
  loading = false;
  submitted = false;
  returnUrl: string;
  error: string;

  userId: number;

  private _subscription = new Subscription();

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private auth: AuthService
  ) { }

  ngOnInit() {
    this.userId = parseInt(this.route.snapshot.params.id, 10);

    this.userProfile = new FormGroup({
      nick: new FormControl('', [Validators.required]),
      name: new FormControl('', [Validators.required]),
      surname: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
    });

    this._subscription.add(
      this.auth.fetchUsers.pipe(
        rx.map(users => users.filter(u => u.id === this.userId)),
        rx.map(u => u.length > 0 ? u[0] : null)
      ).subscribe(user => {
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
    this._subscription.add(
      this.auth.edit(this.userProfile.value, this.userId).subscribe(
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
