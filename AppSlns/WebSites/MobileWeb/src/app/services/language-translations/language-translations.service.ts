import { Injectable } from "@angular/core";
import { Observable, Observer, BehaviorSubject } from "rxjs";
import "rxjs/add/observable/of";
import "rxjs/add/operator/map";
import { HttpService } from "../shared-services/http.service";
import { ContentType } from "../../models/common/content-type.model";
import { environment } from "../../../environments/environment";
import {
  LanguageTranslation,
  LanguageTranslationContract,
  LanguageParams
} from "../../models/language-translation/language-translation-contract.model";

@Injectable()
export class LanguageTranslationService {
  private dataSource = new BehaviorSubject<Array<LanguageTranslation>>(
    new Array<LanguageTranslation>()
  );
  FinalTranslatedList = this.dataSource.asObservable();

  constructor(
    public httpService: HttpService,
    public contentType: ContentType
  ) {}
  private lstLanguageTranslatedText = new Array<LanguageTranslation>();
  private languageTranslationContract = new LanguageTranslationContract();
  private languageParams:LanguageParams = new LanguageParams();
  isLanguageSelected:boolean=false;
  public GetLanguageParams(): LanguageParams {
    return this.languageParams;
  }
  public SetLanguageParams(value: LanguageParams) {
    this.languageParams = value;
  }

  setLanguageTranlationFinalList(
    lstWithTranslatedValues: Array<LanguageTranslation>
  ) {
    if (lstWithTranslatedValues != null) {
      var currentData = this.dataSource.value;
      lstWithTranslatedValues.forEach(element => {
        var languageTransObj = currentData.find(c => c.Key == element.Key);
        if (languageTransObj == null) {
          languageTransObj = new LanguageTranslation();
          languageTransObj.Key = element.Key;
          currentData.push(languageTransObj);
        }
        languageTransObj.value = element.value;
      });
      this.dataSource.next(currentData);
    }
  }

  getLanguageTranslationList(languageCode?: string): Observable<any> {
    var contentType = new ContentType();
    this.languageTranslationContract.lstApiResourceContract = this.lstLanguageTranslatedText;
    this.languageTranslationContract.languageCode = languageCode;
    var jsonData = JSON.stringify(this.languageTranslationContract);
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .post(
          environment.baseApiUrl + environment.GetTranslatedList,
          jsonData,
          contentType.jsonType
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  private getRawResult(apiResources: any): any[] {
    return apiResources;
  }

  clearKeysList() {
    this.lstLanguageTranslatedText = new Array<LanguageTranslation>();
  }

  setKeysInLanguageList(lstKeysToGetValue: Array<string>) {
    lstKeysToGetValue.forEach(element => {
      let languageTransObj = new LanguageTranslation();
      languageTransObj.Key = element;
      languageTransObj.value = "";
      this.lstLanguageTranslatedText.push(languageTransObj);
    });
  }
}
