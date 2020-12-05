import { Observable } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LombardService } from '../lombard.service';
import { LombardProduct } from '../lombard.model';
import * as rx from 'rxjs/operators';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-lombard-details',
  templateUrl: './lombard-details.component.html',
  styleUrls: ['./lombard-details.component.css']
})
export class LombardDetailsComponent implements OnInit {

  product: Observable<LombardProduct>;

  bidAmount: FormControl;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private lombardService: LombardService) { }

  ngOnInit() {
    const productId = parseInt(this.route.snapshot.params.id, 10);
    this.product = this.lombardService.lombardProductById(productId);

    this.bidAmount = new FormControl(0, Validators.required);
  }

}
