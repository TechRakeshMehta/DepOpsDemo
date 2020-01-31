import { Component, OnInit } from '@angular/core';
import { GeocodeService } from '../../../services/shared-services/google-map.service';
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { RegisterUserFormService } from '../../../services/register-user/register-user-form.service';
import { UtilityService } from '../../../services/shared-services/utility.service';
import { OrderInfo, LocationDetail } from '../../../models/order-flow/order.model';
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { KeyedCollection } from "../../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
import { basename } from "path";

@Component({
  selector: 'location-reschedule',
  templateUrl: './location-reschedule.component.html'
})
export class LocationRescheduleComponent implements OnInit {
  zipcode: string;
  IsDisabled: boolean = true;
  orderInfo: OrderInfo;
  result = new Array<any>();
  NoLocationAvailMsg: any;
  IsPreviousButtonVisible: boolean = true;
  isSendForOnline: boolean = false;
  LocationForm = this.formBuilder.group({
    Zipcode: ["", Validators.required],
  });
  constructor(private geoCoder: GeocodeService,
    private formBuilder: FormBuilder,
    private router: Router,
    private orderFlowService: OrderFlowService,
    private utilityService: UtilityService,
    private languageTranslationService: LanguageTranslationService,
  ) { }
  lng: string
  lat: string
  IsLocationsAvail: boolean = true;
  locationID: number;
  Location_validation_messages: any;
  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );
  ValidationMsgsObservable = this.dataSource.asObservable();
  setLocalization() {
    var lstKeys = ["PREVIOUS", "ENTRZIPCODE", "RSCHDLAPPNMNT", "OF", "FNDLOC", "NAME", "ADDRESS", "STEP", "CHSLOC", "SEARCH",
      "CNFMCNLRESCHEDULING", "CNCLRESCHEDULING", "LSTITMY", "LSTITMN", "CNCL"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
    var lstKeysDuplicates = ["NEXT"];
    this.utilityService.SubscribeLocalListWithDupNames(
      this.languageTranslationService,
      lstKeysDuplicates
    );
    var lstValidationKeys = ["PLSSELZIPCODE", "LOCATIONERROR"];
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
    this.Location_validation_messages = {
      Zipcode: [{ type: "required", message: validationMsgs["PLSSELZIPCODE"] }],
    }
    this.NoLocationAvailMsg = validationMsgs["LOCATIONERROR"];
  }
  ngOnInit() {
    this.IsPreviousButtonVisible = this.orderFlowService.isOrderTypeChange || this.orderFlowService.isSendForOnline;
    this.isSendForOnline = this.orderFlowService.isSendForOnline;
    this.setLocalization();
    this.orderInfo = this.orderFlowService.getAlreadyPlacedOrder();
    this.orderInfo.userInfo = this.orderFlowService.getOrderInfo().userInfo;
    this.PopuldateLocationData();
  }
  PopuldateLocationData() {
    var formControls = this.LocationForm.controls;
    if (this.orderInfo.DefaultZipCode != "") {
      this.zipcode = this.orderInfo.DefaultZipCode;
    }
    else if (this.orderInfo.userInfo.PostalCode != "") {
      this.zipcode = this.orderInfo.userInfo.PostalCode;
      this.orderInfo.DefaultZipCode = this.orderInfo.userInfo.PostalCode;
    }
    else {
      this.orderFlowService.GetDefaultZipcode().then(a => {
        this.zipcode = this.orderFlowService.getOrderInfo().DefaultZipCode;
        formControls["Zipcode"].setValue(this.zipcode);
        this.GetListOfLocations((this.zipcode).toString());
      });
    }
    if (this.zipcode != undefined && this.zipcode.length > 0) {
      formControls["Zipcode"].setValue(this.zipcode);
      this.GetListOfLocations((this.zipcode).toString());
    }
  }
  onSearch() {
    this.IsLocationsAvail = true;
    if (this.LocationForm.valid) {
      this.locationID = 0;
      var LocationFormObject = this.LocationForm.value;
      this.GetListOfLocations(LocationFormObject.Zipcode);
    }
    else {
      this.utilityService.validateAllFormFields(this.LocationForm);
    }
    this.IsDisabled = true;
  }
  GetListOfLocations(zipcode: string) {
    this.geoCoder.geocodeLocation(zipcode.toString()).then(res => {
      if (this.geoCoder.error != "ZERO_RESULTS") {
        this.lng = this.geoCoder.lng;
        this.lat = this.geoCoder.lat;
        this.orderFlowService.GetLocation(this.lng, this.lat).
          then(
            res => { // Success
              this.result = Array.of(this.orderFlowService.lstLocations)[0];
              if (this.orderInfo.LocationDetail.LocationID > 0
                && this.orderInfo.DefaultZipCode == zipcode) {
                this.selectLocation(this.orderInfo.LocationDetail.LocationID);
              }
            }
          );
        this.geoCoder.error = "";
      }
      else {
        this.result = [];
        this.IsLocationsAvail = false;
      }
    });
  }

  isError(field: string) {
    return this.utilityService.isError(
      this.LocationForm,
      field,
      this.Location_validation_messages[field]
    );
  }
  errorMessage(field: string) {
    return this.utilityService.ErrorMessage(
      this.LocationForm,
      field,
      this.Location_validation_messages[field]
    );
  }
  isFieldValid(field: string) {
    return this.utilityService.isFieldValid(this.LocationForm, field);
  }

  displayFieldCss(field: string) {
    return this.utilityService.displayFieldCss(this.LocationForm, field);
  }
  selectLocation(locationId: number) {
    this.locationID = locationId;
    if (locationId != undefined && locationId > 0) {
      this.orderInfo.LocationDetail.LocationName = this.result.find(x => x.LocationID == this.locationID).LocationName;
      this.orderInfo.LocationDetail.LocationAddress = this.result.find(x => x.LocationID == this.locationID).FullAddress;
      this.orderInfo.LocationDetail.LocationDescription = this.result.find(x => x.LocationID == this.locationID).Description;
      this.IsDisabled = false;
    }
    else {
      this.IsDisabled = true;
    }
  }
  onSubmit() {
    var formControls = this.LocationForm.controls;
    if (this.locationID > 0) {      
      this.orderFlowService.GetAppointmentSlots(this.locationID).subscribe(values => {
        if (values != undefined) {
          this.orderFlowService.isLocationChange = true;
          this.orderInfo.lstScheduleSlots = values;
          this.orderInfo.DefaultZipCode = formControls["Zipcode"].value.toString();
          if(this.orderInfo.LocationDetail.LocationID != this.locationID)
          {
            this.orderInfo.LocationDetail.IsLocationUpdate =true;
          }
          this.orderInfo.LocationDetail.LocationID = this.locationID;
          this.orderFlowService.setAlreadyPlacedOrder(this.orderInfo);
          this.router.navigate(["orderDetail/calenderReschedule"]);
        }
      });
    }
  }

  goBack() {
    this.router.navigate(["orderDetail/orderReschedule"]);
  }

}
