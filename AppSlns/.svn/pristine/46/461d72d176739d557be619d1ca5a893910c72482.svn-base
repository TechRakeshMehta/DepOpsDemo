import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl } from "@angular/forms";
import { AddressInfo, LookupInfo } from "../../../models/user/user.model";
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { FormBuilder } from "@angular/forms";
import { Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { LookupService } from "../../../services/shared-services/lookup.service";
import { UtilityService } from "../../../services/shared-services/utility.service";
import { UserInfo } from "../../../models/user/user.model";
import { OrderInfo } from "../../../models/order-flow/order.model";
import { KeyedCollection } from "../../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
import { basename } from "path";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";

@Component({
  selector: "address-info",
  templateUrl: "addressInfo.component.html"
})
export class AddressInfoComponent implements OnInit {
  title = "Address";
  address: AddressInfo;
  lstCountry: Array<LookupInfo>;
  lstStates: Array<LookupInfo>;
  selectedCountryId: string = "";
  selectedCountryName: string;
  selectedState: string;
  HideState: boolean;  
  IsEditProfile:boolean;
  IsZipCodeLabel : boolean = true;
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
  address_validation_messages: any;
  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();

  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );

  ValidationMsgsObservable = this.dataSource.asObservable();
  setLocalization() {
    //labels
    var lstKeys = ["CREATODR","OF", "STEP", "PERSONALINFO","ZIPCODE","POSTALCODE", "ADDRESS", "COUNTRY", 
    "STATE", "CITY","PREVIOUS","NEXT","CNFMCNLORDER","CNCLORDR","LSTITMY","LSTITMN","CNCL","UPDATE"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
   // validaton msgs    
    var lstValidaionKeys = ["ADDRESSREQ", "CITYREQ", "ENTERVALIDZIPCODEVALDTN","ZIPCODE","POSTALCODE", "ENTERVALIDPOSTALCODEVALDTN",
     "PLSSELSTATE","PLSSELCOUNTRY","CITYINVALIDASCIICODE","ENTERVALIDPOSTALASCIICODE","ENTERVALIDZIPASCIICODE","ADDRESSASCIIINVALIDCODE"];
    this.utilityService.PopulateValidationCollectionFromKeys(lstValidaionKeys, this.validationMessages);


    var lstKeysDuplicates=["PLEASESELECT"];
    var lstKeysDuplicates=["PAYMENTINST"];
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
  initializeValidationMessages(validationMsgs: KeyedCollection<string>) {
    this.address_validation_messages = {
    'address': [
      { type: 'required', message: validationMsgs["ADDRESSREQ"]},
      {
        type: "pattern", message: validationMsgs["ADDRESSASCIIINVALIDCODE"]  
    },
    ],
    'city': [
      { type: 'required', message: validationMsgs["CITYREQ"]},
      {
        type: "pattern", message: validationMsgs["CITYINVALIDASCIICODE"]

    },
  ],
      'Zipcode': [
        { type: 'required', message: validationMsgs["ENTERVALIDZIPCODEVALDTN"]},
        {
          type: "pattern", message: validationMsgs["ENTERVALIDZIPASCIICODE"]  
      },
      ],
      'Postalcode': [
        { type: 'required', message: validationMsgs["ENTERVALIDPOSTALCODEVALDTN"]},
        {
          type: "pattern", message: validationMsgs["ENTERVALIDPOSTALASCIICODE"]  
      },
      ],
    'State': [
      { type: 'required',  message: validationMsgs["PLSSELSTATE"]},],
    'Country': [
      { type: 'required',  message: validationMsgs["PLSSELCOUNTRY"] },]
  }
  }
  constructor(
    private orderFlowService: OrderFlowService,
    private formBuilder: FormBuilder,
    private router: Router,
    private lookupservice: LookupService,
    private utilityService: UtilityService,
    private languageTranslationService: LanguageTranslationService
  ) { }
  userInfo: UserInfo;
  orderInfo: OrderInfo;
  ngOnInit() {
    this.IsEditProfile=this.orderFlowService.IsEditProfile;
    this.setLocalization();
    this.selectedCountryId = undefined;
    this.orderInfo = this.orderFlowService.getOrderInfo();
    this.userInfo = this.orderFlowService.getOrganizationUserInfoDetails();
    this.lookupservice.getCountry().subscribe(values => {
      if (values.length != undefined) {
        this.lstCountry = values;        
      }
    });
    // if (this.orderFlowService.isAddresslInfoAvailable) {
    this.populateFormControls();

    // }
  }
  populateFormControls() {

    var addressInfo = this.userInfo;
    var formControls = this.addressForm.controls;
    formControls["address"].setValue(addressInfo.Address);
    formControls["city"].setValue(addressInfo.CityName);
    formControls["Zipcode"].setValue(addressInfo.PostalCode);
    if(addressInfo.CountryId.toString() > "0")
    {
      this.selectedCountryId = addressInfo.CountryId.toString();
    }

    this.selectedCountryName = addressInfo.CountryName;
    if (addressInfo.StateName == "" || addressInfo.StateName == null) {
      this.addressForm.controls["State"].setValue("SITI@4863");
      this.selectedState = "SITI@4863";
      this.HideState = true;
      this.IsZipCodeLabel = false;
      addressInfo.StateName = this.selectedState;
    }
    else {
      if (
        this.selectedCountryName == "CANADA" ||
        this.selectedCountryName == "UNITED STATES of AMERICA - STATE" ||
        this.selectedCountryName == "MEXICO" ||
        this.selectedCountryName == "UNITED STATES of AMERICA"
      ) {
        this.IsZipCodeLabel = true;
        
        this.HideState = false;
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
      //}

      this.selectedState = addressInfo.StateName;
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
    var validationField =  field == 'Zipcode' ? this.IsZipCodeLabel ? 'Zipcode' : 'Postalcode': field;
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
      this.selectedCountryName == "UNITED STATES of AMERICA - STATE" ||
      this.selectedCountryName == "UNITED STATES of AMERICA" ||
      this.selectedCountryName == "MEXICO"      
    ) {            
      this.HideState = false;
      this.IsZipCodeLabel = true;
      
      this.lookupservice.getStates(selectedCountryID).subscribe(values => {
        if (values.length != undefined) {
          this.lstStates = values;
          this.selectedState = undefined;
        }
      });

    } else {
      this.IsZipCodeLabel = false;
      
      this.addressForm.controls["State"].setValue("SITI@4863");
      this.selectedState = "SITI@4863";
      this.HideState = true;
    }
  }

  onStateSelected(selectedState: string) {
    this.selectedState = selectedState;
  }

 

  onSubmit(form: any) {
    if (this.addressForm.valid) {
      this.userInfo.CountryId = parseInt(this.selectedCountryId);
      this.userInfo.CountryName = this.selectedCountryName;
      if (this.selectedState == "SITI@4863") {
        this.userInfo.StateName = "";
      }
      else {
        this.userInfo.StateName = this.selectedState;
      }
      var addressFormObject = this.addressForm.value;
      this.userInfo.Address = addressFormObject.address;
      this.userInfo.CityName = addressFormObject.city;
      this.userInfo.PostalCode = addressFormObject.Zipcode;
      this.orderFlowService.setOrganizationUserInfoDetails(this.userInfo); 
      this.router.navigate(["/createOrder/contactInfo"]);
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
    this.router.navigate(["createOrder/aliasInfo"]);
  }  
}
