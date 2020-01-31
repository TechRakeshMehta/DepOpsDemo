import { Component, OnInit } from "@angular/core";
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { Router } from "@angular/router";
import {
  ApplicantOrderPaymentOptions,
  OrderInfo
} from "../../../models/order-flow/order.model";
import { UtilityService } from "../../../services/shared-services/utility.service";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { CommonService } from "../../../services/shared-services/common.service";
import { AppConsts } from "../../../../environments/constants/appConstants";
import { DatePipe } from '@angular/common';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ModalContentComponent } from "../../confirmation-box/confirmation-modal.component";


@Component({
  selector: "app-purchase-detail",
  templateUrl: "./purchase-detail.component.html",
  styleUrls: ["./purchase-detail.component.css"]
})
export class PurchaseDetailComponent implements OnInit {
  hierarchyNodeID: number;
  IsDisabled: boolean;
  IsCreditCardSelected: boolean;
  lstPayments = new Array<any>();
  lstInstructions = new Array<any>();
  showAgreement: boolean = true;
  InstructionText: string;
  AggrementText: String;
  CreditCard: String = "PTCC";
  TotalPrice: string = "";
  PackageName: string = "";
  IsAgreementChecked: boolean = false;
  bkgPkgHierarchyMappingID: string = "";
  packageId: number;
  orderInfo: OrderInfo;
  poid: number;
  PaymentOptionName: string = "";
  PaymentByInstAlone: boolean = false;
  PaymentByInst: boolean = false;
  PricePaidByInstitution: string;
  BalanceAmount: string;
  HidePaymentByInst: boolean = true;
  HideBalAmount: boolean = true;
  HidePaymentType: boolean = false;
  InstructionTextPaidByInst: string;
  poidPaidByInst: number;
  PaymentOptionNamePaidByInst: string;
  HideInstructionText: boolean = false;
  HideInstructionTextPaidByInst: boolean = false;
  isSendForOnline: boolean = false;
  constructor(
    private orderFlowService: OrderFlowService,
    private router: Router,
    private languageTranslationService: LanguageTranslationService,
    private utilityService: UtilityService,
    private commonService: CommonService,
    private datepipe: DatePipe, private modalService: BsModalService,

  ) {
    this.modalService.onHide.subscribe((result: any) => {
      if (document.getElementById('OK') != undefined) {
        if (result === document.getElementById('OK').textContent) {
          this.confirmPopUpClick();
        }
      } { }
    });
  }

