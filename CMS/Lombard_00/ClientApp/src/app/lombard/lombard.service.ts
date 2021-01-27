import { User } from './../model/auth.model';
import { AuthService } from './../auth/auth.service';
import { Injectable } from '@angular/core';
import * as moment from 'moment';
import { of, Observable, combineLatest } from 'rxjs';
import { Bid, FoundResult, ItemBid, LombardProduct, Tag } from './lombard.model';
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
        ...this.authService.currentUserValue
      },
      item: {
        ...product
      }
    });
  }

  updateAuction(product: LombardProduct) {
    return this.http.post('api/item/edit', {
      user: {
        ...this.authService.currentUserValue
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

  findItems(tags: Tag[], sortBy: number): Observable<FoundResult> {
    const stringTags = tags.map(tag => tag.name);
    return this.http.post<FoundResult>('api/item/find', {
      user: {
        ...this.authService.currentUserValue
      },
      tags: stringTags,
      sortBy: Number(sortBy)
    });
  }

  myOwnProducts(): Observable<LombardProduct[]> {
    return combineLatest([
      this.authService.currentUser,
      this.lombardProducts
    ]).pipe(
      rx.map(([user, products]) => products.filter(p => p.startingBid.user.id === user.id))
    );
  }
}
