import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-lombard-new',
  templateUrl: './lombard-new.component.html',
  styleUrls: ['./lombard-new.component.css']
})
export class LombardNewComponent implements OnInit {

  productForm: FormGroup;

  constructor() { }

  ngOnInit() {
    this.productForm = new FormGroup({
      productName: new FormControl('', Validators.required),
      productCategory: new FormControl('', Validators.required),
      expirationDate: new FormControl('', Validators.required),
      productUrl: new FormControl('', Validators.required),
      startingBid: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required)
    });
  }

  create() {
    console.log(this.productForm.value);
  }

}
