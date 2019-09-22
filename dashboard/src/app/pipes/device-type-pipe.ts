import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'deviceType' })
export class DeviceType implements PipeTransform {
  transform(status: number) {
    if (status === 1) {
      return 'Drone';
    }
    return status;
  }
}
