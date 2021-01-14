import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AdminStatistics } from '../../admin.model';
import { AdminService } from '../../admin.service';

@Component({
  selector: 'app-stats-card',
  templateUrl: './stats-card.component.html',
  styleUrls: ['./stats-card.component.scss']
})
export class StatsCardComponent implements OnInit {

  stats: Observable<AdminStatistics>;

  constructor(private adminService: AdminService) {
  }

  ngOnInit() {
    this.stats = this.adminService.fetchStatistics;
  }


  calculateMaximum(val: number) {
    if (val < 10) {
      return 10;
    } else if (val < 100) {
      return 100;
    } else if (val < 1000) {
      return 1000;
    } else {
      return 10000;
    }
  }

}