  ngOnInit() {
    this.orderInfo = this.orderFlowService.getOrderInfo();
    this.isSendForOnline = this.orderFlowService.isSendForOnline;
    this.orderInfo.CurrentStep = this.orderInfo.TotalSteps - 1;
    this.TotalPrice = this.orderInfo.GrandTotal;
    this.PackageName = this.orderInfo.packageName;
    this.bkgPkgHierarchyMappingID = this.orderInfo.bkgPkgHierarchyMappingID.toString();
    this.packageId = this.orderInfo.bkgPackageId;
    this.hierarchyNodeID = this.orderInfo.SelectedHierarchyNodeID;
    var isBillingCode = false;
    var billingCodeAmt = "0";
    this.PaymentByInstAlone = this.orderInfo.IsPaymentByInstAlone;
    this.PaymentByInst = this.orderInfo.IsPaymentByInst;
    if (this.orderInfo.BillingCodeAmount != undefined && this.orderInfo.BillingCodeAmount != null && this.orderInfo.BillingCodeAmount != "") {
      if (this.PaymentByInstAlone) {
        this.HidePaymentType = true;
      }
      this.HidePaymentByInst = false;
      this.HideBalAmount = false;
      if (this.orderInfo.GrandTotal <= this.orderInfo.BillingCodeAmount) {
        this.PricePaidByInstitution = this.orderInfo.GrandTotal;
        this.BalanceAmount = "0";
      }
      else {
        this.PricePaidByInstitution = this.orderInfo.BillingCodeAmount;
        var balAmount = parseFloat(this.orderInfo.GrandTotal) - parseFloat(this.orderInfo.BillingCodeAmount);
        this.BalanceAmount = balAmount.toString();
      }
    }
    if (this.orderInfo.CbiBillingCode != undefined && this.orderInfo.CbiBillingCode != "") {
      isBillingCode = true;
      billingCodeAmt = this.orderInfo.BillingCodeAmount;
    }

    this.orderFlowService.getPaymentOptions(0, this.bkgPkgHierarchyMappingID, this.hierarchyNodeID, isBillingCode)
      .subscribe(s => {

        this.lstPayments = s[0].lstPaymentOptions;
        this.poid = this.lstPayments[0].PaymentOptionId;
        this.PaymentOptionName = this.lstPayments[0].PaymentOptionName;
        if (this.orderInfo.CbiBillingCode != "" && this.orderInfo.CbiBillingCode != null) {
          if (isBillingCode && billingCodeAmt != "" && billingCodeAmt != null) {
            this.poidPaidByInst = this.lstPayments.find(x => x.PaymentOptionCode == "PTINA").PaymentOptionId;
            // this.PaymentOptionNamePaidByInst =this.lstPayments.find(x=>x.PaymentOptionCode=="PTINA").PaymentOptionName;
            // //this.InstructionTextPaidByInst=this.lstPayments.find(x=>x.PaymentOptionCode=="PTINA").PaymentOptionInstructionText;
            var itemToRemove = this.lstPayments.find(x => x.PaymentOptionCode == "PTINA");
            var indexToRemove = this.lstPayments.indexOf(itemToRemove);
            this.lstPayments.splice(indexToRemove, 1);
          }
        }

        this.poid = this.lstPayments[0].PaymentOptionId;
        this.PaymentOptionName = this.lstPayments[0].PaymentOptionName;
        this.orderFlowService.getInstructionTexts(billingCodeAmt, isBillingCode).subscribe(s => {
          this.lstInstructions = s;
          this.BindInstruction(this.poid);
        });
      });
    var lstKeys = ["TOTALPRICE", "OF", "ORDERSELECTION", "PRCHSDTL", "PAYMENTTYPE", "USERAGRMNT", "PREVIOUS", "NEXT",
      "HAVEREADUSERAGRMNT", "CREATODR", "STEP", "CNFMCNLORDER", "PAYMENTINST", "CNCLORDR", "LSTITMY", "LSTITMN", "CNCL", 
      "PAIDBYINST", "BALANCEAMT","CNCL", "CONFIRMMSG", "OK","SLOTEXPALRTMSG"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
    var lstKeysDuplicates = ["PAYMENTINST"];
    this.utilityService.SubscribeLocalListWithDupNames(
      this.languageTranslationService,
      lstKeysDuplicates
    );
    // this.orderFlowService.getAgreementText(this.languageTranslationService.GetLanguageParams().SelectedLangCode).subscribe(result => {
    //   this.AggrementText = result;
    // });
    if (this.commonService.isLanguageSpanish) {
      this.commonService.isLanguageSpanish.subscribe(isSpanishLang => {
        this.setLocalizationFields(isSpanishLang);
      });
    }
    document.getElementById("SLOTEXPALRTMSG").style.display = 'none';
    document.getElementById("OK").style.display = 'none';
    document.getElementById("LSTITMN").style.display = 'none';
  }

  setLocalizationFields(isSpanishLanguage: boolean) {
    if (isSpanishLanguage) {
      this.orderFlowService.getAgreementText(AppConsts.SpanishCode).subscribe(data => {
        this.AggrementText = data
      });
    }
    else {
      this.orderFlowService.getAgreementText(AppConsts.EnglishCode).subscribe(data => {
        this.AggrementText = data
      });
    }

  }

  onChange(paymentId: number) {
    this.BindInstruction(paymentId);
    this.poid = paymentId;
    this.PaymentOptionName = this.lstPayments.find(x => x.PaymentOptionId == this.poid).PaymentOptionName;

  }
  CheckBoxEvent(event: any) {
    if (event.target.checked) {
      this.IsDisabled = false;
      this.IsAgreementChecked = true;
    }
    else {
      this.IsDisabled = true;
      this.IsAgreementChecked = false;
    }

  }
  BindInstruction(paymentId: number) {
    var recordCreditCard = this.lstInstructions.find(
      x => x.PaymentOptionID == paymentId
    );
    if (recordCreditCard != undefined && recordCreditCard != "" && recordCreditCard != null) {
      if (recordCreditCard.InstructionText != null && recordCreditCard.InstructionText != undefined && recordCreditCard.InstructionText != "") {
        this.InstructionText = recordCreditCard.InstructionText;
        this.HideInstructionText = false;
      }
    }
    else {
      this.HideInstructionText = true;
    }
    if (
      this.CreditCard ==
      this.lstPayments.find(x => x.PaymentOptionId == paymentId)
        .PaymentOptionCode
    ) {
      this.showAgreement = true;
      this.IsCreditCardSelected = true;
      if (this.IsAgreementChecked || this.PaymentByInstAlone) {
        this.IsDisabled = false;
      }
      else {
        this.IsDisabled = true;
      }

    } else {
      this.showAgreement = false;
      this.IsCreditCardSelected = false;
      this.IsDisabled = false;
    }
    if (this.orderInfo.BillingCodeAmount != undefined && this.orderInfo.BillingCodeAmount != null && this.orderInfo.BillingCodeAmount != "") {
      var record = this.lstInstructions.find(
        x => x.Code == "PTINA"
      );
      if (record != undefined && record != "") {
        if (record.InstructionText != null && record.InstructionText != undefined && record.InstructionText != "") {
          this.InstructionTextPaidByInst = record.InstructionText;
          this.HideInstructionTextPaidByInst = false;
        }
        this.PaymentOptionNamePaidByInst = this.lstInstructions.find(x => x.Code == "PTINA").Name;
      }
      else {
        this.HideInstructionTextPaidByInst = true;
      }
    }
  }

  goBack() {
    if (this.isSendForOnline == true) { 
      switch (this.orderInfo.selectedOrderType) {
        case "1":
          this.router.navigate(["orderDetail/scheduleSlots"]);
          this.orderFlowService.setAlreadyPlacedOrder(this.orderInfo);
          break;
        case "2":
          this.router.navigate(["orderDetail/scheduleSlots"]);
          this.orderFlowService.setAlreadyPlacedOrder(this.orderInfo);
          break;
        case "3":
          this.router.navigate(["createOrder/OutOfState"]);
          this.orderInfo.TotalSteps = 7;
          this.orderFlowService.setOrderInfo(this.orderInfo);
          break;
        default:
      }
    }
    else {
      this.orderFlowService.DecrementStep();
      this.router.navigate(["createOrder/OrderReview"]);
    }
  }
  

  confirmPopUpClick() {
    let stepUrl ="";
    if(!this.orderFlowService.isOrderAlreadyPlaced)
    {
       stepUrl = "createOrder/EventDetailConfirm";
      if(this.orderInfo.LocationDetail.IsEventCode){
        this.orderInfo.CurrentStep = 1;
        stepUrl = "createOrder/EventDetailConfirm";
      }
      else{
        stepUrl = "createOrder/scheduleslot";
        this.orderInfo.CurrentStep = 5;
      } 
    }
    else
    {
      if(this.orderInfo.LocationDetail.IsEventCode){
        stepUrl = "/orderDetail/scheduleSlots";
      }
      else{
        stepUrl = "/orderDetail/calenderReschedule";
      } 

    }
    
    this.orderFlowService.setOrderInfo(this.orderInfo);
    this.router.navigate([stepUrl]);   
  }
  IsReservedSlotExpired(){
    var result = true;
    if(!this.orderInfo.LocationDetail.IsOutOfState){
      this.orderFlowService.IsReservedSlotExpired(this.orderInfo.LocationDetail.ReserverSlotID,false)
      .then(result=>{
        if(result === true){
        const initialState = {
          message: document.getElementById('SLOTEXPALRTMSG').textContent,
          title: '',
          confirmBtnName: document.getElementById('OK').textContent,
          hideCloseBtn:true
        };
        this.modalService.show(ModalContentComponent, { initialState });
        }
        else{
          this.onSubmit();
        }
      })
    }
    else{
      this.onSubmit();
    }
    return result;
  }

  result: OrderInfo;
  onSubmit() {
    this.orderInfo.selectedPaymentModeData=new Array<ApplicantOrderPaymentOptions>();
    if (this.orderInfo.BillingCodeAmount != undefined && this.orderInfo.BillingCodeAmount != null && this.orderInfo.BillingCodeAmount != "") {
      if (this.orderInfo.GrandTotal <= this.orderInfo.BillingCodeAmount) {
        var paymentDetail = new ApplicantOrderPaymentOptions();
        paymentDetail.isbkg = true;
        paymentDetail.pkgid = this.orderInfo.bkgPackageId;
        paymentDetail.poid = this.poidPaidByInst;
        paymentDetail.isZP = false;
        this.orderFlowService.SetSelectedPaymentModeData(paymentDetail);
      }
      else {
        var paymentDetail = new ApplicantOrderPaymentOptions();
        paymentDetail.isbkg = true;
        paymentDetail.pkgid = this.orderInfo.bkgPackageId;
        paymentDetail.poid = this.poidPaidByInst;
        paymentDetail.isZP = false;
        this.orderFlowService.SetSelectedPaymentModeData(paymentDetail);
        var paymentDetail = new ApplicantOrderPaymentOptions();
        paymentDetail.isbkg = true;
        paymentDetail.pkgid = this.orderInfo.bkgPackageId;
        paymentDetail.poid = this.poid;
        paymentDetail.isZP = false;
        this.orderFlowService.SetSelectedPaymentModeData(paymentDetail);
      }
    }
    else {
      var paymentDetail = new ApplicantOrderPaymentOptions();
      paymentDetail.isbkg = true;
      paymentDetail.pkgid = this.orderInfo.bkgPackageId;
      paymentDetail.poid = this.poid;
      paymentDetail.isZP = false;
      this.orderFlowService.SetSelectedPaymentModeData(paymentDetail);
    }
    this.orderInfo.PaymentInstructions = this.InstructionText;
    this.orderInfo.PaymentType = this.PaymentOptionName;
    this.orderFlowService.setOrderInfo(this.orderInfo);
    if (this.isSendForOnline == true) {
      this.orderFlowService
        .CompleteApplicantOrder(this.orderFlowService.getOrderInfo())
        .then(value => {
          if (
            this.CreditCard ==
            this.lstPayments.find(x => x.PaymentOptionId == this.poid)
              .PaymentOptionCode && !this.PaymentByInstAlone
          )
            this.router.navigate(["createOrder/CardSelection"]);
          else {
            this.router.navigate(["createOrder/OrderSummary"]);
          }
        });
    }
    else {
      this.orderFlowService
        .SubmitOrder(this.orderFlowService.getOrderInfo())
        .then(value => {
          if (
            this.CreditCard ==
            this.lstPayments.find(x => x.PaymentOptionId == this.poid)
              .PaymentOptionCode && !this.PaymentByInstAlone
          )
            this.router.navigate(["createOrder/CardSelection"]);
          else {
            this.router.navigate(["createOrder/OrderSummary"]);
          }
        });
    }

  }
}
