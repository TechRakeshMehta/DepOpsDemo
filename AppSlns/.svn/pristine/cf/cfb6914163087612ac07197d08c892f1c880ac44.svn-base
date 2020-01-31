import { Injectable } from '@angular/core';
import { MapsAPILoader } from '@agm/core';
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { of } from 'rxjs/observable/of';
import { filter, catchError, tap, map, switchMap } from 'rxjs/operators';
import { fromPromise } from 'rxjs/observable/fromPromise';

declare var google: any;

@Injectable()
export class GeocodeService {
  error: string;
  private geocoder: any;
  lng: string;
  lat: string;
  constructor(private mapLoader: MapsAPILoader) { }

  private initGeocoder() {
    this.geocoder = new google.maps.Geocoder();
  }

  private waitForMapsToLoad(): Observable<boolean> {
    if (!this.geocoder) {
      return fromPromise(this.mapLoader.load())
        .pipe(
          tap(() => this.initGeocoder()),
          map(() => true)
        );
    }
    return of(true);
  }


  geocodeLocation(location: string): Promise<any> {

    return this.waitForMapsToLoad().pipe(
      // filter(loaded => loaded),
      switchMap(() => {
        return new Promise((resolve, reject) => {
          this.geocoder.geocode({ 'address': location }, (results: any, status: any) => {
            if (status == google.maps.GeocoderStatus.OK) {
              this.lat = results[0].geometry.location.lat();
              this.lng = results[0].geometry.location.lng();
              this.error = "";
              resolve();
            } else {
              console.log('Error - ', results, ' & Status - ', status);
              this.error = status;
              resolve();
            }

          });
        })
      })
    ).toPromise()
  }
}