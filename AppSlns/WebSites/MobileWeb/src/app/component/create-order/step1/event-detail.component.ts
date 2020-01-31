import { Component, OnInit } from "@angular/core";
import { FormGroup, FormBuilder, FormControl, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import {
  OrderInfo,
  LocationDetail,
  ScheduleSlots
} from "../../../models/order-flow/order.model";
import { UtilityService } from "../../../services/shared-services/utility.service";
import { DatePipe } from '@angular/common';
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { KeyedCollection } from "../../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
import { Alert } from "selenium-webdriver";

@Component({
  selector: "event-detail",
  templateUrl: "event-detail.component.html"
})
export class EventDetailComponent implements OnInit {
  EventDetailForm = this.formBuilder.group({
    eventCode: ["", Validators.required]
  });

  orderInfo: OrderInfo;
  eventCode: string;
  IsValidEventCode: boolean = true;
 // ErrorMessage: string;
  EventCodeCusMsg:string;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private orderFlowService: OrderFlowService,
    private utilityService: UtilityService,
    private datepipe: DatePipe,
    private languageTranslationService: LanguageTranslationService,
  ) { }

  Event_validation_messages: any;
  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );
  ValidationMsgsObservable = this.dataSource.asObservable();
  setLocalization() {
    var lstKeys = ["PREVIOUS","NEXT","ENTREVENTCODE","CREATODR","STEP"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
    var lstValidationKeys = ["EVENTCODEREQ","PLZENTRVLDEVNTCODE"];
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
    this.Event_validation_messages = {
      eventCode: [
        { type: "required", message: validationMsgs["EVENTCODEREQ"] }
      ]
    };

    //this.EventCodeCusMsg="Please enter the valid Event Code"
    this.EventCodeCusMsg=validationMsgs["PLZENTRVLDEVNTCODE"];
  }
  ngOnInit() {
    this.setLocalization();
    this.orderInfo = this.orderFlowService.getOrderInfo();
    if (
      this.orderInfo.LocationDetail != undefined &&
      this.orderInfo.LocationDetail.EventCode != undefined
    ) {
      this.populateFormControls();
    }
  }

  populateFormControls() {
    this.eventCode = this.orderInfo.LocationDetail.EventCode;
  }
  isError(field: string) {
    return this.utilityService.isError(
      this.EventDetailForm,
      field,
      this.Event_validation_messages[field]
    );
  }
  errorMessage(field: string) {
    return this.utilityService.ErrorMessage(
      this.EventDetailForm,
      field,
      this.Event_validation_messages[field]
    );
  }
  isFieldValid(field: string) {
    return this.utilityService.isFieldValid(this.EventDetailForm, field);
  }

  displayFieldCss(field: string) {
    return this.utilityService.displayFieldCss(this.EventDetailForm, field);
  }
  populateScheduleSlots(data:Array<any>){
    this.orderInfo.lstScheduleSlots=this.orderFlowService.initializelstScheduleSlot();
    data.forEach(element => {
      var result=new  ScheduleSlots();
      result.SlotID=element.SlotID;
      result.SlotDate = element.SlotDate;
      result.StartDateTime=element.StartDateTime;
      result.EndDateTime=element.EndDateTime;
      result.IsAvailable=element.IsAvailable;
      this.orderInfo.lstScheduleSlots.push(result);
    });
  }
  onSubmit() {
    if (this.EventDetailForm.valid) {
      this.orderFlowService
        .validateEventCode(this.eventCode,this.languageTranslationService
          .GetLanguageParams().SelectedLangCode).subscribe(values => {
          if (!values.HasError) {
            this.populateScheduleSlots(values.ResponseObject);
            var data=values.ResponseObject[0];
            this.orderInfo.LocationDetail.LocationID = data.LocationId;
            this.orderInfo.LocationDetail.LocationName = data.LocationName;
            this.orderInfo.LocationDetail.LocationAddress = data.LocationAddress;
            this.orderInfo.LocationDetail.LocationDescription = data.LocationDescription;
            this.orderInfo.LocationDetail.EventName = data.EventName;
            this.orderInfo.LocationDetail.SlotDate = data.SlotDate.replace("T00:00:00","");
            this.orderInfo.LocationDetail.EventCode = this.eventCode;
            this.orderInfo.LocationDetail.EventDescription = data.EventDescription;
            this.orderInfo.LocationDetail.IsEventCode = true;
            this.orderInfo.LocationDetail.IsLocationServiceTenant = true;
            this.orderFlowService.setOrderInfo(this.orderInfo); 
            this.router.navigate(["createOrder/EventDetailConfirm"]);
            
          } else {
            this.IsValidEventCode =false;
           // this.ErrorMessage = values.ErrorMessage;
          }
        });
    } else {
      this.utilityService.validateAllFormFields(this.EventDetailForm);
    }
  }

  goBack() {
    this.router.navigate(["createOrder"]);
  }
}
