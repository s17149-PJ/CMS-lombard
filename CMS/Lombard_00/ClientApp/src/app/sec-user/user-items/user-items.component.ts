import { map } from 'rxjs/operators';
import { Observable, combineLatest } from 'rxjs';
import { LombardService } from './../../lombard/lombard.service';
import { AuthService } from 'src/app/auth/auth.service';
import { Component, OnInit } from '@angular/core';
import { LombardProduct } from 'src/app/lombard/lombard.model';
import * as rx from 'rxjs/operators';

@Component({
  selector: 'app-user-items',
  templateUrl: './user-items.component.html',
  styleUrls: ['./user-items.component.css']
})
export class UserItemsComponent implements OnInit {

  lombardProducts: Observable<LombardProduct[]>;

  constructor(private auth: AuthService, private lombard: LombardService) { }

  ngOnInit() {
    this.lombardProducts = combineLatest([
      this.lombard.lombardProducts,
      this.auth.currentUser
    ]).pipe(
      rx.map(([products, user]) => products.filter(p => this.lombard.isCurrentlyBidding(p, user))),
      rx.tap(x => console.log(x))
    );
  }

}
