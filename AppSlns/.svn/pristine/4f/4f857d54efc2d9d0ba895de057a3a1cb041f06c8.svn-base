import { Component, OnInit, OnChanges, DoCheck } from '@angular/core';
import { OrderInfo, ScheduleSlots } from '../../../models/order-flow/order.model';
import { OrderFlowService } from '../../../services/order-flow/order-flow.service';
import { Router } from "@angular/router";
import { Time } from '@angular/common';
import { FormBuilder, FormControl } from "@angular/forms";
import { Validators } from "@angular/forms";
import { UtilityService } from "../../../services/shared-services/utility.service";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { KeyedCollection } from "../../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
import { basename } from "path";
import { CommonService } from '../../../services/shared-services/common.service';
import { AppConsts } from '../../../../environments/constants/appConstants';

@Component({
  selector: 'app-scheduleslot',
  templateUrl: './scheduleslot.component.html',
  styleUrls: ['./scheduleslot.component.css']
})
export class ScheduleslotComponent implements OnInit {
  lstScheduleSlots: Array<ScheduleSlots> = [];
  orderInfo: OrderInfo;
  selectedSlotID: number = 0;
  SelectedStartTime: Date;
  SelectedEndTime: Date;
  ErrorMessage: string = "";
  IsSlotSelected: boolean=true;
  ErrorCustMessage: string = "";
  locale: string = 'es';
  hideDateSlotInSpanish:boolean=true;
  dateSlotInSpanish:string;
  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );
  ValidationMsgsObservable = this.dataSource.asObservable();
  scheudleslot = this.formBuilder.group({
    Slots: [""]
  });
  constructor(
    private formBuilder: FormBuilder,
    private orderFlowService: OrderFlowService,
    private router: Router,
    private languageTranslationService: LanguageTranslationService,
    private utilityService: UtilityService,
    private commonService: CommonService
  ) { }

  setLocalization() {

    var lstKeys = ["SELECTED","OF", "AVAILABLE", "NOTAVAILABLE", "CREATODR", "STEP",
      "SCHEDULEAPPOINTMENT", "AVLBLSLOTS","FOR", "SLCTEDDATETIME", "SLCTAPPOINTMENT", "PREVIOUS", "NEXT",
      "CNFMCNLORDER", "CNCLORDR", "LSTITMY", "LSTITMN", "CNCL","SLOTEXPINSTRUCTION"];
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
    this.orderInfo = this.orderFlowService.getOrderInfo();
    if (this.orderInfo.LocationDetail != undefined && this.orderInfo.LocationDetail.SlotDate != '') {
      this.selectedSlotID = this.orderInfo.LocationDetail.SlotID;
      this.SelectedStartTime = this.orderInfo.LocationDetail.StartTime;
      this.SelectedEndTime = this.orderInfo.LocationDetail.EndTime;
      this.lstScheduleSlots = this.orderInfo.lstScheduleSlots.filter(x => x.SlotDate.toString() == (this.orderInfo.LocationDetail.SlotDate + 'T00:00:00'));
    }
    
    this.setLocalization();
    this.orderFlowService.GetLocalizedDateFormat(this.orderInfo.LocationDetail.SlotDate,AppConsts.SpanishCode).subscribe(value=>{
      this.dateSlotInSpanish=value;
    })
    this.hideDateSlotInSpanish= this.languageTranslationService.GetLanguageParams().SelectedLangCode==AppConsts.EnglishCode;
    this.commonService.isLanguageSpanish.subscribe(isSpanishLang => {
        this.hideDateSlotInSpanish=!isSpanishLang
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
      this.orderFlowService.ReserveSlot(this.orderInfo.LocationDetail).subscribe(values => {
        if (!values.HasError) {
          var data = values.ResponseObject;
          this.orderInfo.LocationDetail.ReserverSlotID = data;
          this.orderInfo.LocationDetail.IsEventCode = false;
          this.orderInfo.LocationDetail.IsLocationServiceTenant = true;
          this.orderFlowService.setOrderInfo(this.orderInfo);
          this.orderFlowService.IncrementStep();
          this.router.navigate(["createOrder/OrderReview"]);
        } else {
          this.ErrorMessage = values.ErrorMessage;
        }
      });
    }
    else { this.IsSlotSelected = false }
  }
  goBack() {
    this.router.navigate(["createOrder/schedulecalender"]);
  }
}
