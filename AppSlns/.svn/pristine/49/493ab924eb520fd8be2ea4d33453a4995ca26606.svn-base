import { Component, OnInit, TemplateRef } from '@angular/core';
import { GeocodeService } from '../../../services/shared-services/google-map.service';
import { OrderFlowService } from '../../../services/order-flow/order-flow.service';
import { Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { RegisterUserFormService } from '../../../services/register-user/register-user-form.service';
import { UtilityService } from '../../../services/shared-services/utility.service';
import { OrderInfo, LocationDetail } from '../../../models/order-flow/order.model';
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { KeyedCollection } from "../../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
import { basename } from "path";
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
@Component({
  selector: 'app-location',
  templateUrl: './location.component.html',
  styleUrls: ['./location.component.css']
})
export class LocationComponent implements OnInit {
  refresh: any;
  IsImageError: boolean=false;
  refreshMsg: any;
  locationName: String;
  NoImgMsg: any;
  Locations: any;
  zipcode: string;
  IsDisabled: boolean = true;
  orderInfo: OrderInfo;
  // lstLocatons: new Array<any>();
  result = new Array<any>();
  NoLocationAvailMsg: any;
  modalRef: BsModalRef;
  LocationForm = this.formBuilder.group({
    Zipcode: ["", Validators.required],
  });
  constructor(private geoCoder: GeocodeService,
    private formBuilder: FormBuilder,
    private router: Router,
    private orderFlowService: OrderFlowService,
    private utilityService: UtilityService,
    private languageTranslationService: LanguageTranslationService,
    private modalService: BsModalService
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
    var lstKeys = ["PREVIOUS", "ENTRZIPCODE", "CREATODR", "OF", "FNDLOC", "NAME", "ADDRESS", "STEP", "CHSLOC", "SEARCH", "IMGSNOTAVAIL","RFRESHMSG","REFRESH"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
    var lstKeysDuplicates = ["NEXT", "IMAGES"];
    this.utilityService.SubscribeLocalListWithDupNames(
      this.languageTranslationService,
      lstKeysDuplicates
    );
    var lstValidationKeys = ["PLSSELZIPCODE", "LOCATIONERROR", "IMGSNOTAVAIL","RFRESHMSG","REFRESH"];
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
    this.NoImgMsg = validationMsgs["IMGSNOTAVAIL"];
    this.refreshMsg=validationMsgs["RFRESHMSG"];
    this.refresh=validationMsgs["REFRESH"];
  }
  ngOnInit() {
    this.setLocalization();
    this.orderInfo = this.orderFlowService.getOrderInfo();
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
        this.zipcode = this.orderInfo.DefaultZipCode;
        formControls["Zipcode"].setValue(this.zipcode);
        this.GetListOfLocations(
          (this.zipcode).toString());


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
              var lstKeysDuplicates = ["IMAGES"];
              this.utilityService.SubscribeLocalListWithDupNames(
                this.languageTranslationService,
                lstKeysDuplicates
              );
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
          this.orderInfo.lstScheduleSlots = values;
          this.orderInfo.DefaultZipCode = formControls["Zipcode"].value.toString();
          this.orderInfo.LocationDetail.LocationID = this.locationID;
          this.orderFlowService.setOrderInfo(this.orderInfo);
          this.orderFlowService.IncrementStep();
          this.router.navigate(["createOrder/Orderflowpackages"]);
        }
      });
    }
  }

  goBack() {
    this.router.navigate(["createOrder"]);
  }
  openModal(template: TemplateRef<any>, locationId: number, locationName: String) {
    this.locationID=locationId;
    this.locationName=locationName;
    this.orderFlowService.getLocationImages(locationId).subscribe(s => {
      console.log(s);
      if (s != -1) {
        this.Locations = s;
        this.IsImageError=false;
      }
      else
      {
        this.IsImageError=true;
        this.Locations=null;
      }
    
      this.modalRef = this.modalService.show(template);
    });

  }
  refreshModel()
  {
    this.orderFlowService.getLocationImages(this.locationID).subscribe(s => {
      console.log(s);
      if (s != -1) {
        this.Locations = s;
        this.locationName = this.locationName;
        this.IsImageError=false;
      }
      else
      {
        this.IsImageError=true;
        this.Locations=null;
      }
  
    });
  }
}
