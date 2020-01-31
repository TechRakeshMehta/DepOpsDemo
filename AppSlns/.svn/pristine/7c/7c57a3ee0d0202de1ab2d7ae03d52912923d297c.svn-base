import { Injectable } from '@angular/core';
import { Observable, Observer } from 'rxjs';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/map';


/*User defined service*/
import { HttpService } from "../shared-services/http.service";
import { ContentType } from '../../models/common/content-type.model'
import { environment } from '../../../environments/environment'
import { UserRegistration } from '../../models/applicant/package-contract.model';
import { LanguageParams } from '../../models/language-translation/language-translation-contract.model';

@Injectable()
export class LookupService {
    constructor(public httpService: HttpService, public contentType: ContentType) {

    }

    private languageParams: LanguageParams = new LanguageParams();

    public GetLanguageParams(): LanguageParams {
        return this.languageParams;
    }

    public SetLanguageParams(value: LanguageParams) {
        this.languageParams = value;
    }

    getCountry(): Observable<any> {
        return Observable.create((observer: Observer<any>) => {
            this.httpService.get(environment.baseApiUrl + environment.GetCountryList)
                .map(this.getRawResult)
                .subscribe((rNArray: any) => {
                    if (rNArray != null && rNArray != undefined) {
                        observer.next(rNArray);
                        observer.complete();

                    }
                }, (err: any) => {
                    console.log("call failed");
                    observer.next(-1);
                    observer.complete();
                });
        });
    }

    getStates(countryId: number): Observable<any> {
        return Observable.create((observer: Observer<any>) => {
            this.httpService.get(environment.baseApiUrl + environment.GetStates + '/' + countryId)
                .map(this.getRawResult)
                .subscribe((rNArray: any) => {
                    if (rNArray != null && rNArray != undefined) {
                        observer.next(rNArray);
                        observer.complete();

                    }
                }, (err: any) => {
                    console.log("call failed");
                    observer.next(-1);
                    observer.complete();
                });
        });
    }
    getZipcodes(stateID: number): Observable<any> {
        return Observable.create((observer: Observer<any>) => {
            this.httpService.get(environment.baseApiUrl + environment.GetZipCodes + '/' + stateID)
                .map(this.getRawResult)
                .subscribe((rNArray: any) => {
                    if (rNArray != null && rNArray != undefined) {
                        observer.next(rNArray);
                        observer.complete();

                    }
                }, (err: any) => {
                    console.log("call failed");
                    observer.next(-1);
                    observer.complete();
                });
        });
    }
    getCities(stateID: number): Observable<any> {
        return Observable.create((observer: Observer<any>) => {
            this.httpService.get(environment.baseApiUrl + environment.GetCities + '/' + stateID)
                .map(this.getRawResult)
                .subscribe((rNArray: any) => {
                    if (rNArray != null && rNArray != undefined) {
                        observer.next(rNArray);
                        observer.complete();

                    }
                }, (err: any) => {
                    console.log("call failed");
                    observer.next(-1);
                    observer.complete();
                });
        });
    }
    getCounty(cityId: number, zipcode: number): Observable<any> {
        return Observable.create((observer: Observer<any>) => {
            this.httpService.get(environment.baseApiUrl + environment.GetCounty + '?cityId=' + cityId + '&zipCode=' + zipcode)
                .map(this.getRawResult)
                .subscribe((rNArray: any) => {
                    if (rNArray != null && rNArray != undefined) {
                        observer.next(rNArray);
                        observer.complete();

                    }
                }, (err: any) => {
                    console.log("call failed");
                    observer.next(-1);
                    observer.complete();
                });
        });
    }

    getGender(langCode:string): Observable<any> {
        return Observable.create((observer: Observer<any>) => {
            this.httpService.get(environment.baseApiUrl + environment.GetGender + '/' + langCode)
                .map(this.getRawResult)
                .subscribe((rNArray: any) => {
                    if (rNArray != null && rNArray != undefined) {
                        observer.next(rNArray);
                        observer.complete();

                    }
                }, (err: any) => {
                    console.log("call failed");
                    observer.next(-1);
                    observer.complete();
                });
        });
    }

    getCustomAttributeOptions(attributeName: string): Observable<any> {
        return Observable.create((observer: Observer<any>) => {
            this.httpService.get(environment.baseApiUrl + environment.GetCustomAttributeOptions + '/' + attributeName)
                .map(this.getRawResult)
                .subscribe((rNArray: any) => {
                    if (rNArray != null && rNArray != undefined) {
                        observer.next(rNArray);
                        observer.complete();
                    }
                }, (err: any) => {
                    console.log("call failed");
                    observer.next(-1);
                    observer.complete();
                });
        });
    }

    GetCascadingAttributeData(attributeGroupID: number, attributeId: number, SearchID?: string): Observable<any> {
        return Observable.create((observer: Observer<any>) => {
            this.httpService.get(environment.baseApiUrl + environment.GetCascadingAttributeData + '/' + attributeGroupID + '/' + attributeId + '/' + encodeURIComponent(SearchID))
                .map(this.getRawResult)
                .subscribe((rNArray: any) => {
                    if (rNArray != null && rNArray != undefined) {
                        observer.next(rNArray);
                        observer.complete();
                    }
                }, (err: any) => {
                    console.log("call failed");
                    observer.next(-1);
                    observer.complete();
                });
        });
    }

    // SubmitUserRegistration(userRegistration : UserRegistration,description: any): Observable<any> {
    //     alert(userRegistration);
    //     return Observable.create((observer: Observer<any>) => {
    //     this.httpService.post(environment.baseApiUrl + environment.SubmitUserRegistration,userRegistration,this.contentType.jsonType)
    //         .map(this.getRawResult)
    //         .subscribe((rNArray: any) => {
    //             if (rNArray != null && rNArray != undefined) {
    //             observer.next(rNArray);
    //                 observer.complete();
    //             }
    //         }, (err: any) => {
    //             console.log("call failed");
    //             observer.next(-1);
    //             observer.complete();
    //         });
    // });
    // }
    private getRawResult(apiResponse: any): any[] {
        return apiResponse;
    }

    getCommLanguages(): Observable<any> {
        return Observable.create((observer: Observer<any>) => {
            this.httpService.get(environment.baseApiUrl + environment.GetCommLanguage)
                .map(this.getRawResult)
                .subscribe((rNArray: any) => {
                    if (rNArray != null && rNArray != undefined) {
                        observer.next(rNArray);
                        observer.complete();

                    }
                }, (err: any) => {
                    console.log("call failed");
                    observer.next(-1);
                    observer.complete();
                });
        });
    }


    isSuffixDropDown(): Promise<any> {
        let promise = new Promise((resolve, reject) => {
            this.httpService.get(environment.baseApiUrl + environment.IsSuffixDropDown)
            .toPromise()
            .then(res => {
              // Success
              resolve(res);
            });
        });
        return promise;
      }
      getSuffixDropDownList(): Promise<any> {
        let promise = new Promise((resolve, reject) => {
            this.httpService.get(environment.baseApiUrl + environment.GetSuffixDropDown)
            .toPromise()
            .then(res => {
              // Success
              resolve(res);
            });
        });
        return promise;
      }
}