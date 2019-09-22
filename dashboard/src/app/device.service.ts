import { Injectable } from '@angular/core';
import {
  HttpClient,
  HttpHeaders,
  HttpErrorResponse,
  HttpResponse
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  constructor(protected http: HttpClient) {}

  list(): Observable<DeviceSlimModel[]> {
    return this.http
      .get(`${environment.api}/v1/Device?take=0&skip=0`, httpOptions)
      .pipe(map((res: any) => res as DeviceSlimModel[]));
  }
}

export interface DeviceSlimModel {
  serialNumber: string;
  state: number;
  lastKnownSpeed: number;
  lastKnownLatitue: number;
  lastKnownLongitude: number;
  lastContact: Date;
  model: string;
  name: string;
  type: number;
  id: string;
  createdAt: Date;
  lastModifiedAt: Date;
}
