import { Component, OnInit } from '@angular/core';
import { DeviceService, DeviceSlimModel } from '../device.service';
import { DataSource } from '@angular/cdk/table';
import { Observable, Subject } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-device-list',
  templateUrl: './device-list.component.html',
  styleUrls: ['./device-list.component.scss']
})
export class DeviceListComponent implements OnInit {
  counter = 10;
  dataSource = new MatTableDataSource<DeviceSlimModel>();
  displayedColumns = [
    'serialNumber',
    'state',
    'lastKnownSpeed',
    'lastKnownLatitue',
    'lastKnownLongitude',
    'lastContact',
    'model',
    'name',
    'type'
  ];

  constructor(private deviceService: DeviceService) {}

  ngOnInit() {
    this.refresh();
    this.refreshCounter();
  }

  refreshCounter() {
    console.log('counter');
    this.counter -= 1;
    if (this.counter < 1) {
      this.counter = 10;
      this.refresh();
    }
    setTimeout(() => this.refreshCounter(), 1000);
  }

  refresh() {
    this.deviceService.list().subscribe(data => {
      this.dataSource.data = data;
    });
  }
}
