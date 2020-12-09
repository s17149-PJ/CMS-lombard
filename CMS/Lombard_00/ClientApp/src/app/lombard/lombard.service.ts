import { AuthService } from './../auth/auth.service';
import { Injectable } from '@angular/core';
import * as moment from 'moment';
import { of, Observable } from 'rxjs';
import { LombardProduct } from './lombard.model';
import * as rx from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { User } from '../model/auth.model';

@Injectable({
  providedIn: 'root'
})
export class LombardService {

  products: LombardProduct[] = [
    {
      id: 1, name: 'Branzoletka', category: { name: 'Złoto' },
      publishDate: moment().startOf('month').valueOf(), expirationDate: moment().startOf('day').valueOf(),
      img: 'https://mdbootstrap.com/img/Photos/Lightbox/Thumbnail/img%20(97).jpg',
      description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec efficitur, tellus eget accumsan dignissim, felis neque placerat dui, non blandit orci augue ornare velit. Nam in orci ut ligula consequat feugiat quis quis arcu. Fusce eu laoreet nulla. Aenean eget finibus metus. Sed sit amet ex cursus, consequat lectus in, convallis nibh. Morbi auctor risus eleifend, finibus ipsum vel, pellentesque nisl. Duis facilisis mattis tincidunt. Morbi sit amet feugiat urna. Sed tempor justo odio, vel tempus ante condimentum id.Fusce sem nisl, vehicula quis viverra ullamcorper, vulputate non arcu. Nulla id elit eu dui dapibus venenatis ornare sit amet est. Sed sodales, ipsum vitae pharetra ornare, sem nisi laoreet ex, sit amet gravida metus nisi sit amet magna. Aenean egestas mauris nec eleifend laoreet. Nunc rutrum tellus in est tincidunt placerat. Nulla congue faucibus mollis. Praesent et varius leo, vel congue ex. Pellentesque faucibus diam diam, vitae condimentum purus tristique eu. Proin nec lacus iaculis, molestie nibh in, tempor odio.'
    },
    {
      id: 2, name: 'Wisiorek', category: { name: 'Srebro' },
      publishDate: moment().startOf('month').valueOf(), expirationDate: moment().startOf('day').valueOf(),
      img: 'https://mdbootstrap.com/img/Photos/Lightbox/Thumbnail/img%20(97).jpg',
      description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec efficitur, tellus eget accumsan dignissim, felis neque placerat dui, non blandit orci augue ornare velit. Nam in orci ut ligula consequat feugiat quis quis arcu. Fusce eu laoreet nulla. Aenean eget finibus metus. Sed sit amet ex cursus, consequat lectus in, convallis nibh. Morbi auctor risus eleifend, finibus ipsum vel, pellentesque nisl. Duis facilisis mattis tincidunt. Morbi sit amet feugiat urna. Sed tempor justo odio, vel tempus ante condimentum id.Fusce sem nisl, vehicula quis viverra ullamcorper, vulputate non arcu. Nulla id elit eu dui dapibus venenatis ornare sit amet est. Sed sodales, ipsum vitae pharetra ornare, sem nisi laoreet ex, sit amet gravida metus nisi sit amet magna. Aenean egestas mauris nec eleifend laoreet. Nunc rutrum tellus in est tincidunt placerat. Nulla congue faucibus mollis. Praesent et varius leo, vel congue ex. Pellentesque faucibus diam diam, vitae condimentum purus tristique eu. Proin nec lacus iaculis, molestie nibh in, tempor odio.'
    },
    {
      id: 3, name: 'Naszyjnik', category: { name: 'Złoto' },
      publishDate: moment().startOf('month').valueOf(), expirationDate: moment().startOf('day').valueOf(),
      img: 'https://mdbootstrap.com/img/Photos/Lightbox/Thumbnail/img%20(97).jpg',
      description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec efficitur, tellus eget accumsan dignissim, felis neque placerat dui, non blandit orci augue ornare velit. Nam in orci ut ligula consequat feugiat quis quis arcu. Fusce eu laoreet nulla. Aenean eget finibus metus. Sed sit amet ex cursus, consequat lectus in, convallis nibh. Morbi auctor risus eleifend, finibus ipsum vel, pellentesque nisl. Duis facilisis mattis tincidunt. Morbi sit amet feugiat urna. Sed tempor justo odio, vel tempus ante condimentum id.Fusce sem nisl, vehicula quis viverra ullamcorper, vulputate non arcu. Nulla id elit eu dui dapibus venenatis ornare sit amet est. Sed sodales, ipsum vitae pharetra ornare, sem nisi laoreet ex, sit amet gravida metus nisi sit amet magna. Aenean egestas mauris nec eleifend laoreet. Nunc rutrum tellus in est tincidunt placerat. Nulla congue faucibus mollis. Praesent et varius leo, vel congue ex. Pellentesque faucibus diam diam, vitae condimentum purus tristique eu. Proin nec lacus iaculis, molestie nibh in, tempor odio.'
    },
    {
      id: 4, name: 'Pierścień', category: { name: 'Złoto' },
      publishDate: moment().startOf('month').valueOf(), expirationDate: moment().startOf('day').valueOf(),
      img: 'https://mdbootstrap.com/img/Photos/Lightbox/Thumbnail/img%20(97).jpg',
      description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec efficitur, tellus eget accumsan dignissim, felis neque placerat dui, non blandit orci augue ornare velit. Nam in orci ut ligula consequat feugiat quis quis arcu. Fusce eu laoreet nulla. Aenean eget finibus metus. Sed sit amet ex cursus, consequat lectus in, convallis nibh. Morbi auctor risus eleifend, finibus ipsum vel, pellentesque nisl. Duis facilisis mattis tincidunt. Morbi sit amet feugiat urna. Sed tempor justo odio, vel tempus ante condimentum id.Fusce sem nisl, vehicula quis viverra ullamcorper, vulputate non arcu. Nulla id elit eu dui dapibus venenatis ornare sit amet est. Sed sodales, ipsum vitae pharetra ornare, sem nisi laoreet ex, sit amet gravida metus nisi sit amet magna. Aenean egestas mauris nec eleifend laoreet. Nunc rutrum tellus in est tincidunt placerat. Nulla congue faucibus mollis. Praesent et varius leo, vel congue ex. Pellentesque faucibus diam diam, vitae condimentum purus tristique eu. Proin nec lacus iaculis, molestie nibh in, tempor odio.'
    },
    {
      id: 5, name: 'Sygnet', category: { name: 'Stal' },
      publishDate: moment().startOf('month').valueOf(), expirationDate: moment().startOf('day').valueOf(),
      img: 'https://mdbootstrap.com/img/Photos/Lightbox/Thumbnail/img%20(97).jpg',
      description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec efficitur, tellus eget accumsan dignissim, felis neque placerat dui, non blandit orci augue ornare velit. Nam in orci ut ligula consequat feugiat quis quis arcu. Fusce eu laoreet nulla. Aenean eget finibus metus. Sed sit amet ex cursus, consequat lectus in, convallis nibh. Morbi auctor risus eleifend, finibus ipsum vel, pellentesque nisl. Duis facilisis mattis tincidunt. Morbi sit amet feugiat urna. Sed tempor justo odio, vel tempus ante condimentum id.Fusce sem nisl, vehicula quis viverra ullamcorper, vulputate non arcu. Nulla id elit eu dui dapibus venenatis ornare sit amet est. Sed sodales, ipsum vitae pharetra ornare, sem nisi laoreet ex, sit amet gravida metus nisi sit amet magna. Aenean egestas mauris nec eleifend laoreet. Nunc rutrum tellus in est tincidunt placerat. Nulla congue faucibus mollis. Praesent et varius leo, vel congue ex. Pellentesque faucibus diam diam, vitae condimentum purus tristique eu. Proin nec lacus iaculis, molestie nibh in, tempor odio.'
    },
    {
      id: 6, name: 'Opaska', category: { name: 'Stal' },
      publishDate: moment().startOf('month').valueOf(), expirationDate: moment().startOf('day').valueOf(),
      img: 'https://mdbootstrap.com/img/Photos/Lightbox/Thumbnail/img%20(97).jpg',
      description: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec efficitur, tellus eget accumsan dignissim, felis neque placerat dui, non blandit orci augue ornare velit. Nam in orci ut ligula consequat feugiat quis quis arcu. Fusce eu laoreet nulla. Aenean eget finibus metus. Sed sit amet ex cursus, consequat lectus in, convallis nibh. Morbi auctor risus eleifend, finibus ipsum vel, pellentesque nisl. Duis facilisis mattis tincidunt. Morbi sit amet feugiat urna. Sed tempor justo odio, vel tempus ante condimentum id.Fusce sem nisl, vehicula quis viverra ullamcorper, vulputate non arcu. Nulla id elit eu dui dapibus venenatis ornare sit amet est. Sed sodales, ipsum vitae pharetra ornare, sem nisi laoreet ex, sit amet gravida metus nisi sit amet magna. Aenean egestas mauris nec eleifend laoreet. Nunc rutrum tellus in est tincidunt placerat. Nulla congue faucibus mollis. Praesent et varius leo, vel congue ex. Pellentesque faucibus diam diam, vitae condimentum purus tristique eu. Proin nec lacus iaculis, molestie nibh in, tempor odio.'
    },
  ];

  constructor(private http: HttpClient, private authService: AuthService) { }

  get fetchProducts(): Observable<LombardProduct[]> {
    return this.http
      .post<LombardProduct[]>('api/item/Slist', {
        success: this.authService.currentUserValue.success,
        id: this.authService.currentUserValue.id,
        nick: this.authService.currentUserValue.nick,
        name: this.authService.currentUserValue.name,
        surname: this.authService.currentUserValue.surname,
        roles: this.authService.currentUserValue.roles,
        token: this.authService.currentUserValue.token
      })
      .pipe(rx.map((products) => products));
  }

  get lombardProducts(): Observable<LombardProduct[]> {
    return this.fetchProducts;
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
}
