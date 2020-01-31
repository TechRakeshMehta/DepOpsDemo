import { Component, OnInit } from "@angular/core";
import {
  FormGroup,
  FormBuilder,
  FormControl,
  Validators
} from "@angular/forms";
import { Router } from "@angular/router";
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { OrderInfo,EventSlots,ScheduleSlots } from "../../../models/order-flow/order.model";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { KeyedCollection } from "../../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
import { basename } from "path";
import { CommonService } from '../../../services/shared-services/common.service';
import { AppConsts } from '../../../../environments/constants/appConstants';
import { DatePipe } from "@angular/common";
import { UtilityService } from "../../../services/shared-services/utility.service";



@Component({
  selector: "event-detail-confirm",
  templateUrl: "event-detail-confirm.component.html"
})
export class EventDetailConfirmComponent implements OnInit {
  EventDetailconfirmForm = this.formBuilder.group({
    StartDate: ["", Validators.required],
    EventDescription: ["", Validators.required],
    Slots: [""]
  });

  lstScheduleSlots: Array<ScheduleSlots>=[];
  orderInfo: OrderInfo;
  StartDateTime: string;
  EventDescription: string = "";
  dateSlotInSpanish:string;
  ErrorMessage: string = "";
  IsSlotSelected: boolean=true;
  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );
  selectedSlotID: number = 0;
  SelectedStartTime: Date;
  SelectedEndTime: Date;
  ValidationMsgsObservable = this.dataSource.asObservable();
  ErrorCustMessage: string = "";
  hideDateSlotInSpanish:boolean=true;
  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private orderFlowService: OrderFlowService,
    private datepipe: DatePipe,
    private languageTranslationService: LanguageTranslationService,
    private utilityService: UtilityService,
    private commonService: CommonService
  ) {}



  setLocalization() {

    var lstKeys = ["PREVIOUS","NEXT","CREATODR","EVNTSTRTDATE","EVNTNAMENDES","STEP","SELECTED","OF", "AVAILABLE", "NOTAVAILABLE", "CREATODR", "STEP",
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
    if (
       this.orderInfo.LocationDetail != undefined 
    && this.orderInfo.LocationDetail.EventCode != undefined
     ) {
      this.populateFormControls();
      this.selectedSlotID = this.orderInfo.LocationDetail.SlotID;
      this.SelectedStartTime = this.orderInfo.LocationDetail.StartTime;
      this.SelectedEndTime = this.orderInfo.LocationDetail.EndTime;
     
      this.lstScheduleSlots = this.orderInfo.lstScheduleSlots;
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

  populateFormControls() {
    this.EventDescription = this.orderInfo.LocationDetail.EventName + ',' + this.orderInfo.LocationDetail.EventDescription;
    this.StartDateTime = this.orderInfo.LocationDetail.SlotDate;
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
          this.orderInfo.LocationDetail.IsEventCode = true;
          this.orderInfo.LocationDetail.IsLocationServiceTenant = true;
          this.orderFlowService.setOrderInfo(this.orderInfo);
          this.orderFlowService.IncrementStep();

          this.router.navigate(["createOrder/Orderflowpackages"]);
        } else {
 
          this.ErrorMessage = values.ErrorMessage;
        }
      });
    }
    else { this.IsSlotSelected = false }
    
  }

  goBack() {
    this.router.navigate(["createOrder/eventdetails"]);
  }
}
