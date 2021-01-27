import { ItemBid, LombardProduct } from './../../lombard/lombard.model';
import { map } from 'rxjs/operators';
import { Observable, combineLatest } from 'rxjs';
import { LombardService } from './../../lombard/lombard.service';
import { AuthService } from 'src/app/auth/auth.service';
import { Component, OnInit } from '@angular/core';
import * as rx from 'rxjs/operators';
import { LombardComponent } from 'src/app/lombard/lombard.component';

@Component({
  selector: 'app-user-items',
  templateUrl: './user-items.component.html',
  styleUrls: ['./user-items.component.css']
})
export class UserItemsComponent implements OnInit {

  lombardProducts: Observable<LombardProduct[]>;
  wonProducts: Observable<LombardProduct[]>;
  anyAuctions: Observable<boolean>;
  ownProducts: Observable<LombardProduct[]>;

  constructor(private auth: AuthService, private lombard: LombardService) { }

  ngOnInit() {
    this.lombardProducts = combineLatest([
      this.lombard.lombardProducts,
      this.auth.currentUser
    ]).pipe(
      rx.map(([products, user]) => products.filter(p => this.lombard.isCurrentlyBidding(p, user)))
    );

    this.wonProducts = this.lombard.wonUserProducts();

    this.anyAuctions = combineLatest([
      this.lombardProducts,
      this.wonProducts
    ]).pipe(
      rx.map(([currProd, wonProd]) => currProd.length > 0 || wonProd.length > 0),
      rx.shareReplay(1)
    );

    this.ownProducts = this.lombard.myOwnProducts();
  }

  currentlyWinning(product: LombardProduct): Observable<boolean> {
    return this.auth.currentUser.pipe(
      rx.map(user => user.id === product.bids.sort((a, b) => b.id - a.id)[0].user.id)
    );
  }

}
