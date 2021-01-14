import { Bid } from './../lombard.model';
import { isNil } from 'lodash';
import { AuthService } from 'src/app/auth/auth.service';
import { combineLatest, Observable, Subscription, BehaviorSubject } from 'rxjs';
import { Component, OnInit, OnDestroy } from '@angular/core';
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
export class LombardDetailsComponent implements OnInit, OnDestroy {

  _product = new BehaviorSubject<LombardProduct>(null);

  bidAmount: FormControl;

  isUserActive: Observable<boolean>;

  private _subscription = new Subscription();

  constructor(private route: ActivatedRoute,
    private router: Router,
    private lombardService: LombardService,
    private auth: AuthService) { }

  ngOnInit() {
    const productId = parseInt(this.route.snapshot.params.id, 10);
    this.bidAmount = new FormControl(0, Validators.required);

    this._subscription.add(
      this.lombardService.lombardProductById(productId).subscribe(p => {
        this._product.next(p);
        this.bidAmount.setValue(p ? p.winningBid ? (p.winningBid.money + 5) : (p.startingBid ? p.startingBid.money + 5 : 0) : 0)
      })
    );

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
      rx.first(),
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
    ).subscribe(p => {
      this._product.next(p.item);
      this.bidAmount.setValue(p ? p.item.winningBid ? (p.item.winningBid.money + 5) : (p.item.startingBid.money + 5) : 0)
    });
  }

  get product(): Observable<LombardProduct> {
    return this._product.asObservable();
  }

  ngOnDestroy() {
    this._subscription.unsubscribe();
  }
}
