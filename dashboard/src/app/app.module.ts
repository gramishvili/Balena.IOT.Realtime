import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DeviceListComponent } from './device-list/device-list.component';

import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { DeviceService } from './device.service';
import { HttpClientModule } from '@angular/common/http';
import { DeviceStatus } from './pipes/device-state-pipe';
import { DeviceType } from './pipes/device-type-pipe';

@NgModule({
  declarations: [AppComponent, DeviceListComponent, DeviceStatus, DeviceType],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatCardModule,
    MatTableModule,
    MatButtonModule,
    HttpClientModule
  ],
  providers: [DeviceService],
  bootstrap: [AppComponent]
})
export class AppModule {}
