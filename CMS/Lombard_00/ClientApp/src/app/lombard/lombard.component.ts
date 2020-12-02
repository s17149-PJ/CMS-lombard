import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { LombardProduct, LompardProductCategory } from './lombard.model';
import { LombardService } from './lombard.service';
import * as rx from 'rxjs/operators';
import { MatSort, MatTableDataSource } from '@angular/material';

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
}
