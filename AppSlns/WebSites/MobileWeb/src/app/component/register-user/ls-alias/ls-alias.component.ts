import { Component, OnInit } from "@angular/core";
import { AliasComponent } from "../../register-user/alias.component";
import { RegisterUserFormService } from "../../../services/register-user/register-user-form.service";
import { PersonAliasContract } from "../../../models/user/user.model";
import { Router } from "@angular/router";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { KeyedCollection } from "../../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
import { UtilityService } from "../../../services/shared-services/utility.service";

@Component({
  selector: "app-ls-alias",
  templateUrl: "./ls-alias.component.html",
  styleUrls: ["./ls-alias.component.css"]
})
export class LsAliasComponent implements OnInit {
  hdnAddMoreAliasbtn: boolean = true;
  alias: PersonAliasContract;
  IsAliasCheckboxDisable: boolean = false;
  IsAliasCheckboxChecked: boolean = false;
  lstAlias: Array<PersonAliasContract> = [];
  IsNextButtonDisabled: boolean = false;
  constructor(
    private registerUserFormService: RegisterUserFormService,
    private router: Router,
    private utilityService: UtilityService,
    private languageTranslationService: LanguageTranslationService
  ) { }

  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );
  ValidationMsgsObservable = this.dataSource.asObservable();
  setLocalization() {
    var lstKeys = ["CRTACCOUNT", "ISALIASNAME", "PREVIOUS", "NEXT", "ADDMORE",
      "CNFMCNLREGISTRATION", "CNCLREGISTRATION", "LSTITMY", "LSTITMN", "CNCL"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
    var lstKeysDuplicates = ["CNCL"];
    this.utilityService.SubscribeLocalListWithDupNames(
      this.languageTranslationService,
      lstKeysDuplicates
    );
    var lstValidationKeys = [""];
    this.utilityService.PopulateValidationCollectionFromKeys(lstValidationKeys, this.validationMessages);

    this.dataSource.next(this.validationMessages);
    this.utilityService.SubscribeValidationMessages(
      this.languageTranslationService,
      this.dataSource
    );
    this.ValidationMsgsObservable.subscribe(result => {
      this.initializeValidationMessages(result);
    });
  }

  initializeValidationMessages(validationMsgs: KeyedCollection<string>) {
  }

  ngOnInit() {
    var userInfo = this.registerUserFormService.getUserInfo();
    if (userInfo.IsPersonalInfoAvail == false) {
      this.router.navigate([""]);
    }

    this.setLocalization();
    this.lstAlias = new Array<PersonAliasContract>();
    this.IsAliasCheckboxChecked = this.registerUserFormService.isAliasInfoAvailable;
    this.lstAlias = this.registerUserFormService.getAliasInfo();
    if (this.lstAlias.length == 0) {
      this.IsAliasCheckboxChecked = false;
      this.registerUserFormService.isAliasInfoAvailable = false;
    }
    if (this.lstAlias.length > 0) {
      this.IsAliasCheckboxChecked = true;
      this.IsAliasCheckboxDisable = true;
      this.registerUserFormService.isAliasInfoAvailable = true;
      this.IsNextButtonDisabled = false;
    }
    if (this.lstAlias.length < 5 && this.lstAlias.length > 0) {
      this.hdnAddMoreAliasbtn = false;
    }

  }

  onShowHideAddMoreButton(hide: boolean) {
    if (this.lstAlias.length > 0) {
      this.IsAliasCheckboxChecked = true;
      this.IsAliasCheckboxDisable = true;
    }
    else {
      this.IsAliasCheckboxChecked = false;
      this.IsAliasCheckboxDisable = false;
      this.IsNextButtonDisabled = false;
    }
    if (this.lstAlias.length > 0 && this.lstAlias.length < 5) {
      this.hdnAddMoreAliasbtn = false;
    }
    else {
      this.hdnAddMoreAliasbtn = true;
    }
    if (hide == true)
      this.hdnAddMoreAliasbtn = true;
  }

  onNext(IsNextButtonDisabled: boolean) {
    this.IsNextButtonDisabled = IsNextButtonDisabled;
  }

  AddAlias(e: any) {
    if (e.target.checked) {
      this.registerUserFormService.isAliasInfoAvailable = true;
      this.alias = new PersonAliasContract();
      this.alias.IdForIndex = this.lstAlias.length + 1;
      this.alias.IsSaveButtonHidden = false;
      this.alias.IsEditbtnHidden = true;
      this.alias.IsDeletebtnHidden = true;
      this.alias.IsCancelbtnHidden = false;
      this.alias.IsAddMoreButtonClicked = true;
      this.alias.IsAliasReadonly = false;
      this.registerUserFormService.setAliasInfo(this.alias);
      this.IsNextButtonDisabled = true;
    } else {
      this.registerUserFormService.isAliasInfoAvailable = false;
      this.lstAlias.pop();
      this.hdnAddMoreAliasbtn = true;
      this.IsNextButtonDisabled = false;
    }
  }

  AddMoreAlias() {
    if (this.lstAlias.length <= 5) {
      this.alias = new PersonAliasContract();
      this.alias.IdForIndex = this.GetMaxID() + 1;
      this.alias.IsSaveButtonHidden = false;
      this.alias.IsEditbtnHidden = true;
      this.alias.IsDeletebtnHidden = true;
      this.alias.IsCancelbtnHidden = false;
      this.alias.IsAddMoreButtonClicked = true;
      this.alias.IsAliasReadonly = false;
      this.registerUserFormService.setAliasInfo(this.alias);
      this.IsNextButtonDisabled = true;
    }
    if (this.lstAlias.length < 5 || this.lstAlias.length > 0) {
      this.hdnAddMoreAliasbtn = true;
    }
  }
  onSubmit() {
    var userInfo = this.registerUserFormService.getUserInfo();
    userInfo.IsAliasInfoAvail = true;
    this.registerUserFormService.setUserInfo(userInfo);

    this.router.navigate(["/registerUser/address"]);
  }
  onBack() {
    var userInfo = this.registerUserFormService.getUserInfo();
    userInfo.IsPersonalInfoAvail = false;
    this.registerUserFormService.setUserInfo(userInfo);

    var alias = new PersonAliasContract();
    alias = this.lstAlias.filter(x => x.IsSaveButtonHidden == false)[0];
    if (alias != undefined) {
      this.lstAlias.pop();
    }
    this.lstAlias.filter(x => x.IsAliasReadonly = true);
    this.router.navigate(["/registerUser"]);
  }

  GetMaxID() {
    var _maxValue: number = 0;
    this.lstAlias.forEach(x => {
      if (x.IdForIndex > _maxValue) {
        _maxValue = x.IdForIndex
      }
    });
    return _maxValue;
  }

}
