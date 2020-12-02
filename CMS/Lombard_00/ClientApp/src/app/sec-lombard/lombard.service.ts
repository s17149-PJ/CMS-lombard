import { Injectable } from '@angular/core';
import * as moment from 'moment';
import { of, Observable } from 'rxjs';
import { LombardProduct } from './lombard.model';
import * as rx from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LombardService {

  products: LombardProduct[] = [
    {
      name: 'Branzoletka', category: { name: 'Złoto' },
      publishDate: moment().startOf('month').valueOf(), expirationDate: moment().startOf('day').valueOf()
    },
    {
      name: 'Wisiorek', category: { name: 'Srebro' },
      publishDate: moment().startOf('month').valueOf(), expirationDate: moment().startOf('day').valueOf()
    },
    {
      name: 'Naszyjnik', category: { name: 'Złoto' },
      publishDate: moment().startOf('month').valueOf(), expirationDate: moment().startOf('day').valueOf()
    },
    {
      name: 'Pierścień', category: { name: 'Złoto' },
      publishDate: moment().startOf('month').valueOf(), expirationDate: moment().startOf('day').valueOf()
    },
    {
      name: 'Sygnet', category: { name: 'Stal' },
      publishDate: moment().startOf('month').valueOf(), expirationDate: moment().startOf('day').valueOf()
    },
    {
      name: 'Opaska', category: { name: 'Stal' },
      publishDate: moment().startOf('month').valueOf(), expirationDate: moment().startOf('day').valueOf()
    },
  ];

  constructor() { }

  get lombardProducts(): Observable<LombardProduct[]> {
    return of(this.products);
  }
}
