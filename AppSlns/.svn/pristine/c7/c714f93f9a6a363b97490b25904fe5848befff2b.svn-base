import { Component, OnInit, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup, FormControl } from "@angular/forms";
import { Router } from "@angular/router";
import { LanguageTranslationService } from "../../services/language-translations/language-translations.service";
import { AppConsts } from "../../../environments/constants/appConstants";
import {
  LanguageTranslation,
  LanguageParams
} from "../../models/language-translation/language-translation-contract.model";
import { CommonService } from "../../services/shared-services/common.service";
import { StorageService } from "../../services/shared-services/storage.service";
import { lang } from "moment";

@Component({
  selector: "language-translation",
  templateUrl: "languageTranslation.component.html"
})
export class LanguageTranslationComponent implements OnInit {
  languageId: string;
  languageName: string;
  languageCode: string;
  selectedLangCode: string;
  languageParams: LanguageParams;
  LocalStorageLanguageCode: string
  private lstLanguageTranslated: Array<LanguageTranslation> = new Array<
    LanguageTranslation
    >();
  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private languageTranslationService: LanguageTranslationService,
    private commonService: CommonService,
    private storageService: StorageService,

  ) { }
  ngOnInit() {
    this.languageParams = this.languageTranslationService.GetLanguageParams();
    this.languageName = this.languageParams.LangName;
    this.languageCode = this.languageParams.LangCode;
    this.LocalStorageLanguageCode = this.storageService.GetItem("langCode")
    if (this.LocalStorageLanguageCode != undefined && this.LocalStorageLanguageCode != "") {

      if (this.LocalStorageLanguageCode == AppConsts.SpanishCode) {
        this.languageParams.SelectedLangCode = this.LocalStorageLanguageCode
        this.languageParams.LangName = this.languageName =
          AppConsts.EnglishLangName;
        this.languageParams.LangCode = this.languageCode = AppConsts.EnglishCode;
        this.commonService.SetSpanish(true)
      }
      else {
        this.languageParams.SelectedLangCode = this.LocalStorageLanguageCode;
        this.languageParams.LangName = this.languageName =
          AppConsts.SpanishLangName;
        this.languageParams.LangCode = this.languageCode = AppConsts.SpanishCode;
        this.commonService.SetSpanish(false)
      }
    }

    this.getTranslatedList(this.languageParams.SelectedLangCode);

  }
  ngOnDestroy() {
    this.storageService.SetItem("langCode", "");
  }

  public setCurrentCulture(langCode: string) {
    if (langCode == AppConsts.SpanishCode) {

      this.languageParams.LangName = this.languageName =
        AppConsts.SpanishLangName;
      this.languageParams.LangCode = this.languageCode = AppConsts.SpanishCode;
      this.languageParams.SelectedLangCode = AppConsts.EnglishCode;
      //  this.languageTranslationService.SetLanguageParams(this.languageParams);
    }
    else {

      this.languageParams.LangName = this.languageName =
        AppConsts.EnglishLangName;
      this.languageParams.LangCode = this.languageCode = AppConsts.EnglishCode;
      this.languageParams.SelectedLangCode = AppConsts.SpanishCode;
      //this.languageTranslationService.SetLanguageParams(this.languageParams);
    }
  }


  LanguageClick() {

    this.languageTranslationService.isLanguageSelected = true;
    this.commonService.ResetTimer(true);
    if (this.languageCode != null) {
      if (this.languageCode === AppConsts.EnglishCode) {
        this.commonService.SetSpanish(false);
        this.setCurrentCulture(AppConsts.SpanishCode);
      } else {
        this.commonService.SetSpanish(true);
        this.setCurrentCulture(AppConsts.EnglishCode);
      }
      this.getTranslatedList(this.languageParams.SelectedLangCode);
    }
    var navigationUrl = this.router.url;
    if (navigationUrl == "/createOrder/schedulecalender") {
      this.router.navigate(["Dummy"]);
    }
    if (navigationUrl == "/orderDetail/calenderReschedule") {
      this.router.navigate(["Dummy"]);
    }
  }
  public getTranslatedList(btnlanguageCode: string) {
    this.languageTranslationService
      .getLanguageTranslationList(btnlanguageCode)
      .subscribe(values => {
        if (values != undefined) {
          let lstWithTranslatedValues = new Array<LanguageTranslation>();
          for (var i = 0; i < values.length; i++) {
            let LanguageContract = new LanguageTranslation();
            LanguageContract.Key = values[i].Key;
            LanguageContract.value = values[i].value;
            lstWithTranslatedValues.push(LanguageContract);
          }
          this.languageTranslationService.setLanguageTranlationFinalList(
            lstWithTranslatedValues
          );
        }
      });
  }
  public clearKeysList() {
    this.languageTranslationService.clearKeysList();
  }
  public setKeysInLanguageList(lstKeysToGetValue: Array<string>) {
    this.languageTranslationService.setKeysInLanguageList(lstKeysToGetValue);
  }
}
