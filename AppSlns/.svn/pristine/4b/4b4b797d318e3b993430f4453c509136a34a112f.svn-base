import { Component, OnInit } from "@angular/core";
import { FormBuilder, Validators, FormGroup, FormControl } from "@angular/forms";
import { Router } from "@angular/router";
import { AttributesForCustomForm } from "../../../models/custom-forms/custom-attribute";
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { DISABLED } from "@angular/forms/src/model";
import { mailingInfoAtt } from "../../../models/custom-forms/custom-attribute";
import { UtilityService } from "../../../services/shared-services/utility.service";
import { OrderInfo } from '../../../models/order-flow/order.model';
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { KeyedCollection } from "../../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
import { AppConsts } from "../../../../environments/constants/appConstants";
import { CommonService } from "../../../services/shared-services/common.service";

@Component({
  selector: "app-service-details",
  templateUrl: "./service-details.component.html",
  styleUrls: ["./service-details.component.css"]
})
export class ServiceDetailsComponent implements OnInit {
  orderInfo: OrderInfo;
  serviceInfoGroupID: number;
  lstMailingAttribute: Array<mailingInfoAtt>;
  infoEmployeeDetailsGroupID: number;
  isDCLRequired: boolean;
  IsDCLDisable: boolean = false;
  dclValidationExpression: string = "";
  dclValidationMessage: string;
  customHtml:string="";
  ServiceDetailForm = this.formBuilder.group({
    ReasonFingerprinted: [{ value: "", disabled: true }],
    CBIUniqueID: [{ value: "", disabled: true }],
    AcctAdr: [{ value: "", disabled: true }],
    DaycareLicense: [{ value: "", disabled: true }, Validators.required],
    AcctNam: [{ value: "", disabled: true }],
    AcctCty: [{ value: "", disabled: true }],
    AcctZip: [{ value: "", disabled: true }],
    CRS: [{ value: "", disabled: true }],
    ACCTSTA: [{ value: "", disabled: true }],
    TOTALFEE: [{ value: "", disabled: true }]
  });

  constructor(
    private orderFlowService: OrderFlowService,
    private formBuilder: FormBuilder,
    private router: Router,
    private utilityService: UtilityService,
    private languageTranslationService: LanguageTranslationService,
    private commonService: CommonService
  ) { }

