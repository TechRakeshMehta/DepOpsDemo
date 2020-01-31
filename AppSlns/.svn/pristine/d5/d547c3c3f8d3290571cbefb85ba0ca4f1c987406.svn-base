import { Component, OnInit } from "@angular/core";
import { AliasInfoComponent } from "../../step3/aliasInfo.component";
import { OrderFlowService } from "../../../../services/order-flow/order-flow.service";
import { UserInfo, PersonAliasContract } from "../../../../models/user/user.model";
import { Router } from "@angular/router";
import { OrderInfo } from "../../../../models/order-flow/order.model";
import { LanguageTranslationService } from "../../../../services/language-translations/language-translations.service";
import { UtilityService } from "../../../../services/shared-services/utility.service";
import { KeyedCollection } from "../../../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
import { basename } from "path";

@Component({
  selector: "app-ls-aliasInfo",
  templateUrl: "./ls-aliasInfo.component.html",
  styleUrls: ["./ls-aliasInfo.component.css"]
})
export class LsAliasInfoComponent implements OnInit {
  hdnAddMoreAliasbtn: boolean = true;
  alias: PersonAliasContract;
  IsAliasInfoCheckboxDisable: boolean = false;
  IsAliasInfoCheckboxChecked: boolean = false;
  IsNextButtonDisabled: boolean = false;
  IsLegalNameChange: boolean=false;
  IsOneAliasRequired = false;
  OneAliasRequiredMsg: string="";
  IsEditProfile:boolean;
  constructor(
    private orderFlowService: OrderFlowService,
    private router: Router,
    private languageTranslationService: LanguageTranslationService,
    private utilityService: UtilityService
  ) { }
  userInfo: UserInfo;
  orderInfo: OrderInfo;
  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
);
  ValidationMsgsObservable = this.dataSource.asObservable();
  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
  ngOnInit() {
    this.IsEditProfile=this.orderFlowService.IsEditProfile;
    this.orderInfo = this.orderFlowService.getOrderInfo(); //Only for current step
    this.userInfo = this.orderFlowService.getOrganizationUserInfoDetails();
    this.IsLegalNameChange = this.orderInfo.IsLegalNameChange == "true" ? true : false;
    if (this.userInfo.PersonAliasList.length > 0) {
      this.IsAliasInfoCheckboxChecked = true;
      this.IsAliasInfoCheckboxDisable = true;
      this.orderFlowService.isAliasInfoAvailable = true;
      this.IsNextButtonDisabled = false;
    }
    if (this.userInfo.PersonAliasList.length < 5 && this.userInfo.PersonAliasList.length > 0) {
      this.hdnAddMoreAliasbtn = false;
    }
    var lstKeys = ["PERSONALINFO","ISALIASNAME","ADDMORE","PREVIOUS","NEXT","CREATODR","STEP","UPDATE"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );

    var lstValidaionKeys = ["ONEALIASREQUD"];
        this.utilityService.PopulateValidationCollectionFromKeys(lstValidaionKeys, this.validationMessages);

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
    this.OneAliasRequiredMsg = validationMsgs["ONEALIASREQUD"];  
}

  onShowHideAddMoreButton(hide: boolean) {
    if (this.userInfo.PersonAliasList.length > 0) {
      this.IsAliasInfoCheckboxChecked = true;
      this.IsAliasInfoCheckboxDisable = true;
    }
    else {
      this.IsAliasInfoCheckboxChecked = false;
      this.IsAliasInfoCheckboxDisable = false;
      this.IsNextButtonDisabled = false;
    }

    if (this.userInfo.PersonAliasList.length > 0 && this.userInfo.PersonAliasList.length < 5) {
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

  AddAliasInfo(e: any) {
    this.orderFlowService.getOrganizationUserInfoDetails();
    if (e.target.checked) {
      this.orderFlowService.isAliasInfoAvailable = true;
      this.alias = new PersonAliasContract();
      this.alias.IdForIndex = this.userInfo.PersonAliasList.length + 1;
      this.alias.IsSaveButtonHidden = false;
      this.alias.IsEditbtnHidden = true;
      this.alias.IsDeletebtnHidden = true;
      this.alias.IsCancelbtnHidden = false;
      this.alias.IsAddMoreButtonClicked = true;
      this.alias.IsAliasReadonly = false;
      this.userInfo.PersonAliasList.push(this.alias);
      this.IsNextButtonDisabled = true;
    } else {
      this.orderFlowService.isAliasInfoAvailable = false;
      this.userInfo.PersonAliasList = [];
      this.hdnAddMoreAliasbtn = true;
      this.IsNextButtonDisabled = false;
    }
  }

  AddMoreAlias() {
    if (this.userInfo.PersonAliasList.length <= 5) {
      this.alias = new PersonAliasContract();
      this.alias.IdForIndex = this.GetMaxID() + 1;
      this.alias.IsSaveButtonHidden = false;
      this.alias.IsEditbtnHidden = true;
      this.alias.IsDeletebtnHidden = true;
      this.alias.IsCancelbtnHidden = false;
      this.alias.IsAddMoreButtonClicked = true;
      this.alias.IsAliasReadonly = false;
      this.userInfo.PersonAliasList.push(this.alias);
      this.IsNextButtonDisabled = true;
    }
    if (this.userInfo.PersonAliasList.length < 5 || this.userInfo.PersonAliasList.length > 0) {
      this.hdnAddMoreAliasbtn = true;
    }
  }
  onSubmit() {
    if(this.IsLegalNameChange)
    {
      if(this.userInfo.PersonAliasList.length> 0)
      {
        this.OneAliasRequiredMsg ="";
        this.router.navigate(["/createOrder/addressInfo"]);
      }
      else
      {
        this.IsOneAliasRequired=true;
        return false;
      }
    }
    else
    {
      this.router.navigate(["/createOrder/addressInfo"]);
    }
   
  }
  onBack() {
    this.orderFlowService.IsDobAvailable = true;
    var alias = new PersonAliasContract();
    alias = this.userInfo.PersonAliasList.filter(x => x.IsSaveButtonHidden == false)[0];
    if (alias != undefined) {
      this.userInfo.PersonAliasList.pop();
    }
    this.userInfo.PersonAliasList.filter(x => x.IsAliasReadonly = true);
    this.router.navigate(["/createOrder/personalInfo"]);
  }

  GetMaxID() {    
    var _maxValue: number = 0;
    this.userInfo.PersonAliasList.forEach(x => {
      if (x.ID > _maxValue) {
        _maxValue = x.ID
      }
      if (x.IdForIndex > _maxValue) {
        _maxValue = x.IdForIndex
      }
    });
    return _maxValue;
  }
}
