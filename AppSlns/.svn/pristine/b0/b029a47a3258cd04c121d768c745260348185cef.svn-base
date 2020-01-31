import { Component, OnInit } from "@angular/core";
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { OrderInfo } from "../../../models/order-flow/order.model";
import {
  UserInfo,
  PersonAliasContract,
  AddressInfo
} from "../../../models/user/user.model";
import { AttributesForCustomForm, PaymentTypeDetails } from "../../../models/custom-forms/custom-attribute";
import { Router } from "@angular/router";
import { UtilityService } from "../../../services/shared-services/utility.service";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { CommonService } from "../../../services/shared-services/common.service";
import { LanguageParams } from "../../../models/language-translation/language-translation-contract.model";
import { AppConsts } from "../../../../environments/constants/appConstants";
import { conformToMask } from "angular2-text-mask";
import { StorageService } from "../../../services/shared-services/storage.service";

@Component({
  selector: "app-order-summary",
  templateUrl: "./order-summary.component.html",
  styleUrls: ["./order-summary.component.css"]
})
export class OrderSummaryComponent implements OnInit {
  public mask = ['(', /[0-9]/, /\d/, /\d/, ')', '-', /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/]
  PrimaryPhone: string;
  orderInfo: OrderInfo;
  userInfo: UserInfo;
  AggrementText: String;
  showAgreement: boolean = false;
  IsZipCodeLabel: boolean = true;
  addressInfo: AddressInfo;
  lstCustomAttribute: Array<AttributesForCustomForm>;
  lstOrderInfo: Array<OrderInfo>;
  InstructionText: string;
  languageParams: LanguageParams;
  lstCustomAttributeGroup: Array<{
    GroupName: string;
    Attributes: Array<AttributesForCustomForm>;
  }>;
  lstPayments: Array<PaymentTypeDetails>;
  HideBalAmount: boolean = true;
  CreditCard: string = "PTCC";
  isAlreadyPlacedOrder: boolean;
  isSendForOnline: boolean;

