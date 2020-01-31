import { Component, OnInit } from "@angular/core";
import { FormBuilder, Validator, FormGroup, FormControl } from "@angular/forms";
import { Router } from "@angular/router";
import { AttributesForCustomForm } from "../../../models/custom-forms/custom-attribute";
import { mailingInfoAtt } from "../../../models/custom-forms/custom-attribute";
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { LookupService } from "../../../services/shared-services/lookup.service";
import { OrderInfo } from "../../../models/order-flow/order.model";
import { UtilityService } from "../../../services/shared-services/utility.service";
import { Validators } from "@angular/forms";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { KeyedCollection } from "../../../models/common/dictionary.model";
import { Observable, BehaviorSubject, Subscription } from "rxjs";
import { basename } from "path";
import { CommonService } from "../../../services/shared-services/common.service";
import { AppConstant } from "../../../models/common/AppConst";
import { AppConsts } from "../../../../environments/constants/appConstants";

@Component({
  selector: "app-maildetails",
  templateUrl: "./maildetails.component.html",
  styleUrls: ["./maildetails.component.css"]
})
export class MaildetailsComponent implements OnInit {
  IsValidName: boolean = true;
  InvalidNameMsg: string = "";
  orderInfo: OrderInfo;
  groupName: string;
  lstMailingAttribute: Array<mailingInfoAtt>;
  lstMailingAttributeSpanish: Array<mailingInfoAtt>;
  employerDetailsGroupID: number;
  mailInfoStateAttrID: number;
  lstOfStateAttr: Array<any>;
  SelectedState: string;
  zipCodeRegex: string = "^[0-9]{5}$";
  employerStateAbbrCBIID: number;
  employerStateAbbrCBIIDValue: string = '';
  customHtml: string = "";
  mailInfoForm = this.formbuilder.group({
    Name: ["", Validators.required],
    Address: ["", Validators.required],
    City: ["", Validators.required],
    State: ["", Validators.required],
    ZipCode: ["", [Validators.required, Validators.pattern(this.zipCodeRegex)]]
  });
  subscriptionLanguage: Subscription;

  constructor(
    private orderFlowService: OrderFlowService,
    private formbuilder: FormBuilder,
    private router: Router,
    private lookupService: LookupService,
    private utilityService: UtilityService,
    private languageTranslationService: LanguageTranslationService,
    private commonService: CommonService
  ) { }

