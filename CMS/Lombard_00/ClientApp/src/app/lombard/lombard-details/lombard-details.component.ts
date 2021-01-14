import { Bid } from './../lombard.model';
import { isNil } from 'lodash';
import { AuthService } from 'src/app/auth/auth.service';
import { combineLatest, Observable } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LombardService } from '../lombard.service';
import { LombardProduct } from '../lombard.model';
import * as rx from 'rxjs/operators';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-lombard-details',
  templateUrl: './lombard-details.component.html',
  styleUrls: ['./lombard-details.component.css']
})
export class LombardDetailsComponent implements OnInit {

  product: Observable<LombardProduct>;

  bidAmount: FormControl;

  isUserActive: Observable<boolean>;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private lombardService: LombardService,
    private auth: AuthService) { }

  ngOnInit() {
    const productId = parseInt(this.route.snapshot.params.id, 10);
    this.product = this.lombardService.lombardProductById(productId);

    this.bidAmount = new FormControl(0, Validators.required);

    this.isUserActive = this.auth.currentUser.pipe(
      rx.map(user => !isNil(user)),
      rx.shareReplay(1)
    )
  }

  submitBid() {
    combineLatest([
      this.auth.currentUser,
      this.product
    ]).pipe(
      rx.switchMap(([user, product]) => {
        const bid: Bid = {
          userId: user.id,
          token: user.token,
          subjectId: product.id,
          money: this.bidAmount.value,
          isRating: false
        }
        return this.lombardService.createBid(bid);
      })
    ).subscribe(resp => console.log(resp));
  }

}
