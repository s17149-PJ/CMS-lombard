import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { Observable, of, Subscription } from 'rxjs';
import { ItemBid, LombardProduct, LompardProductCategory, Tag } from './lombard.model';
import { LombardService } from './lombard.service';
import * as rx from 'rxjs/operators';
import * as moment from 'moment';
import { map } from 'lodash';

@Component({
  selector: 'app-lombard',
  templateUrl: './lombard.component.html',
  styleUrls: ['./lombard.component.css']
})
export class LombardComponent implements OnInit {

  // @ViewChild(MatSort, null) sort: MatSort;

  // displayedColumns: string[] = ['name', 'category', 'publishDate', 'expirationDate'];
  // // _products: Observable<LombardProduct[]>;
  // // products = new MatTableDataSource(null);

  lombardProducts: Observable<LombardProduct[]>;
  lombardProductCategories: Observable<LompardProductCategory[]>;
  tags: Observable<Tag[]>;

  panelOpenState = false;

  private _subscription = new Subscription();

  constructor(public lombard: LombardService) { }

  ngOnInit() {
    this.lombardProducts = this.lombard.lombardProducts.pipe(
      rx.map(items => items)
    );

    this.lombardProductCategories = this.lombard.lombardProducts.pipe(
      rx.map(products => products.map(p => p.category))
    );
    // this.lombard.lombardProducts.pipe(
    //   rx.map(products => products)
    // ).subscribe(p => {
    //   this.products.data = p;
    //   this.products.sort = this.sort;
    // });
  }

  getDate(date: string): string {
    console.log(date);
    const d = moment(date).format('DD/MM/YYYY').valueOf();
    console.log(d);
    return d;
  }

  shortenDescription(text: string): string {
    return text.substring(0, 50) + '...';
  }

  currentBestPrice(product: LombardProduct): ItemBid {
    return product.bids.sort((a, b) => b.id - a.id)[0];
  }

  money(product: LombardProduct): Observable<number> {
    const price = this.currentBestPrice(product);
    return of(price ? price.money : 0);
  }
}
