import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatPaginator, MatSort } from '@angular/material';
import { AuthService } from 'src/app/auth/auth.service';
import { RoleDefinition, User } from 'src/app/model/auth.model';

@Component({
  selector: 'app-admin-panel-users',
  templateUrl: './admin-panel-users.component.html',
  styleUrls: ['./admin-panel-users.component.css'],
})
export class AdminPanelUsersComponent implements OnInit {
  displayedColumns: string[] = ['role', 'name', 'surname', 'nick'];
  tableData = new MatTableDataSource<User>();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  constructor(private auth: AuthService) {}

  ngOnInit(): void {
    this.auth.fetchUsers.subscribe((data) => {
      this.tableData.data = data;
      this.tableData.paginator = this.paginator;
      this.tableData.sort = this.sort;
    });
  }

  getRoleName(roles: RoleDefinition[]): string {
    return roles.sort((r1, r2) => r1.id - r2.id)[0].name;
  }
}
