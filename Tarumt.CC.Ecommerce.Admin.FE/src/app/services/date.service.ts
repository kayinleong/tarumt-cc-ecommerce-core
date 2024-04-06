import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class DateService {
  humanifyUnixTimestamp(unixTimestamp: string) {
    return new Date(unixTimestamp).toLocaleString('en-us');
  }

  convertToHtmlDateFormat(date: string) {
    var dateParts = date.split('/');
    var month = dateParts[0];
    var day = dateParts[1];
    var year = dateParts[2];

    if (month.length == 1) {
      month = '0' + month;
    }

    var outputDate = year + '-' + month + '-' + day;
    return outputDate;
  }

  convertToServerDateFormat(date: string) {
    var dateParts = date.split('-');
    var year = dateParts[0];
    var month = dateParts[1];
    var day = dateParts[2];
    var outputDate = month + '/' + day + '/' + year;
    return outputDate;
  }
}
