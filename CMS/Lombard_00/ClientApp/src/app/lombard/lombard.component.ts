import { AuthService } from 'src/app/auth/auth.service';
import { FormControl, FormGroup } from '@angular/forms';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { Observable, of, Subscription, combineLatest, BehaviorSubject } from 'rxjs';
import { FoundResult, ItemBid, LombardProduct, LompardProductCategory, Tag } from './lombard.model';
import { LombardService } from './lombard.service';
import * as rx from 'rxjs/operators';
import * as moment from 'moment';

@Component({
  selector: 'app-lombard',
  templateUrl: './lombard.component.html',
  styleUrls: ['./lombard.component.css']
})
export class LombardComponent implements OnInit {

  lombardProducts: Observable<FoundResult>;
  lombardProductCategories: Observable<LompardProductCategory[]>;
  tags: Tag[] = [];
  tagsSubject = new BehaviorSubject<Tag[]>([]);
  tagForm: FormGroup;

  panelOpenState = false;

  private _subscription = new Subscription();

  constructor(public auth: AuthService, public lombard: LombardService) { }

  ngOnInit() {
    this.tagForm = new FormGroup({
      tag: new FormControl(''),
    });

    this.lombardProducts = this.tagsSubject.pipe(
      rx.switchMap(t => this.lombard.findItems(t).pipe(
        rx.map(item => item)
      )),
      rx.shareReplay(1)
    );

    this.lombardProductCategories = this.lombard.lombardProducts.pipe(
      rx.map(products => products.map(p => p.category))
    );

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

  addTag(): void {
    const t = this.tagForm.value;
    const newTag: Tag = { name: t.tag };
    this.tags.push(newTag);
    this.tagForm.reset();
    this.tagsSubject.next(this.tags);
  }

  remove(tag: Tag): void {
    const index = this.tags.indexOf(tag);
    if (index >= 0) {
      this.tags.splice(index, 1);
    }
    this.tagsSubject.next(this.tags);
  }

  checkIfFound(tag: Tag): Observable<boolean> {
    return this.lombardProducts.pipe(
      rx.map(i => i.foundTags.filter(t => t.name === tag.name).length > 0)
    );
  }
}
