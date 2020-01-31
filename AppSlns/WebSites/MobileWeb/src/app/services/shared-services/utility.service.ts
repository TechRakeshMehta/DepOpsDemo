import { Injectable } from "@angular/core";
import { FormBuilder, FormGroup, FormControl } from "@angular/forms";
import { LanguageTranslationService } from "../language-translations/language-translations.service";
import { KeyedCollection } from "../../models/common/dictionary.model";
import { BehaviorSubject } from "rxjs";
import {
  LanguageTranslation,
  LanguageParams
} from "../../models/language-translation/language-translation-contract.model";


export class ErrorModel {
  hasError: boolean = true;
  ErrorMessage: string = "";
}

@Injectable()
export class UtilityService {
  SubscribeLocalList(
    languageTranslationService: LanguageTranslationService,
    lstKeys: string[]

  ) {
    languageTranslationService.clearKeysList();
    languageTranslationService.setKeysInLanguageList(lstKeys);
    this.PopulateLanguageTranslation(languageTranslationService)
    languageTranslationService.FinalTranslatedList.subscribe(lst => {
      if (lst != null && lst.length > 0) {
        lstKeys.forEach(element => {
          var result = lst.find(attr => attr.Key === element);
          if (result) {
            if (document.getElementById(result.Key) != null)
              document.getElementById(result.Key).innerHTML = result.value;
          }
        });
      }
    });
  }

  SubscribeLocalListWithDupNames(
    languageTranslationService: LanguageTranslationService,
    lstKeys: string[]

  ) {
   // languageTranslationService.clearKeysList();
    languageTranslationService.setKeysInLanguageList(lstKeys);
    this.PopulateLanguageTranslation(languageTranslationService)
    languageTranslationService.FinalTranslatedList.subscribe(lst => {
      if (lst != null && lst.length > 0) {
        lstKeys.forEach(element => {
          var result = lst.find(attr => attr.Key === element);
          if (result) {
            var count = document.getElementsByName(result.Key).length;
            while (count > 0)

              if (document.getElementsByName(result.Key)[count - 1] != null) {
                document.getElementsByName(result.Key)[count - 1].innerHTML = result.value;
                count--;
              }
          }
        });
      }
    });
  }

  SubscribeValidationMessages(
    languageTranslationService: LanguageTranslationService,
    lstKeysObservable: BehaviorSubject<KeyedCollection<string>>
  ) {
    var lstKeys = lstKeysObservable.value;
    var keys = lstKeys.Keys();
    //languageTranslationService.clearKeysList();
    languageTranslationService.setKeysInLanguageList(keys);
    this.PopulateLanguageTranslation(languageTranslationService)
    languageTranslationService.FinalTranslatedList.subscribe(lst => {
      if (lst != null && lst.length > 0) {
        keys.forEach(element => {
          var result = lst.find(attr => attr.Key === element);
          if (result) {
            if (lstKeys.ContainsKey(result.Key))
              lstKeys[result.Key] = result.value;
          }
        });
      }
      lstKeysObservable.next(lstKeys);
    });
  }

  PopulateLanguageTranslation(languageTranslationService: LanguageTranslationService) {
    var languageParams: LanguageParams = languageTranslationService.GetLanguageParams();
    languageTranslationService
      .getLanguageTranslationList(languageParams.SelectedLangCode)
      .subscribe(values => {
        if (values != undefined) {
          let lstWithTranslatedValues = new Array<LanguageTranslation>();
          for (var i = 0; i < values.length; i++) {
            let LanguageContract = new LanguageTranslation();
            LanguageContract.Key = values[i].Key;
            LanguageContract.value = values[i].value;
            lstWithTranslatedValues.push(LanguageContract);
          }
          languageTranslationService.setLanguageTranlationFinalList(
            lstWithTranslatedValues
          );
        }
      });
  }

  PopulateValidationCollectionFromKeys(
    lstKeys: Array<string>,
    keyValueCollection: KeyedCollection<string>
  ) {
    lstKeys.forEach(element => {
      keyValueCollection.Add(element, "");
    });
  }
  ErrorMessage(form: any, field: any, e: Array<any>): any {
    var count = e.length;
    const control = form.get(field);
    while (count > 0) {
      count--;
      var hasError = control.hasError(e[count].type);
      if (hasError && control.touched) {
        return e[count].message;
      }
    }
    return "";
  }
  errorContract = new ErrorModel();
  isError(form: any, field: any, e: Array<any>): any {
    var count = e.length;
    const control = form.get(field);
    while (count > 0) {
      count--;
      var hasError = control.hasError(e[count].type);
      if (hasError && control.touched) {
        return true;
      }
    }
    return false;
  }
  contact_validation_messages = {
    primaryEmail: [
      { type: "required", message: "Email is required" },
      { type: "pattern", message: "Enter a valid email" }
    ]
  };
  isFieldValid(form: FormGroup, field: string): boolean {
    const control = form.get(field);
    return !control.valid && control.touched && !control.disabled;
  }

  validateAllFormFields(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field);
      if (control instanceof FormControl) {
        control.markAsTouched({ onlySelf: true });
      } else if (control instanceof FormGroup) {
        this.validateAllFormFields(control);
      }
    });
  }
  displayFieldCss(formGroup: FormGroup, field: string) {
    return {
      "is-invalid": this.isFieldValid(formGroup, field),
      "has-feedback": this.isFieldValid(formGroup, field)
    };
  }

  public CalenderObjectToDateString(obj:any){    
      return obj.date.year + '-' + obj.date.month + '-' + obj.date.day;
  }

  public DateStringToCalenderObject(str:string){
    str = str.replace('-','/').replace('T',' ').replace('-','/');    
    let date = new Date(str);
    return {
      year: date.getFullYear(),
      month: date.getMonth() + 1,
      day: date.getDate()};    
  }
}
