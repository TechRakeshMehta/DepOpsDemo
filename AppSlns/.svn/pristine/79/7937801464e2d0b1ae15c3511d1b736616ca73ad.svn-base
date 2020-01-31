import { Component, OnInit } from '@angular/core';
import { LookupService } from "../../../services/shared-services/lookup.service";
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { FormBuilder, FormControl } from "@angular/forms";
import { Validators } from "@angular/forms";
import { LookupInfo } from "../../../models/user/user.model";
import { Router } from "@angular/router";
import { AttributesForCustomForm } from '../../../models/custom-forms/custom-attribute';
import { debug } from 'util';
import { OrderInfo } from '../../../models/order-flow/order.model';
import { UtilityService } from "../../../services/shared-services/utility.service";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { KeyedCollection } from "../../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
import { basename } from "path";
import { CommonService } from '../../../services/shared-services/common.service';

@Component({
  selector: 'app-fingerprinting',
  templateUrl: './fingerprinting.component.html',
  styleUrls: ['./fingerprinting.component.css']
})
export class FingerPrintingComponent implements OnInit {
  regex = "^[0-9\s]*$";

  lstCustomAttribute: Array<AttributesForCustomForm>;
  infofingerPrintingGroupID: number;

  lstPlaceofBirthCountry: Array<any>;
  placeofbirthCountryAttrID: number;
  SelectedPlaceofCountry: string;
  lstPlaceofBirthState: Array<any>;
  placeofbirthStateAttrID: number;
  SelectedState: string;
  PlaceOfBirthCBIAbbrID: number;
  SelectedPlaceOfBirthCBI: string;
  lstCitizenShip: Array<any>;
  citizenShipAttrID: number;
  citizenShipCodeAttrID: number;
  SelectedCitizenShip: string;
  SelectedCitizenShipCode: string;
  lstRaceAttribute: Array<LookupInfo>;
  SelectedRaceName: string;
  lstEyeColorAttribute: Array<LookupInfo>;
  SelectedEyeColorName: string;
  lstHairColorAttribute: Array<LookupInfo>;
  SelectedHairColorName: string;
  lstHeightFeetAttribute: Array<LookupInfo>;
  SelectedHeightFeetName: string;
  lstHeightInchesAttribute: Array<LookupInfo>;
  SelectedHeightInchesName: string;
  customHtml: string = "";
  weight: string = "";
  customFormID: number;
  instanceID: number;
  groupID: number;
  orderInfo: OrderInfo;

  customForm = this.formBuilder.group({
    race: ["", Validators.required],
    eyeColor: ["", Validators.required],
    hairColor: ["", Validators.required],
    heightFeet: ["", Validators.required],
    heightInches: ["", Validators.required],
    Country: ["", Validators.required],
    State: ["", Validators.required],
    CitizenShip: ["", Validators.required],
    weight: ["", [Validators.required, Validators.pattern(this.regex), Validators.min(50), Validators.max(499)]]
  });