  constructor(private orderFlowService: OrderFlowService,
    private router: Router, private languageTranslationService: LanguageTranslationService,
    private commonService: CommonService,
    private utilityService: UtilityService,
    private storageService: StorageService) { }
  HideSSN: boolean = false;
  HideState: Boolean = false;
  SSNMaskedText: string = "";
  setCustomFormsLocalization(isSpanishLanguage: boolean) {

    this.lstCustomAttributeGroup = new Array<{ GroupName: string, Attributes: Array<AttributesForCustomForm> }>();
    if (isSpanishLanguage) {
      this.orderInfo.lstCustomAttributeInSpanish.forEach(element => {
        var res = this.lstCustomAttributeGroup.find(z => z.GroupName == element.SectionTitle);
        if (res == null) {
          this.lstCustomAttributeGroup.push({ 'GroupName': element.SectionTitle, 'Attributes': this.orderInfo.lstCustomAttributeInSpanish.filter(x => x.AttributeGroupId == element.AttributeGroupId && !x.IsHiddenFromUI) });
        }
      });
      this.orderFlowService.getAgreementText(AppConsts.SpanishCode).subscribe(data => {
        this.AggrementText = data
      });
    }
    else {
      this.lstCustomAttribute.forEach(element => {
        var res = this.lstCustomAttributeGroup.find(z => z.GroupName == element.SectionTitle);
        if (res == null) {
          this.lstCustomAttributeGroup.push({ 'GroupName': element.SectionTitle, 'Attributes': this.lstCustomAttribute.filter(x => x.AttributeGroupId == element.AttributeGroupId && !x.IsHiddenFromUI) });
        }
      });
      this.orderFlowService.getAgreementText(AppConsts.EnglishCode).subscribe(data => {
        this.AggrementText = data
      });
    }

  }
  isOrderPlacedAlready() {

    var alreadyPlacedOrderStorage = this.storageService.GetItem(
      AppConsts.IsAlreadyPlacedOrder
    );
    this.storageService.SetItem(AppConsts.IsAlreadyPlacedOrder, "");
    if (
      alreadyPlacedOrderStorage != undefined &&
      alreadyPlacedOrderStorage != ""
    ) {
      return alreadyPlacedOrderStorage == AppConsts.TrueLiteral;
    }
    else
      return false;
  }
  isSendForOnlineOrder() {

    var alreadyPlacedOrderStorage = this.storageService.GetItem(
      AppConsts.IsSendForOnline
    );
    this.storageService.SetItem(AppConsts.IsSendForOnline, "");
    if (
      alreadyPlacedOrderStorage != undefined &&
      alreadyPlacedOrderStorage != ""
    ) {
      return alreadyPlacedOrderStorage == AppConsts.TrueLiteral;
    }
    else
      return false;
  }
  ngOnInit() {
    //Disable Browser's back button code starts
    history.pushState(null, null, document.URL);
    window.addEventListener('popstate', function () {
      history.pushState(null, null, document.URL);
    });
    //Disable Browser's back button code ends

    //this.userInfo = this.orderInfo.userInfo;
    var lstKeys = ["THANKSORDERCONFIRMD","COMPLETEORDER", "ORDERSUMMARY", "ORDSELDTLS", "ODRNUMBER",
      "TOTALPRICE", "ORDRDTLS", "ORDSELDTLS", "AMOUNT", "PROFILEDETAILS", "FIRSTNAME", "MIDDLENAME", "OF",
      "LASTNAME", "GENDER", "DOB",
      , "EMAILADD", "PHONENUM", "ADDRESS", "CITY", "STATE", "GOTODASHBOARD", "PRINT", "CREATODR", "STEP", "USERAGRMNT", "BALANCEAMT", "PAYMENTBYINST", "PAIDBYINST"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
    var temporderInfo: any;
    this.isAlreadyPlacedOrder = this.isOrderPlacedAlready();
    this.isSendForOnline = this.isSendForOnlineOrder();
    if (this.isAlreadyPlacedOrder) {
      temporderInfo = this.orderFlowService.getAlreadyPlacedOrder();
    }
    else {
      temporderInfo = this.orderFlowService.getOrderInfo();
    }
    this.languageParams = this.languageTranslationService.GetLanguageParams();
    this.orderFlowService.GetOrderNumberDetails(temporderInfo.OrderNumber).subscribe(ord => {
      this.orderInfo = ord;
      this.lstPayments = this.orderInfo.lstPaymentDataDetail;
      if (this.lstPayments.length > 1) {
        this.HideBalAmount = false;
      }
      var lstKeysDuplicates = ["ORDSELDTLS", "PAYMENTTYPE", "SSN", "COUNTRY", "PAYMENTTYPES", "ZIPCODE", "POSTALCODE", "PAYMENTINST", "BALANCEAMT", "AMOUNT"];
      this.utilityService.SubscribeLocalListWithDupNames(
        this.languageTranslationService,
        lstKeysDuplicates
      );
      if (this.isAlreadyPlacedOrder) {
        this.orderInfo.CurrentStep = 1;
        this.orderInfo.TotalSteps = 1;
      }
      else {
        this.orderInfo.CurrentStep = temporderInfo.TotalSteps;
        this.orderInfo.TotalSteps = temporderInfo.TotalSteps;
        this.orderInfo.CbiBillingCode = temporderInfo.CbiBillingCode;
      }
      this.userInfo = this.orderInfo.userInfo;

      var primaryPhone = conformToMask(
        this.userInfo.PrimaryPhone,
        this.mask,
        { guide: false }
      )
      this.PrimaryPhone = primaryPhone.conformedValue;

      var totalPrice = 0;
      for (var i = (this.orderInfo.lstPaymentDataDetail.length) - 1; i >= 0; i--) {
        totalPrice += parseFloat(this.orderInfo.lstPaymentDataDetail[i].Amount);
      }
      this.orderInfo.GrandTotal = totalPrice.toString();
      this.lstCustomAttributeGroup = Array<{ GroupName: string, Attributes: Array<AttributesForCustomForm> }>();
      this.lstCustomAttribute = this.orderInfo.lstCustomAttribute;

      if (this.commonService.isLanguageSpanish) {
        this.commonService.isLanguageSpanish.subscribe(isSpanishLang => {
          this.setCustomFormsLocalization(isSpanishLang);
        });
      }


      if (this.userInfo.CountryName == "CANADA" ||
        this.userInfo.CountryName == "UNITED STATES of AMERICA" ||
        this.userInfo.CountryName == "UNITED STATES of AMERICA - STATE" ||
        this.userInfo.CountryName == "MEXICO"

      ) {
        this.HideState = false;
        this.IsZipCodeLabel = true;
      }
      else {
        this.HideState = true;
        this.IsZipCodeLabel = false;
      }
      if (this.userInfo.SSN == "") {
        this.HideSSN = true;
      }
      else {
        this.SSNMaskedText = "###-##-" + this.userInfo.SSN.substr((this.userInfo.SSN.length) - 4, this.userInfo.SSN.length);
      }
      this.addressInfo = this.userInfo.AddressInfo;

      if (this.lstPayments.filter(x => x.PaymentTypeCode == "PTCC").length > 0) {
        this.showAgreement = true;
      }

      this.getSummaryHTML();
    });
  }

  goBackToDashboard() {
    this.orderFlowService.setOrderInfoToEmpty();
    this.orderFlowService.setAlreadyPlacedOrderInfoToEmpty();
    this.router.navigate(["userDashboard"]);
  }

  ngAfterViewInit() {
    //this.getSummaryHTML();
  }
  getSummaryHTML() {
    this.orderInfo.LanguageCode = this.languageTranslationService.GetLanguageParams().SelectedLangCode;
    this.orderFlowService.CreateReceiptFile(this.orderInfo)
      .subscribe(value => {
      });
  }

  downloadOrderReceipt() {
    this.orderFlowService.DownloadOrderReceipt(this.orderInfo.OrderNumber)
      .subscribe(value => {
        if (value == -1) {
          return;

        }

        var orderReceiptRawData = atob(value.DocumentByte);
        var orderReceiptName = value.FileName;
        var rawDataLength = orderReceiptRawData.length;

        var receiptData_Array = new Uint8Array(new ArrayBuffer(rawDataLength));

        for (var i = 0; i < rawDataLength; i++) {
          receiptData_Array[i] = orderReceiptRawData.charCodeAt(i);
        }

        var blob = new Blob([receiptData_Array], { type: 'application/pdf' });
        var orderReceiptURL = window.URL.createObjectURL(blob);

        //Create anchor element to download
        var linkElement = document.createElement('a');

        linkElement.setAttribute('href', orderReceiptURL);
        linkElement.setAttribute("download", orderReceiptName);
        linkElement.setAttribute("target", "_blank");
        window.open(orderReceiptURL, '_blank');
      });
  }
}
