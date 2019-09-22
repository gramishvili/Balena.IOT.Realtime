import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'deviceStatus' })
export class DeviceStatus implements PipeTransform {
  transform(status: number) {
    if (status === 1) {
      return 'Operating normally';
    }
    if (status === 99) {
      return 'Operates abnormally';
    }
    return status;
  }
}