  validation_messages: any;
  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );
  ValidationMsgsObservable = this.dataSource.asObservable();


  ngOnInit() {
    var lstKeys = ["REASONFINGERPRINT", "CBIUNIQUEID", "DAYCARELIC", "SERVICEDETAIL",
      "ACCTNAM","OF", "ACCRADR", "ACCTCTY", "ACCTSTA","TOTALFEE", "ACCTZIP", "FINGERPRINTEDCRS", "PREVIOUS",
      "NEXT", "CREATODR", "STEP", "CNFMCNLORDER", "CNCLORDR", "LSTITMY", "LSTITMN", "CNCL"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
    var lstValidationKeys = ["PLSENTRDAYCARELICENSE", "DAYCARELICENSEMAXDIGVAL"];
    this.utilityService.PopulateValidationCollectionFromKeys(lstValidationKeys, this.validationMessages);
    this.dataSource.next(this.validationMessages);
    this.utilityService.SubscribeValidationMessages(
      this.languageTranslationService,
      this.dataSource
    );
    this.ValidationMsgsObservable.subscribe(result => {
      this.initializeValidationMessages(result);
    });
    this.orderInfo = this.orderFlowService.getOrderInfo();
    if (this.orderFlowService.isOrderlInfoAvailable) {
      this.populateFormControls();
    }
    
  }
  initializeValidationMessages(validationMsgs: KeyedCollection<string>) {
    this.validation_messages = {
      'DaycareLicense': [
        { type: 'required',  message: validationMsgs["PLSENTRDAYCARELICENSE"] },
        { type: 'pattern',  message: validationMsgs["DAYCARELICENSEMAXDIGVAL"] }
      ]
    }
  }
  setCustomFormsLocalization(isSpanishLanguage: boolean) {
    if (isSpanishLanguage) {
      this.customHtml = this.orderInfo.lstCustomAttributeInSpanish.filter(x => x.AttributeGroupId == this.serviceInfoGroupID)[0].CustomHtml;
      if ((this.customHtml != '' || this.customHtml != undefined) && document.getElementById("customHtml") != null ) {
        document.getElementById("customHtml").innerHTML = this.customHtml;
      }
    }
    else {
      this.customHtml = this.orderInfo.lstCustomAttribute.filter(x => x.AttributeGroupId == this.serviceInfoGroupID)[0].CustomHtml;
      if ((this.customHtml != '' || this.customHtml != undefined) && document.getElementById("customHtml") != null) {
        document.getElementById("customHtml").innerHTML = this.customHtml;
      }
    }
  }
  populateFormControls() {

    var formControls = this.ServiceDetailForm.controls;
    this.serviceInfoGroupID = this.orderInfo.lstCustomAttribute.filter(x => x.AttriButeGroupName == "Service Information")[0].AttributeGroupId;
    this.isDCLRequired = this.orderInfo.lstCustomAttribute.find(x => x.Name == "DCL # Req").AttributeDataValue == 'Y' ? true : false;
    this.dclValidationExpression = this.orderInfo.lstCustomAttribute.find(x => x.Name == "DCL").ValidateExpression;
    this.dclValidationMessage = this.orderInfo.lstCustomAttribute.find(x => x.Name == "DCL").ValidationMessage;
    this.populateFormControlValue("ReasonFingerprinted", "Reason Fingerprinted");
    this.populateFormControlValue("CBIUniqueID", "CBIUniqueID");
    this.populateFormControlValue("AcctNam", "AcctNam (Literal)");
    this.populateFormControlValue("AcctAdr", "AcctAdr");
    this.populateFormControlValue("AcctCty", "AcctCty");
    this.populateFormControlValue("AcctZip", "AcctZip");
    this.populateFormControlValue("ACCTSTA", "ACCTSTA");
    this.populateFormControlValue("DaycareLicense", "DCL");
    this.populateFormControlValue("CRS", "Reason Fingerprinted Colorado Revised Statute (C.R.S.)");
    this.populateFormControlValue("TOTALFEE", "Total Fee");
    this.commonService.isLanguageSpanish.subscribe(isSpanishLang => {
      this.setCustomFormsLocalization(isSpanishLang);
    });
    if (this.isDCLRequired) {
      if (this.dclValidationExpression != '') {
        formControls["DaycareLicense"].setValidators([Validators.required, Validators.pattern(this.dclValidationExpression)]);
      }
      else {
        formControls["DaycareLicense"].setValidators(Validators.required);
      }
      this.ServiceDetailForm.get("DaycareLicense").enable();
      this.IsDCLDisable = false;

    }
    else {
      formControls["DaycareLicense"].clearValidators();
      this.IsDCLDisable = true;
    }
  }

  populateFormControlValue(controlName: string, attributeName: string) {
    this.ServiceDetailForm.controls[controlName].setValue(
      this.orderInfo.lstCustomAttribute.filter(
        x => x.AttributeGroupId == this.serviceInfoGroupID &&
          x.Name == attributeName
      )[0].AttributeDataValue
    );
  }

  isFieldValid(field: string) {
    return this.utilityService.isFieldValid(this.ServiceDetailForm, field);
  }
  displayFieldCss(field: string) {
    return this.utilityService.displayFieldCss(this.ServiceDetailForm, field);
  }
  errorMessage(field: string) {
    return this.utilityService.ErrorMessage(this.ServiceDetailForm, field, this.validation_messages[field]);
  }
  onSubmit() {

    if (this.IsDCLDisable) {
      if (this.ServiceDetailForm.disable) {
        this.setAttributeValue();
      }
    }
    else {
      if (this.ServiceDetailForm.valid) {
        this.setAttributeValue();
      } else {
        this.utilityService.validateAllFormFields(this.ServiceDetailForm);
      }
    }

  }
  setAttributeValue() {
    var serviceInfoObject = this.ServiceDetailForm.value;
    this.orderInfo.lstCustomAttribute.filter(
      x => x.AttributeGroupId == this.serviceInfoGroupID && x.Name == "DCL"
    )[0].AttributeDataValue = serviceInfoObject.DaycareLicense;

    this.orderFlowService.isOrderlInfoAvailable = true;
    if (this.orderInfo.lstCustomAttribute.filter(x => x.AttriButeGroupName == "Employer Details"
      || x.AttriButeGroupName == "Business Address"
      || x.AttriButeGroupName == "Facility Address"
      || x.AttriButeGroupName == "Mailing Address"
    ).length > 0) {
      this.infoEmployeeDetailsGroupID = this.orderInfo.lstCustomAttribute.filter(
        x => x.AttriButeGroupName == "Employer Details"
          || x.AttriButeGroupName == "Business Address"
          || x.AttriButeGroupName == "Facility Address"
          || x.AttriButeGroupName == "Mailing Address"
      )[0].AttributeGroupId;
    }

    if (this.infoEmployeeDetailsGroupID != undefined) {
      this.orderFlowService.getMailingInfo(AppConsts.EnglishCode).subscribe((values: any) => {
        this.lstMailingAttribute = values;
        if (
          this.lstMailingAttribute.filter(
            x =>
              x.AttributeGroupID === this.infoEmployeeDetailsGroupID &&
              x.IsAttributeGroupHidden === true
          ).length > 0
        ) {
          this.orderInfo.lstCustomAttribute = this.orderInfo.lstCustomAttribute.filter(x => x.AttributeGroupId != this.infoEmployeeDetailsGroupID);
          this.orderInfo.lstCustomAttributeInSpanish = this.orderInfo.lstCustomAttributeInSpanish.filter(x => x.AttributeGroupId != this.infoEmployeeDetailsGroupID);
          this.orderInfo.isEmpoyerDetailsMendatory = false;
          this.orderFlowService.IncrementStep();
          this.orderFlowService.setOrderInfo(this.orderInfo);
          if (this.orderInfo.selectedOrderType == "1") {
            this.router.navigate(["createOrder/schedulecalender"]);
          }
          else {
            this.router.navigate(["createOrder/OrderReview"]);
          }
        }
        else {

          this.lstMailingAttribute.forEach(x => {
            this.orderFlowService.setmailingAttributeInfo(x);
          });
          this.orderFlowService.getMailingInfo(AppConsts.SpanishCode).subscribe((values: any) => {
             this.orderInfo.lstMailingAttributeSpanish=values; 
             this.orderInfo.isEmpoyerDetailsMendatory = true;
             this.orderFlowService.setOrderInfo(this.orderInfo);
             this.router.navigate(["createOrder/MailingInfo"]);
          });
          
        }
      });
    }
    else {
      this.orderFlowService.IncrementStep();
      this.orderFlowService.setOrderInfo(this.orderInfo);
      if (this.orderInfo.selectedOrderType == "1") {
        this.router.navigate(["createOrder/schedulecalender"]);
      }
      else {
        this.router.navigate(["createOrder/OrderReview"]);
      }
    }
  }
  goBack() {
    this.router.navigate(["createOrder/fingerPrinting"]);
  }
  goBackToDashboard() {
    this.orderFlowService.setOrderInfoToEmpty();
    this.router.navigate(["userDashboard"]);
  }

 

}