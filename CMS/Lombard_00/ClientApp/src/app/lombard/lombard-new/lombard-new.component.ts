import { LombardProduct } from './../lombard.model';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { LombardService } from '../lombard.service';

@Component({
  selector: 'app-lombard-new',
  templateUrl: './lombard-new.component.html',
  styleUrls: ['./lombard-new.component.css']
})
export class LombardNewComponent implements OnInit {

  productForm: FormGroup;

  constructor(private lombardService: LombardService) { }

  ngOnInit() {
    this.productForm = new FormGroup({
      name: new FormControl('', Validators.required),
      category: new FormControl('', Validators.required),
      finallizationDateTime: new FormControl('', Validators.required),
      imageMetaData: new FormControl('', Validators.required),
      startingBid: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required)
    });
  }

  create() {
    this.lombardService.createNewAuction(this.productForm.value).subscribe(r => console.log(r));
  }

}