  constructor(
    private lookupService: LookupService,
    private formBuilder: FormBuilder,
    private router: Router,
    private orderFlowService: OrderFlowService,
    private utilityService: UtilityService,
    private languageTranslationService: LanguageTranslationService,
    private commonService: CommonService
  ) { }
  fingerprint_validation_messages: any;
  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );
  ValidationMsgsObservable = this.dataSource.asObservable();

  setLocalization() {

    var lstKeys = ["POBCOUNTRY", "POBSTATE", "CITIZENSHIP", "INFOFINGERPRINT", "RACE", "EYECOLOR", "OF",
      "HAIRCOLOR", "HEIGHTFEET", "HEIGHTINCH", "WEIGHT", "CREATODR", "STEP", "PREVIOUS", "NEXT",
      "CNFMCNLORDER", "CNCLORDR", "LSTITMY", "LSTITMN", "CNCL"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
    var lstValidationKeys = ["PLSENTRPLCBIRTHCOUNTRTY", "PLSENTRPLCBIRTHSTATE", "PLSENTRCITIZENSHIP",
      "PLSENTRRACE", "PLSENTREYECOLOR", "PLSENTRHAIRCOLOR", "PLSENTRHEIGHTFT", "PLSENTRHEIGHTINCH", "PLSENTRWEIGHT", "WGHTBTWNVALDTN"];
    this.utilityService.PopulateValidationCollectionFromKeys(lstValidationKeys, this.validationMessages);

    var lstKeysDuplicates = ["PLEASESELECT"];
    this.utilityService.SubscribeLocalListWithDupNames(
      this.languageTranslationService,
      lstKeysDuplicates
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

  setCustomFormsLocalization(isSpanishLanguage: boolean) {
    if (isSpanishLanguage) {
      this.customHtml = this.orderInfo.lstCustomAttributeInSpanish.filter(x => x.AttributeGroupId == this.infofingerPrintingGroupID)[0].CustomHtml;
      if ((this.customHtml != '' || this.customHtml != undefined) && document.getElementById("customHtml") != null) {
        document.getElementById("customHtml").innerHTML = this.customHtml;
      }
    }
    else {
      this.customHtml = this.lstCustomAttribute.filter(x => x.AttributeGroupId == this.infofingerPrintingGroupID)[0].CustomHtml;
      if ((this.customHtml != '' || this.customHtml != undefined) && document.getElementById("customHtml") != null) {
        document.getElementById("customHtml").innerHTML = this.customHtml;
      }
    }
  }
  initializeValidationMessages(validationMsgs: KeyedCollection<string>) {
    this.fingerprint_validation_messages = {
      Country: [{ type: "required", message: validationMsgs["PLSENTRPLCBIRTHCOUNTRTY"] }],
      State: [{ type: "required", message: validationMsgs["PLSENTRPLCBIRTHSTATE"] }],
      CitizenShip: [{ type: "required", message: validationMsgs["PLSENTRCITIZENSHIP"] }],
      race: [{ type: "required", message: validationMsgs["PLSENTRRACE"] }],
      eyeColor: [{ type: "required", message: validationMsgs["PLSENTREYECOLOR"] }],
      hairColor: [{ type: "required", message: validationMsgs["PLSENTRHAIRCOLOR"] }],
      heightFeet: [{ type: "required", message: validationMsgs["PLSENTRHEIGHTFT"] }],
      heightInches: [{ type: "required", message: validationMsgs["PLSENTRHEIGHTINCH"] }],
      weight: [
        { type: 'required', message: validationMsgs["PLSENTRWEIGHT"] },
        { type: 'min', message: validationMsgs["WGHTBTWNVALDTN"] },
        { type: 'max', message: validationMsgs["WGHTBTWNVALDTN"] },
        { type: 'pattern', message: validationMsgs["WGHTBTWNVALDTN"] }
      ]

    }
  }
  ngOnInit() {
    this.setLocalization();
    this.lookupService.getCustomAttributeOptions('Race').subscribe(values => {
      if (values != undefined) {
        this.lstRaceAttribute = values;
      }
    });
    this.lookupService.getCustomAttributeOptions('Eye Color').subscribe(values => {
      if (values != undefined) {
        this.lstEyeColorAttribute = values;
      }
    });
    this.lookupService.getCustomAttributeOptions('Hair Color').subscribe(values => {
      if (values != undefined) {
        this.lstHairColorAttribute = values;
      }
    });
    this.lookupService.getCustomAttributeOptions('Height Feet').subscribe(values => {
      if (values != undefined) {
        this.lstHeightFeetAttribute = values;
      }
    });
    this.lookupService.getCustomAttributeOptions('Height Inches').subscribe(values => {
      if (values != undefined) {
        this.lstHeightInchesAttribute = values;
      }
    });
    this.orderInfo = this.orderFlowService.getOrderInfo();
    if (this.orderFlowService.isOrderlInfoAvailable) {
      this.lstCustomAttribute = this.orderFlowService.getCustomAttributeInfo();
      this.bindCascadingDropdown();
      this.populateFormControls();
    } else {
      this.orderFlowService.getServiceDetails(this.orderInfo.bkgPackageId, this.orderInfo.CbiUniqueId, 'AAAA').subscribe((values: any) => {
        if (values != undefined) {
          this.lstCustomAttribute = values.lstCustomFormAttributes;
          this.customFormID = values.customFormId;
          this.instanceID = values.instanceId;
          this.groupID = values.groupId;
          this.bindCascadingDropdown();
          this.orderFlowService.getServiceDetails(this.orderInfo.bkgPackageId, this.orderInfo.CbiUniqueId, 'AAAB').subscribe((values: any) => {
            if (values != undefined) {
              this.orderInfo.lstCustomAttributeInSpanish = values.lstCustomFormAttributes;
              this.commonService.isLanguageSpanish.subscribe(isSpanishLang => {
                this.setCustomFormsLocalization(isSpanishLang);
              });
            }
          }); 
        }
      }); 
    } 
  }

  bindCascadingDropdown() {
    this.infofingerPrintingGroupID = this.lstCustomAttribute.filter(x => x.AttriButeGroupName == "CBI Finger Printing Attribute Group")[0].AttributeGroupId;
    this.placeofbirthCountryAttrID = this.lstCustomAttribute.filter(x => x.AttributeGroupId == this.infofingerPrintingGroupID && x.Name == "Place Of Birth CBI (Country)")[0].AttributeId;
    this.placeofbirthStateAttrID = this.lstCustomAttribute.filter(x => x.AttributeGroupId == this.infofingerPrintingGroupID && x.Name == "Place Of Birth CBI (State)")[0].AttributeId;
    this.citizenShipAttrID = this.lstCustomAttribute.filter(x => x.AttributeGroupId == this.infofingerPrintingGroupID && x.Name == "Citizenship CBI")[0].AttributeId;
    this.PlaceOfBirthCBIAbbrID = this.lstCustomAttribute.filter(x => x.AttributeGroupId == this.infofingerPrintingGroupID && x.Name == "Place Of Birth CBI (Abbr)")[0].AttributeId;
    this.citizenShipCodeAttrID = this.lstCustomAttribute.filter(x => x.AttributeGroupId == this.infofingerPrintingGroupID && x.Name == "Citizenship Code CBI")[0].AttributeId;

    this.lookupService.GetCascadingAttributeData(this.infofingerPrintingGroupID, this.placeofbirthCountryAttrID).subscribe(values => {
      if (values != undefined) {
        this.lstPlaceofBirthCountry = values;
      }
    });

    this.lookupService.GetCascadingAttributeData(this.infofingerPrintingGroupID, this.citizenShipAttrID).subscribe(values => {
      if (values != undefined) {
        this.lstCitizenShip = values;
      }
    });

    if (this.SelectedPlaceofCountry == "") {
      this.SelectedPlaceofCountry = "UNITED STATES of AMERICA - STATE";
      this.lookupService.GetCascadingAttributeData(this.infofingerPrintingGroupID, this.placeofbirthStateAttrID, this.SelectedPlaceofCountry).subscribe(values => {
        if (values != undefined) {
          this.lstPlaceofBirthState = values;
        }
      });
    }
  }

  onCountrySelected(selectedCountryID: string) {
    this.customForm.controls["State"].setValue("");
    this.SelectedPlaceofCountry = selectedCountryID;
    this.lookupService.GetCascadingAttributeData(this.infofingerPrintingGroupID, this.placeofbirthStateAttrID, this.SelectedPlaceofCountry).subscribe(values => {
      if (values != undefined) {
        this.lstPlaceofBirthState = values;
        if (this.lstPlaceofBirthState != undefined && this.lstPlaceofBirthState.length == 1) {
          this.SelectedState = this.lstPlaceofBirthState[0];
          this.onStateSelected(this.SelectedState);
        } else if (this.SelectedState == '') {
          this.SelectedState = undefined;
        }
      }
    });
  }

  onStateSelected(selectedStateID: string) {
    this.SelectedState = selectedStateID;
    this.lookupService.GetCascadingAttributeData(this.infofingerPrintingGroupID, this.PlaceOfBirthCBIAbbrID, this.SelectedState).subscribe(values => {
      if (values != undefined && values.length > 0) {
        this.SelectedPlaceOfBirthCBI = values[0];
      }
    });
  }

  onCitizenshipSelected(selectedCitizenship: string) {
    this.lookupService.GetCascadingAttributeData(this.infofingerPrintingGroupID, this.citizenShipCodeAttrID, selectedCitizenship).subscribe(values => {
      if (values != undefined && values.length > 0) {
        this.SelectedCitizenShipCode = values[0];
      }
    });
  }

  populateFormControls() {
    this.SelectedRaceName = this.lstCustomAttribute.filter(x => x.Name == "Race")[0].AttributeDataValue;
    this.SelectedEyeColorName = this.lstCustomAttribute.filter(x => x.Name == "Eye Color")[0].AttributeDataValue;
    this.SelectedHairColorName = this.lstCustomAttribute.filter(x => x.Name == "Hair Color")[0].AttributeDataValue;
    this.SelectedHeightFeetName = this.lstCustomAttribute.filter(x => x.Name == "Height Feet")[0].AttributeDataValue;
    this.SelectedHeightInchesName = this.lstCustomAttribute.filter(x => x.Name == "Height Inches")[0].AttributeDataValue;
    this.weight = this.lstCustomAttribute.filter(x => x.Name == "Weight")[0].AttributeDataValue;
    this.SelectedPlaceofCountry = this.lstCustomAttribute.filter(x => x.Name == "Place Of Birth CBI (Country)")[0].AttributeDataValue;
    this.onCountrySelected(this.SelectedPlaceofCountry);
    this.SelectedState = this.lstCustomAttribute.filter(x => x.Name == "Place Of Birth CBI (State)")[0].AttributeDataValue;
    this.SelectedCitizenShip = this.lstCustomAttribute.filter(x => x.Name == "Citizenship CBI")[0].AttributeDataValue;
    this.SelectedPlaceOfBirthCBI = this.lstCustomAttribute.filter(x => x.Name == "Place Of Birth CBI (Abbr)")[0].AttributeDataValue;
    this.SelectedCitizenShipCode = this.lstCustomAttribute.filter(x => x.Name == "Citizenship Code CBI")[0].AttributeDataValue;
    this.commonService.isLanguageSpanish.subscribe(isSpanishLang => {
      this.setCustomFormsLocalization(isSpanishLang);
    });
    this.customFormID = this.orderInfo.customFormID;
  }
  onSubmit(form: any) {
    if (this.customForm.valid) {
      this.lstCustomAttribute.filter(x => x.Name == "Race")[0].AttributeDataValue = this.SelectedRaceName;
      this.lstCustomAttribute.filter(x => x.Name == "Eye Color")[0].AttributeDataValue = this.SelectedEyeColorName;
      this.lstCustomAttribute.filter(x => x.Name == "Hair Color")[0].AttributeDataValue = this.SelectedHairColorName;
      this.lstCustomAttribute.filter(x => x.Name == "Height Feet")[0].AttributeDataValue = this.SelectedHeightFeetName;
      this.lstCustomAttribute.filter(x => x.Name == "Height Inches")[0].AttributeDataValue = this.SelectedHeightInchesName;
      this.lstCustomAttribute.filter(x => x.Name == "Weight")[0].AttributeDataValue = this.weight;

      this.lstCustomAttribute.filter(x => x.Name == "Place Of Birth CBI (Country)")[0].AttributeDataValue = this.SelectedPlaceofCountry;
      this.lstCustomAttribute.filter(x => x.Name == "Place Of Birth CBI (State)")[0].AttributeDataValue = this.SelectedState;
      this.lstCustomAttribute.filter(x => x.Name == "Citizenship CBI")[0].AttributeDataValue = this.SelectedCitizenShip;
      this.lstCustomAttribute.filter(x => x.Name == "Place Of Birth CBI (Abbr)")[0].AttributeDataValue = this.SelectedPlaceOfBirthCBI;
      this.lstCustomAttribute.filter(x => x.Name == "Citizenship Code CBI")[0].AttributeDataValue = this.SelectedCitizenShipCode;

      this.lstCustomAttribute.forEach(x => {
        this.orderFlowService.setCustomFromAttributeInfo(x);
      });
      this.orderFlowService.isOrderlInfoAvailable = true;

      this.orderInfo.customFormInstanceId = this.instanceID;
      this.orderInfo.customFormID = this.customFormID;
      this.orderInfo.customFormGroupId = this.groupID;
      this.orderFlowService.setOrderInfo(this.orderInfo);
      this.router.navigate(["createOrder/serviceDetails"]);
    }
    else {
      this.utilityService.validateAllFormFields(this.customForm);
    }
  }

  // validation_messages = {
  //   'weight': [
  //     { type: 'required', message: 'Please enter Weight' },
  //     { type: 'min', message: 'Weight should be between 50 to 499' },
  //     { type: 'max', message: 'Weight should be between 50 to 499' },
  //     { type: 'pattern', message: 'Weight should be between 50 to 499' }
  //   ]
  // }

  isError(field: string) {
    return this.utilityService.isError(this.customForm, field, this.fingerprint_validation_messages[field]);
  }
  errorMessage(field: string) {
    return this.utilityService.ErrorMessage(this.customForm, field, this.fingerprint_validation_messages[field]);
  }

  isFieldValid(field: string) {
    return this.utilityService.isFieldValid(this.customForm, field);
  }

  displayFieldCss(field: string) {
    return this.utilityService.displayFieldCss(this.customForm, field);
  }

  goBack() {
    this.orderFlowService.DecrementStep();
    this.router.navigate(["createOrder/contactInfo"]);
  }
}