  mailingdetails_validation_messages: any;
  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );
  ValidationMsgsObservable = this.dataSource.asObservable();

  setLocalization() {
    var lstKeys = [
      "NAME",
      "ADDRESS",
      "STATE",
      "CITY",
      "ZIPCODE",
      "PREVIOUS",
      "NEXT",
      "CREATODR",
      "STEP",
      "CNFMCNLORDER",
      "CNCLORDR",
      "LSTITMY",
      "LSTITMN", "OF",
      "CNCL"
    ];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
    var lstKeysDuplicates = ["PLEASESELECT"];
    this.utilityService.SubscribeLocalListWithDupNames(
      this.languageTranslationService,
      lstKeysDuplicates
    );
    var lstValidationKeys = [
      "PLSENTRNAME",
      "PLSENTRADDRESS",
      "PLSENTRCITY",
      "PLSENTRSTATE",
      "PLSENTRZIPCODE",
      "COMBINATNMAXCHARVALIDATN",
      "ZIPCODEDIGVALDATN"
    ];
    this.utilityService.PopulateValidationCollectionFromKeys(
      lstValidationKeys,
      this.validationMessages
    );

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
    this.mailingdetails_validation_messages = {
      Name: [{ type: "required", message: validationMsgs["PLSENTRNAME"] }],
      Address: [
        { type: "required", message: validationMsgs["PLSENTRADDRESS"] }
      ],
      State: [{ type: "required", message: validationMsgs["PLSENTRSTATE"] }],
      City: [{ type: "required", message: validationMsgs["PLSENTRCITY"] }],
      ZipCode: [
        { type: "required", message: validationMsgs["PLSENTRZIPCODE"] },
        { type: "pattern", message: validationMsgs["ZIPCODEDIGVALDATN"] }
      ]
    };
    this.InvalidNameMsg = validationMsgs["COMBINATNMAXCHARVALIDATN"];
  }


  ngOnInit() {
    this.setLocalization();
    if (this.orderFlowService.isOrderlInfoAvailable) {
      this.orderInfo = this.orderFlowService.getOrderInfo();
      this.lstMailingAttribute = this.orderInfo.lstMailingAttribute;
      this.lstMailingAttributeSpanish = this.orderInfo.lstMailingAttributeSpanish;
      this.employerDetailsGroupID = this.orderInfo.lstCustomAttribute.filter(
        x =>
          x.AttriButeGroupName == "Employer Details" ||
          x.AttriButeGroupName == "Business Address"
      )[0].AttributeGroupId;
      var stateAttr = this.orderInfo.lstCustomAttribute.filter(
        x =>
          x.AttributeGroupId == this.employerDetailsGroupID &&
          x.Name == "Employer State CBI"
      )[0];

      if (stateAttr != undefined) {
        this.mailInfoStateAttrID = stateAttr.AttributeId;
        if (
          stateAttr.AttributeDataValue != undefined &&
          stateAttr.AttributeDataValue != ""
        ) {
          this.SelectedState = stateAttr.AttributeDataValue;
        }

        var stateAbbrAttr = this.orderInfo.lstCustomAttribute.filter(
          x =>
            x.AttributeGroupId == this.employerDetailsGroupID &&
            x.Name == 'Employer State(Abbr) CBI'
        )[0];

        if (stateAbbrAttr != undefined) {
          this.employerStateAbbrCBIID = stateAbbrAttr.AttributeId;
          if (stateAbbrAttr.AttributeDataValue != undefined && stateAbbrAttr.AttributeDataValue != '') {
            this.employerStateAbbrCBIIDValue = stateAbbrAttr.AttributeDataValue;
          }
        }

      }

      this.setCustomFormsLocalization(
        this.languageTranslationService.GetLanguageParams().SelectedLangCode ==
        AppConsts.SpanishCode
      );

      this.subscriptionLanguage = this.commonService.isLanguageSpanish.subscribe(
        isSpanishLang => {
          this.setCustomFormsLocalization(isSpanishLang);
        }
      );

      this.orderInfo.lstCustomAttribute
        .filter(x => x.AttributeGroupId == this.employerDetailsGroupID)
        .forEach(x => (x.SectionTitle = this.lstMailingAttribute.filter(
          x => x.AttributeGroupID === this.employerDetailsGroupID
        )[0].HeaderLabel));
      this.orderInfo.lstCustomAttributeInSpanish
        .filter(x => x.AttributeGroupId == this.employerDetailsGroupID)
        .forEach(x => (x.SectionTitle = this.lstMailingAttributeSpanish.filter(
          x => x.AttributeGroupID === this.employerDetailsGroupID
        )[0].HeaderLabel));

      this.bindCascadingDropdown();
      this.populateFormControls();
    }
  }
  ngOnDestroy() {
    if (this.subscriptionLanguage && !this.subscriptionLanguage.closed) {
      this.subscriptionLanguage.unsubscribe();
    }
  }
  setCustomFormsLocalization(isSpanishLanguage: boolean) {
    if (isSpanishLanguage) {
      if (
        this.lstMailingAttributeSpanish !== undefined &&
        this.lstMailingAttributeSpanish !== null &&
        this.lstMailingAttributeSpanish.length > 0
      ) {
        this.groupName = this.lstMailingAttributeSpanish.filter(
          x => x.AttributeGroupID === this.employerDetailsGroupID
        )[0].HeaderLabel;
      }
      this.customHtml = this.orderInfo.lstCustomAttributeInSpanish.filter(x => x.AttributeGroupId == this.employerDetailsGroupID)[0].CustomHtml;
      if ((this.customHtml != '' || this.customHtml != undefined) && document.getElementById("customHtml") != null) {
        document.getElementById("customHtml").innerHTML = this.customHtml;
      }
    } else {
      if (
        this.lstMailingAttribute !== undefined &&
        this.lstMailingAttribute !== null &&
        this.lstMailingAttribute.length > 0
      ) {
        this.groupName = this.lstMailingAttribute.filter(
          x => x.AttributeGroupID === this.employerDetailsGroupID
        )[0].HeaderLabel;
      }

      this.customHtml = this.orderInfo.lstCustomAttribute.filter(x => x.AttributeGroupId == this.employerDetailsGroupID)[0].CustomHtml;
      if ((this.customHtml != '' || this.customHtml != undefined) && document.getElementById("customHtml") != null) {
        document.getElementById("customHtml").innerHTML = this.customHtml;
      }
    }
  }

  bindCascadingDropdown() {
    this.lookupService
      .GetCascadingAttributeData(
        this.employerDetailsGroupID,
        this.mailInfoStateAttrID
      )
      .subscribe(values => {
        this.lstOfStateAttr = values;
      });
  }

  populateFormControlValue(controlName: string, attributeName: string) {
    this.mailInfoForm.controls[controlName].setValue(
      this.orderInfo.lstCustomAttribute.filter(
        x =>
          x.AttributeGroupId == this.employerDetailsGroupID &&
          x.Name == attributeName
      )[0].AttributeDataValue
    );
  }

  populateFormControls() {
    var formControls = this.mailInfoForm.controls;
    this.populateFormControlValue("Name", "Employer Name CBI");
    this.populateFormControlValue("Address", "Employer Address CBI");
    this.populateFormControlValue("City", "Employer City CBI");
    this.populateFormControlValue("ZipCode", "Employer Zip Code CBI");
  }

  isError(field: string) {
    return this.utilityService.isError(
      this.mailInfoForm,
      field,
      this.mailingdetails_validation_messages[field]
    );
  }
  errorMessage(field: string) {
    return this.utilityService.ErrorMessage(
      this.mailInfoForm,
      field,
      this.mailingdetails_validation_messages[field]
    );
  }

  isFieldValid(field: string) {
    return this.utilityService.isFieldValid(this.mailInfoForm, field);
  }

  displayFieldCss(field: string) {
    return this.utilityService.displayFieldCss(this.mailInfoForm, field);
  }

  validateName() {
    var mailInfoFormObject = this.mailInfoForm.value;
    this.IsValidName = true;
    var totalLength =
      +mailInfoFormObject.Name.length +
      mailInfoFormObject.Address.length +
      mailInfoFormObject.City.length;
    if (totalLength > 109) {
      this.IsValidName = false;

      return false;
    } else {
      this.IsValidName = true;

      return true;
    }
  }

  onStateSelected(selectedState: string) {
    this.lookupService.GetCascadingAttributeData(this.employerDetailsGroupID, this.employerStateAbbrCBIID, selectedState).subscribe(values => {
      if (values != undefined && values.length > 0) {
        this.employerStateAbbrCBIIDValue = values[0];
      }
    });
  }

  onSubmit() {
    if (this.mailInfoForm.valid && this.validateName()) {
      this.setAttributesValue();
    } else {
      this.utilityService.validateAllFormFields(this.mailInfoForm);
    }
  }

  populateAttributeValue(attributeName: string, attributeValue: string) {
    this.orderInfo.lstCustomAttribute.filter(
      x =>
        x.AttributeGroupId == this.employerDetailsGroupID &&
        x.Name == attributeName
    )[0].AttributeDataValue = attributeValue;
  }

  setAttributesValue() {
    var mailInfoObject = this.mailInfoForm.value;
    this.populateAttributeValue("Employer Name CBI", mailInfoObject.Name);
    this.populateAttributeValue("Employer Address CBI", mailInfoObject.Address);
    this.populateAttributeValue("Employer City CBI", mailInfoObject.City);
    this.populateAttributeValue("Employer State CBI", mailInfoObject.State);
    this.populateAttributeValue(
      "Employer Zip Code CBI",
      mailInfoObject.ZipCode
    );
    this.populateAttributeValue("Employer State(Abbr) CBI", this.employerStateAbbrCBIIDValue);
    this.orderFlowService.isOrderlInfoAvailable = true;
    this.orderFlowService.setOrderInfo(this.orderInfo);
    this.orderFlowService.IncrementStep();
    if (this.orderInfo.selectedOrderType == "1") {
      this.router.navigate(["createOrder/schedulecalender"]);
    } else {
      this.router.navigate(["createOrder/OrderReview"]);
    }
  }

  goBack() {
    this.router.navigate(["createOrder/serviceDetails"]);
  }
}
