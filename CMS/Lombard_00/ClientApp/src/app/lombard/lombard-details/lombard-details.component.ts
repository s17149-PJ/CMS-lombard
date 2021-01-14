import { Bid, ItemBid } from './../lombard.model';
import { isNil, max } from 'lodash';
import { AuthService } from 'src/app/auth/auth.service';
import { combineLatest, Observable, Subscription, BehaviorSubject } from 'rxjs';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LombardService } from '../lombard.service';
import { LombardProduct } from '../lombard.model';
import * as rx from 'rxjs/operators';
import { FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-lombard-details',
  templateUrl: './lombard-details.component.html',
  styleUrls: ['./lombard-details.component.css']
})
export class LombardDetailsComponent implements OnInit, OnDestroy {

  _product = new BehaviorSubject<LombardProduct>(null);

  bidAmount: FormControl;

  isUserActive: Observable<boolean>;

  ownHighestBid: Observable<boolean>;

  private _subscription = new Subscription();

  constructor(private route: ActivatedRoute,
    private router: Router,
    private lombardService: LombardService,
    private auth: AuthService,
    private _snackBar: MatSnackBar) { }

  ngOnInit() {
    const productId = parseInt(this.route.snapshot.params.id, 10);
    this.bidAmount = new FormControl(0, Validators.required);

    this._subscription.add(
      this.lombardService.lombardProductById(productId).subscribe(p => {
        this._product.next(p);
        const bid = p.bids.sort((a, b) => b.id - a.id)[0];
        this.bidAmount.setValue(bid ? (bid.money + 5) : (p.startingBid.money + 5))
      })
    );

    this.isUserActive = this.auth.currentUser.pipe(
      rx.map(user => !isNil(user)),
      rx.shareReplay(1)
    )

    this.ownHighestBid = combineLatest([
      this.currentBestPrice(),
      this.auth.currentUser
    ]).pipe(
      rx.map(([item, user]) => user && item ? item.user.id === user.id : false)
    );

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
      this._product.next(p);
      const bid = p.bids.sort((a, b) => b.id - a.id)[0];
      this.bidAmount.setValue(bid ? (bid.money + 5) : (p.startingBid.money + 5));
      this.openSnackBar('Congratulations! You have sucesfully bid an item for ' + bid.money + '$!', 'OK');
    });
  }

  get product(): Observable<LombardProduct> {
    return this._product.asObservable();
  }

  ngOnDestroy() {
    this._subscription.unsubscribe();
  }

  currentBestPrice(): Observable<ItemBid> {
    return this.product.pipe(
      rx.map(p => p.bids.sort((a, b) => b.id - a.id)[0])
    );
  }

  openSnackBar(message: string, action: string) {
    this._snackBar.open(message, action, {
      duration: 3000,
    });
  }
}
