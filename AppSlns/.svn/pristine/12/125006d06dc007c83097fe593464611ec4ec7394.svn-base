import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl } from "@angular/forms";
import { AddressInfo, LookupInfo } from "../../models/user/user.model";
import { RegisterUserFormService } from "../../services/register-user/register-user-form.service";
import { FormBuilder } from "@angular/forms";
import { Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { LookupService } from "../../services/shared-services/lookup.service";
import { UtilityService } from "../../services/shared-services/utility.service";
import { KeyedCollection } from "../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
import { LanguageTranslationService } from "../../services/language-translations/language-translations.service";

@Component({
  selector: "address-info",
  templateUrl: "address.component.html"
})
export class AddressComponent implements OnInit {
  title = "Address";
  address: AddressInfo;
  lstCountry: Array<LookupInfo>;
  lstStates: Array<LookupInfo>;
  selectedCountryId: string;
  selectedCountryName: string;
  selectedStateID: string;
  selectedState: string = "";
  HideState: boolean=false;
  IsZipCodeLabel: boolean = true;
  address_validation_messages: any;

  ///// language value parameters region start

  CRTACCOUNTLANGVAL: string;
  ADDRESSLANGVAL: string;
  ADDRESSREQLANGVAL: string;
  COUNTRYLANGVAL: string;
  SELECTWITHHYPENSLANGVAL: string;
  PLSSELCOUNTRYLANGVAL: string;
  STATELANGVAL: string;
  PLSSELSTATELANGVAL: string;
  CITYLANGVAL: string;
  CITYREQLANGVAL: string;
  ZIPLANGVAL: string;
  ENTERVALIDZIPCODEVALDTNLANGVAL: string;
  NEXTLANGVAL: string;

  //// language value parameters region end

  cityRegex = "^[\\x01-\\x7F]+$";
  zipRegex = "^[\\x01-\\x7F]+$";
  addressRegex = "^[\\x01-\\x7F]+$";
  addressForm = this.formBuilder.group({
    address: ["", [Validators.required, Validators.pattern(this.addressRegex)]],
    city: ["", [Validators.required, Validators.pattern(this.cityRegex)]],
    Zipcode: ["", [Validators.required,Validators.pattern(this.zipRegex)]],
    State: ["", Validators.required],
    Country: ["", Validators.required]
  });
  initializeValidationMessages(validationMsgs: KeyedCollection<string>) {
    this.address_validation_messages = {
      'address': [
        { type: 'required', message: validationMsgs["ADDRESSREQ"] },
        {
          type: "pattern", message: validationMsgs["ADDRESSASCIIINVALIDCODE"]  
      },
      ],
      'city': [
        { type: 'required', message: validationMsgs["CITYREQ"] },
        {
          type: "pattern", message: validationMsgs["CITYINVALIDASCIICODE"]
  
      },],
      'Zipcode': [
        { type: 'required', message: validationMsgs["ENTERVALIDZIPCODEVALDTN"] },
        {
          type: "pattern", message: validationMsgs["ENTERVALIDZIPASCIICODE"]  
      },
      ],
      'Postalcode': [ 
        { type: 'required', message: validationMsgs["ENTERVALIDPOSTALCODEVALDTN"] },
        {
          type: "pattern", message: validationMsgs["ENTERVALIDPOSTALASCIICODE"]  
      },
      ],
      'State': [
        { type: 'required', message: validationMsgs["PLSSELSTATE"] },],
      'Country': [
        { type: 'required', message: validationMsgs["PLSSELCOUNTRY"] },]
    }
  }

  constructor(
    private registerUserFormService: RegisterUserFormService,
    private formBuilder: FormBuilder,
    private router: Router,
    private lookupservice: LookupService,
    private utilityService: UtilityService,
    private languageTranslationService: LanguageTranslationService
  ) { }

  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );
  ValidationMsgsObservable = this.dataSource.asObservable();
  setLocalization() {
    var lstKeys = ["CRTACCOUNT", "ADDRESS", "COUNTRY", "STATE", "CITY", "ZIPCODE", 
    "POSTALCODE", "PREVIOUS", "NEXT","CNFMCNLREGISTRATION","CNCLREGISTRATION","LSTITMY","LSTITMN","CNCL"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );

    var lstKeysDuplicates=["PLEASESELECT"];
    this.utilityService.SubscribeLocalListWithDupNames(
      this.languageTranslationService,
      lstKeysDuplicates
    );
    var lstValidationKeys = ["ADDRESSREQ", "CITYREQ", "PLSSELSTATE", "PLSSELCOUNTRY",
    "ENTERVALIDPOSTALCODEVALDTN","ENTERVALIDZIPCODEVALDTN", "CITYINVALIDASCIICODE","ENTERVALIDPOSTALASCIICODE","ENTERVALIDZIPASCIICODE","ADDRESSASCIIINVALIDCODE"];
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

  ngOnInit() {
    var userInfo = this.registerUserFormService.getUserInfo();
    if (userInfo.IsAliasInfoAvail == false) {
      this.router.navigate([""]);
    }

    this.setLocalization();
    this.address = this.registerUserFormService.getAddressInfo();
    this.lookupservice.getCountry().subscribe(values => {
      if (values.length != undefined) {
        this.lstCountry = values;
      }
    });

    if (this.registerUserFormService.isAddresslInfoAvailable) {
      this.populateFormControls();
    }
  }
  isFieldValid(field: string) {
    return this.utilityService.isFieldValid(this.addressForm, field);
  }

  displayFieldCss(field: string) {
    return this.utilityService.displayFieldCss(this.addressForm, field);
  }
  isError(field: string) {
    return this.utilityService.isError(this.addressForm, field, this.address_validation_messages[field]);
  }
  errorMessage(field: string) {
    var validationField = field == 'Zipcode' ? this.IsZipCodeLabel ? 'Zipcode' : 'Postalcode' : field;
    return this.utilityService.ErrorMessage(this.addressForm, field, this.address_validation_messages[validationField]);
  }
  onCountrySelected(selectedCountryID: number, Name: string) {
    this.addressForm.controls["State"].setValue("");
    this.selectedCountryId = selectedCountryID.toString();
    this.selectedCountryName = this.lstCountry.find(
      s => s.ID == selectedCountryID
    ).Name;
    if (
      this.selectedCountryName == "CANADA" ||
      this.selectedCountryName == "UNITED STATES of AMERICA" ||
      this.selectedCountryName == "UNITED STATES of AMERICA - STATE" ||
      this.selectedCountryName == "MEXICO"
    ) {
      this.HideState = false;
      this.IsZipCodeLabel = true;
      this.lookupservice.getStates(selectedCountryID).subscribe(values => {
        if (values.length != undefined) {
          this.lstStates = values;
          this.selectedStateID = undefined;
        }
      });
    } else {
      this.IsZipCodeLabel = false;
      this.addressForm.controls["State"].setValue("SITI@4863");
      this.selectedState = "SITI@4863";
      this.HideState = true;
    }
  }

  onStateSelected(selectedStateID: number) {
    this.selectedStateID = selectedStateID.toString();
    this.selectedState = this.lstStates.find(s => s.ID == selectedStateID).Name;
  }

  populateFormControls() {
    var addressInfo = this.registerUserFormService.getAddressInfo();
    var formControls = this.addressForm.controls;
    formControls["address"].setValue(addressInfo.Address);
    formControls["city"].setValue(addressInfo.City);
    formControls["Zipcode"].setValue(addressInfo.ZipCode);
    this.selectedCountryId = addressInfo.CountryId;
    this.selectedCountryName = addressInfo.CountryName;
    if (addressInfo.StateId != "0") {
      if (
        this.selectedCountryName == "CANADA" ||
        this.selectedCountryName == "UNITED STATES of AMERICA" ||
        this.selectedCountryName == "UNITED STATES of AMERICA - STATE" ||
        this.selectedCountryName == "MEXICO"
      ) {
        this.HideState = false;
        this.IsZipCodeLabel = true;
        this.lookupservice
          .getStates(parseInt(this.selectedCountryId))
          .subscribe(values => {
            if (values.length != undefined) {
              this.lstStates = values;
            }
          });
      } else {
        this.IsZipCodeLabel = false;
        this.HideState = true;
      }
    }
    this.selectedStateID = addressInfo.StateId;
    this.selectedState = addressInfo.StateName;
  }

  onSubmit(form: any) {
    if (this.addressForm.valid) {
      this.address.CountryId = this.selectedCountryId;
      this.address.CountryName = this.selectedCountryName;
      this.address.StateId = this.selectedStateID;
      this.address.StateName = this.selectedState;
      var addressFormObject = this.addressForm.value;
      this.address.Address = addressFormObject.address;
      this.address.City = addressFormObject.city;
      this.address.ZipCode = addressFormObject.Zipcode;
      this.registerUserFormService.setAddressInfo(this.address);

      var userInfo = this.registerUserFormService.getUserInfo();
      userInfo.IsAddressInfoAvail = true;
      this.registerUserFormService.setUserInfo(userInfo);

      this.router.navigate(["/registerUser/contact"]);
    } else {
      this.utilityService.validateAllFormFields(this.addressForm);
    }
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
  goBack() {
    var userInfo = this.registerUserFormService.getUserInfo();
    userInfo.IsAliasInfoAvail = false;
    this.registerUserFormService.setUserInfo(userInfo);

    this.router.navigate(["registerUser/alias"]);
  }
}