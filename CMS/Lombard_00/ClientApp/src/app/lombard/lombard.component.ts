import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs/operators';
import { LombardService } from './lombard.service';

@Component({
  selector: 'app-lombard',
  templateUrl: './lombard.component.html',
  styleUrls: ['./lombard.component.css']
})
export class LombardComponent implements OnInit {



  constructor(public lombard: LombardService) {}

  ngOnInit() {
  }

}
