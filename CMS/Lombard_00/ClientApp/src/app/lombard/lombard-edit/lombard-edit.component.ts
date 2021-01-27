import { ActivatedRoute, Router } from '@angular/router';
import { LombardProduct } from '../lombard.model';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { LombardService } from '../lombard.service';
import * as moment from 'moment';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-lombard-edit',
  templateUrl: './lombard-edit.component.html',
  styleUrls: ['./lombard-edit.component.css']
})
export class LombardEditComponent implements OnInit {

  productForm: FormGroup;

  private _subscription = new Subscription();
  productId: number;

  constructor(
    private route: ActivatedRoute,
    private lombardService: LombardService,
    private router: Router
  ) { }

  ngOnInit() {
    this.productId = parseInt(this.route.snapshot.params.id, 10);

    this.productForm = new FormGroup({
      name: new FormControl('', Validators.required),
      tagsString: new FormControl('', Validators.required),
      finallizationDateTime: new FormControl(moment.now(), Validators.required),
      imageMetaData: new FormControl('', Validators.required),
      // startingBid: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required)
    });

    this._subscription.add(
      this.lombardService.lombardProductById(this.productId).subscribe(p => {
        this.productForm.patchValue(p);
      })
    );
  }

  create() {
    this.lombardService.updateAuction({
      ...this.productForm.value,
      id: this.productId
    }).subscribe(r => this.router.navigate(['lombard']));
  }

}
