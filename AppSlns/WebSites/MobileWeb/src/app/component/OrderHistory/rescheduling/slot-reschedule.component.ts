import { Component, OnInit, OnChanges, DoCheck } from '@angular/core';
import { OrderInfo, ScheduleSlots, OrderDetail, RescheduleAppointmentInfo } from '../../../models/order-flow/order.model';
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { Router } from "@angular/router";
//import { Time } from '@angular/common';
import { FormBuilder, FormControl } from "@angular/forms";
import { Validators } from "@angular/forms";
import { UtilityService } from "../../../services/shared-services/utility.service";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { KeyedCollection } from "../../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
//import { basename } from "path";
import { CommonService } from '../../../services/shared-services/common.service';
import { AppConsts } from '../../../../environments/constants/appConstants';

@Component({
  selector: 'slot-reschedule',
  templateUrl: './slot-reschedule.component.html'
})
export class SlotRescheduleComponent implements OnInit {
  lstScheduleSlots: Array<ScheduleSlots> = [];
  orderDetail: OrderDetail;
  orderInfo: OrderInfo;
  selectedSlotID: number = 0;
  SelectedStartTime: Date;
  SelectedEndTime: Date;
  ErrorMessage: string = "";
  IsSlotSelected: boolean = true;
  ErrorCustMessage: string = "";
  EventDescription: string = "";
  hideDateSlotInSpanish: boolean = true;
  hideEventDetails: boolean = true;
  dateSlotInSpanish: string;
  isSendForOnline: boolean = false;
  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );
  private rescheduleAppointmentInfo: RescheduleAppointmentInfo;
  ValidationMsgsObservable = this.dataSource.asObservable();
  scheudleslot = this.formBuilder.group({
    Slots: [""]
  });
  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private languageTranslationService: LanguageTranslationService,
    private utilityService: UtilityService,
    private commonService: CommonService,
    private orderFlowService: OrderFlowService
  ) { }

  setLocalization() {
    var lstKeys = ["SELECTED", "OF", "AVAILABLE", "NOTAVAILABLE", "RSCHDLAPPNMNT", "EVNTNAMENDES", "EVNTSTRTDTNTM",
      "AVLBLSLOTS", "FOR", "SLCTEDDATETIME", "SLCTAPPOINTMENT", "PREVIOUS", "SUBMIT",
      "CNFMCNLRESCHEDULING", "CNCLRESCHEDULING", "LSTITMY", "LSTITMN", "CNCL","SLOTEXPINSTRUCTION"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );

    var lstValidationKeys = ["SELSLOTNOLNGRSELSANTHR"];
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
    this.ErrorCustMessage = validationMsgs["SELSLOTNOLNGRSELSANTHR"];
  }

  ngOnInit() {    
    this.orderInfo = this.orderFlowService.getAlreadyPlacedOrder();
    this.isSendForOnline = this.orderFlowService.isSendForOnline;
    if (this.orderInfo.LocationDetail != undefined && this.orderInfo.LocationDetail.SlotDate != '') {
      this.selectedSlotID = this.orderInfo.LocationDetail.SlotID;
      this.SelectedStartTime = this.orderInfo.LocationDetail.StartTime;
      this.SelectedEndTime = this.orderInfo.LocationDetail.EndTime;
      this.lstScheduleSlots = this.orderInfo.lstScheduleSlots.filter(x => x.SlotDate.toString() == (this.orderInfo.LocationDetail.SlotDate + 'T00:00:00'));
      if (this.orderInfo.LocationDetail.IsEventCode) {
        this.hideEventDetails = false;
        this.EventDescription = this.orderInfo.LocationDetail.EventName + ',' + this.orderInfo.LocationDetail.EventDescription;
      }
    }
    this.setLocalization();
    this.orderFlowService.GetLocalizedDateFormat(this.orderInfo.LocationDetail.SlotDate, AppConsts.SpanishCode).subscribe(value => {
      this.dateSlotInSpanish = value;
    });
    this.hideDateSlotInSpanish = this.languageTranslationService.GetLanguageParams().SelectedLangCode == AppConsts.EnglishCode;
    this.commonService.isLanguageSpanish.subscribe(isSpanishLang => {
      this.hideDateSlotInSpanish = !isSpanishLang
    });
  }

  onslotselected(selectedslotID: number, StartDateTime: Date, EndDateTime: Date) {
    this.selectedSlotID = selectedslotID;
    this.SelectedStartTime = (StartDateTime);
    this.SelectedEndTime = (EndDateTime);
  }

  onSubmit() {
    if (this.selectedSlotID != 0) {
      this.orderInfo.LocationDetail.StartTime = this.SelectedStartTime;
      this.orderInfo.LocationDetail.EndTime = this.SelectedEndTime;
      this.orderInfo.LocationDetail.SlotID = this.selectedSlotID;
      this.rescheduleAppointmentInfo = new RescheduleAppointmentInfo();
      this.rescheduleAppointmentInfo.orderDetail = this.orderFlowService.getSelectedOrderDetail();
      this.rescheduleAppointmentInfo.locationDetail = this.orderInfo.LocationDetail;      
      if (this.isSendForOnline) {
        this.orderFlowService.ReserveSlot(this.orderInfo.LocationDetail).subscribe(values => {
          if (!values.HasError) {
            var data = values.ResponseObject;
            this.orderInfo.LocationDetail.ReserverSlotID = data;
            //this.orderInfo.LocationDetail.IsEventCode = false;
            this.orderInfo.LocationDetail.IsLocationServiceTenant = true;
            this.orderFlowService.setOrderInfo(this.orderInfo);
            this.router.navigate(["createOrder/PaymentDetail"]);
          } else {
            this.ErrorMessage = values.ErrorMessage;
          }
        });
      }
      else {
        this.orderFlowService.RescheduleOrder(this.rescheduleAppointmentInfo, this.languageTranslationService
          .GetLanguageParams().SelectedLangCode).subscribe(values => {
            if (!values.HasError) {
              this.orderFlowService.SuccessMsg = values.Message;
              this.orderFlowService.setOrderDetail(new Array<OrderDetail>());
              this.orderFlowService.setAlreadyPlacedOrderInfoToEmpty();
              this.router.navigate(["OrderHistory"]);
            } else {
              this.orderFlowService.setAlreadyPlacedOrderInfoToEmpty();
              this.ErrorMessage = values.ErrorMessage;
            }
          });
      }

    }
    else { this.IsSlotSelected = false }
  }
  goBack() {
    if (this.orderInfo.LocationDetail.IsEventCode) {
      this.router.navigate(["orderDetail/eventReschedule"]);

    } else {
      this.router.navigate(["orderDetail/calenderReschedule"]);
    }

  }
}
