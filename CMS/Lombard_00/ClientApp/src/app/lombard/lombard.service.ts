import { User } from './../model/auth.model';
import { AuthService } from './../auth/auth.service';
import { Injectable } from '@angular/core';
import * as moment from 'moment';
import { of, Observable, combineLatest } from 'rxjs';
import { Bid, ItemBid, LombardProduct } from './lombard.model';
import * as rx from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { isNil } from 'lodash';

@Injectable({
  providedIn: 'root'
})
export class LombardService {

  constructor(private http: HttpClient, private authService: AuthService) { }

  fetchProducts(): Observable<LombardProduct[]> {
    return this.http
      .post<any>('api/item/list', {
        ...this.authService.currentUserValue
      })
      .pipe(
        rx.map((products) => products),
        rx.shareReplay(1)
      );
  }

  get lombardProducts(): Observable<LombardProduct[]> {
    return this.fetchProducts().pipe(
      rx.map(products => products.filter(p => moment(p.finallizationDateTimeDouble).isAfter(moment.now())))
    );
  }

  get finishedLombardProducts(): Observable<LombardProduct[]> {
    return this.fetchProducts().pipe(
      rx.map(products => products.filter(p => !isNil(p.winningBid) &&
        moment(p.finallizationDateTimeDouble).isBefore(moment.now())))
    );
  }

  lombardProductById(id: number): Observable<LombardProduct> {
    return this.lombardProducts.pipe(
      rx.map(products => products.filter(p => p.id === id)),
      rx.map(products => products[0])
    );
  }

  createNewAuction(product: LombardProduct) {
    return this.http.post('api/item/add', {
      user: {
        success: this.authService.currentUserValue.success,
        id: this.authService.currentUserValue.id,
        nick: this.authService.currentUserValue.nick,
        name: this.authService.currentUserValue.name,
        surname: this.authService.currentUserValue.surname,
        roles: this.authService.currentUserValue.roles,
        token: this.authService.currentUserValue.token
      },
      item: {
        ...product
      }
    });
  }

  createBid(bid: Bid): Observable<LombardProduct> {
    return this.http.post<LombardProduct>('api/bid/create', {
      ...bid
    })
  }

  isCurrentlyBidding(product: LombardProduct, user: User): boolean {
    return product.bids.filter(bid => bid.user.id === user.id).length > 0;
  }

  wonUserProducts(): Observable<LombardProduct[]> {
    return combineLatest([
      this.finishedLombardProducts,
      this.authService.currentUser
    ]).pipe(
      rx.map(([products, user]) => products.filter(product => product.winningBid.user.id === user.id))
    );
  }
}
