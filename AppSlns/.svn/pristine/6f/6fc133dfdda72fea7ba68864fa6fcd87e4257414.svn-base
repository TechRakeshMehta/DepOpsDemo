import { Component, OnInit, TemplateRef } from '@angular/core';
import { OrderDetail, OrderInfo } from '../../models/order-flow/order.model';
import { OrderFlowService } from '../../services/order-flow/order-flow.service';
import { Router } from "@angular/router";
import { UtilityService } from "../../services/shared-services/utility.service";
import { LanguageTranslationService } from "../../services/language-translations/language-translations.service";
import { KeyedCollection } from "../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
import { ModalContentComponent } from "../confirmation-box/confirmation-modal.component";
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { forEach } from '@angular/router/src/utils/collection';
import { setTimeout } from 'timers';
import { CommonService } from '../../services/shared-services/common.service';
import { StorageService } from '../../services/shared-services/storage.service';
import { AppConstant } from '../../models/common/AppConst';
import { AppConsts } from '../../../environments/constants/appConstants';
@Component({
  selector: 'app-order-history',
  templateUrl: './order-history.component.html',
  styleUrls: ['./order-history.component.css']
})
export class OrderHistoryComponent implements OnInit {
  refresh: any;
  refreshMsg: any;
  IsImageError: boolean;
  locationID: number;
  locationName: String;
  NoImgMsg: any;
  Locations: any;
  selectedPaymentOptionId: string;
  orderDetails: Array<OrderDetail>;
  orderInfo: OrderInfo;
  selectedOrder: OrderDetail;
  hdnCnclErrOrderMsg: Boolean = true;
  cnclOrderMsg: string = "";
  ErrorMsg: string = "";
  SuccessMsg: string = "";
  modalRef: BsModalRef;
  constructor(private orderFlowService: OrderFlowService,
    private router: Router, private utilityService: UtilityService
    , private languageTranslationService: LanguageTranslationService
    , private modalService: BsModalService
    , private commonService: CommonService,
    private storageService: StorageService
  ) {
    this.modalService.onHide.subscribe((result: any) => {
      if (document.getElementById('CNLORDERMY_' + this.cnclModelIndex) != undefined || document.getElementById('CNLORDERMY_' + this.cnclModelIndex) != null) {
        if (result === document.getElementById('CNLORDERMY_' + this.cnclModelIndex).textContent) {
          this.OnCancelOrderClick();
        }
      }
    });
  }

  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );
  ValidationMsgsObservable = this.dataSource.asObservable();


  cnclModelIndex: string;
  lstPaymentTypes = new Array<any>();
  setLocalization() {

    var lstKeys = [
      "ODRHISTORY", "GOTODASHBOARD", "ODRSTATUS", "IMGSNOTAVAIL", "RFRESHMSG", "REFRESH"
    ];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );

    var lstKeysDuplicates = ["ODRNUMBER", "ODRDATE", "INSTHIERARCHY", "PAYMENTTYPE", "CHNGPAYMENTTYPE", "AMOUNT", "CNFMCNLORDER", "PRINT", "CNCLORDR", "LSTITMN",
      "LSTITMY", "CNCL", "PAYMENTSTATUS", "ODRSTATUS", "RSCHDLAPPNMNT", "NEWPAYTYPE", "SUBMIT", "LOCIMAGE","CMPLTORDER"];
    this.utilityService.SubscribeLocalListWithDupNames(
      this.languageTranslationService,
      lstKeysDuplicates
    );

    var lstValidationKeys = ["PKGCNCLSCSNRFND", "CRDTRDAMNTRFNDERR", "PKGCNCLSCS", "SOMEERROCCUR", "TRRNSCTNCNCLHOURS", "IMGSNOTAVAIL", "RFRESHMSG", "REFRESH"];
    this.utilityService.PopulateValidationCollectionFromKeys(lstValidationKeys, this.validationMessages);

    this.dataSource.next(this.validationMessages);
    this.utilityService.SubscribeValidationMessages(
      this.languageTranslationService,
      this.dataSource
    );
    this.ValidationMsgsObservable.subscribe(result => {
      this.validationMessages;
      this.initializeValidationMessages(result);

    });
  }
  initializeValidationMessages(validationMsgs: KeyedCollection<string>) {
    this.NoImgMsg = validationMsgs["IMGSNOTAVAIL"];
    this.refreshMsg = validationMsgs["RFRESHMSG"];
    this.refresh = validationMsgs["REFRESH"];
  }

  setMessageLocalization(isSpanishLanguage: boolean) {
    this.cnclOrderMsg = "";
    this.hdnCnclErrOrderMsg = true;
    this.SuccessMsg = "";
    this.ErrorMsg = "";
  }

  ngOnInit() {
    this.commonService.isLanguageSpanish.subscribe(isSpanishLang => {
      this.setMessageLocalization(isSpanishLang);
    });
    this.ErrorMsg = this.orderFlowService.ErrorMsg;
    this.SuccessMsg = this.orderFlowService.SuccessMsg;
    this.orderFlowService.ErrorMsg = this.orderFlowService.SuccessMsg = ''; //Setting messages empty

    this.orderDetails = this.orderFlowService.getOrderDetail();
    this.orderInfo = this.orderFlowService.getAlreadyPlacedOrder();
    if (this.orderDetails == undefined || this.orderDetails.length == 0) {
      this.orderFlowService.GetOrderHistoryDetails().subscribe((values: any) => {
        if (values != undefined) {
          this.orderDetails = values;
          this.setLocalization();
        }
      });
    }
    else {
      this.setLocalization();
    }

  }

  downloadOrderReceipt(OrderNumber: string) {

    this.orderFlowService.DownloadOrderReceipt(OrderNumber)
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
  BackToDashboard() {
    this.router.navigate(["userDashboard"]);
  }
  cancelClick(orderDetatil: OrderDetail, index: string) {
    this.cnclModelIndex = index;
    this.selectedOrder = orderDetatil;
    const initialState = {

      message: document.getElementById('CNLORDERMSG_' + index).textContent,
      title: document.getElementById('CNCLORDRTTL_' + index).textContent,
      confirmBtnName: document.getElementById('CNLORDERMY_' + index).textContent,
      closeBtnName: document.getElementById('CNLORDERMN_' + index).textContent,
    };
    this.modalService.show(ModalContentComponent, { initialState });
  }
  ngOnDestroy() {
    //this.modalService.onHide.unsubscribe();
  }
  OnCancelOrderClick() {
    if (this.selectedOrder != undefined && this.selectedOrder != null && this.selectedOrder.OrderId > 0) {

      this.orderFlowService.CancelOrder(this.selectedOrder).subscribe((values: any) => {
        if (values != undefined) {
          if (!values.HasError) {
            this.hdnCnclErrOrderMsg = false;
            if (values.ResponseObject != undefined || values.ResponseObject != null) {
              this.orderFlowService.GetOrderHistoryDetails().subscribe((values: any) => {
                if (values != undefined) {
                  this.hdnCnclErrOrderMsg = false;
                  this.orderDetails = values;
                  this.setLocalization();
                }
              });
            }
          }
          else {
            this.hdnCnclErrOrderMsg = true;
          }
          this.cnclOrderMsg = this.validationMessages[values.Message];
          this.selectedOrder = null;
          window.scrollTo(0, 0);
        }
      });
    }
  }

  onRescheduleOrderClick(orderDetatil: OrderDetail) {
    this.selectedOrder = orderDetatil;
    this.orderFlowService.isOrderAlreadyPlaced = true;
    if (this.selectedOrder != undefined && this.selectedOrder != null && this.selectedOrder.OrderId > 0) {
      this.orderFlowService.setSelectedOrderDetail(this.selectedOrder);
      if (this.LocationUpdateAllowed()) {
        if (this.IsFingerPrintRejected()) {
          this.orderFlowService.isOrderTypeChange = false;
          this.orderFlowService.isLocationChange = true;
          this.router.navigate(["orderDetail/locationReschedule"]);
        }
        else {
          this.orderFlowService.isOrderTypeChange = true;
          this.router.navigate(["/orderDetail/orderReschedule"]);
        }
      }
      else {
        this.orderFlowService.GetAppointmentSlots(this.selectedOrder.LocationID).subscribe(values => {
          if (values != undefined) {
            this.orderInfo.lstScheduleSlots = values;
            this.orderInfo.LocationDetail.LocationID = this.selectedOrder.LocationID;
            this.orderFlowService.setAlreadyPlacedOrder(this.orderInfo);
            this.orderFlowService.isOrderTypeChange = false;
            this.router.navigate(["/orderDetail/calenderReschedule"]);
          }
        });
      }
    }
  }


  onCompleteYourOrderClick(orderDetail: OrderDetail) {
    if (this.orderInfo != undefined && this.orderInfo != null && orderDetail.OrderId > 0) {
      this.storageService.SetItem(AppConsts.IsAlreadyPlacedOrder, AppConsts.TrueLiteral)
      this.storageService.SetItem(AppConsts.IsSendForOnline, AppConsts.TrueLiteral)
      this.orderInfo.OrderID = orderDetail.OrderId;
      this.orderInfo.OrderNumber = orderDetail.OrderNumber;
      this.orderInfo.CbiBillingCode = orderDetail.BillingCode;
      this.orderInfo.bkgPackageId = orderDetail.PackageID;
      this.orderInfo.bkgPkgHierarchyMappingID = parseInt(orderDetail.BkgPackageHierarchyMappingId);
      this.orderInfo.SelectedHierarchyNodeID = orderDetail.DeptProgramMappingID;
      this.orderFlowService.isOrderAlreadyPlaced = true;
      this.orderFlowService.isSendForOnline = true;
      this.orderFlowService.GetCustomAppributesForOrderID(orderDetail.OrderId, orderDetail.BackageOrderPackageID).subscribe(values => {
        if (values != undefined) {
          if (!values.HasError) {
            this.orderInfo.lstCustomAttribute = values.ResponseObject.lstCustomFormAttributes;
            this.orderInfo.CbiUniqueId = this.orderInfo.lstCustomAttribute.filter(x => x.AttributeName == "CBIUniqueID")[0].AttributeDataValue;
            if (orderDetail.IsBillingCode) {
              this.orderFlowService.getBillingCodeAmount(this.orderInfo.CbiUniqueId, this.orderInfo.CbiBillingCode).subscribe(values => {
                if (values != null && values != undefined && values != "") {
                  this.orderInfo.BillingCodeAmount = values;
                  this.orderFlowService.setAlreadyPlacedOrder(this.orderInfo);
                  this.orderFlowService.setOrderInfo(this.orderInfo);
                  this.router.navigate(["orderDetail/orderReschedule"]);
                }
              });
            }
            else {
              this.orderFlowService.setAlreadyPlacedOrder(this.orderInfo);
              this.orderFlowService.setOrderInfo(this.orderInfo);
              this.router.navigate(["orderDetail/orderReschedule"]);
            }
          }
        }
      });


    }
  }

  LocationUpdateAllowed() {
    if (this.IsFingerPrintRejected()) {
      return true;
    }
    if (this.selectedOrder.IsOnsiteAppointment || this.selectedOrder.IsColoradoFingerPrinting) {
      return true;
    }
    return false;
  }

  IsFingerPrintRejected() {
    if (this.selectedOrder.AppointmentStatusCode == 'AAAK'
      || this.selectedOrder.AppointmentStatusCode == 'AAAJ'
      || this.selectedOrder.AppointmentStatusCode == 'AAAN') {
      return true;
    }
    return false;
  }

  onChangePaymentTypeClick(orderDetail: OrderDetail, index: string) {

    this.orderFlowService.getPaymentOptions(orderDetail.DeptProgramPackageID,
      orderDetail.BkgPackageHierarchyMappingId, orderDetail.DeptProgramMappingID, orderDetail.IsBillingCode)
      .subscribe(result => {
        this.lstPaymentTypes = result[0].lstPaymentOptions;
        var paymentTypeCodes: Array<string> = orderDetail.PaymentTypeCode.split(',');
        this.lstPaymentTypes = this.lstPaymentTypes.filter(x => paymentTypeCodes.indexOf(x.PaymentOptionCode) < 0);
        document.getElementById('dvChangePayType_' + index).style.display = 'block';
        if (this.lstPaymentTypes.length > 0)
          this.selectedPaymentOptionId = this.lstPaymentTypes[0].PaymentOptionId;
      });
  }
  onChangePaymentOption(paymentOptionId: string) {
    this.selectedPaymentOptionId = paymentOptionId;
  }
  onChangePaymentTypeSubmit(orderDetail: OrderDetail) {
    this.orderInfo = this.orderFlowService.getAlreadyPlacedOrder();
    this.orderInfo.OrderNumber = orderDetail.OrderNumber;
    this.orderFlowService.setAlreadyPlacedOrder(this.orderInfo);
    this.storageService.SetItem(AppConsts.IsAlreadyPlacedOrder, AppConsts.TrueLiteral);
    this.orderFlowService
      .ChangePaymentTypeSubmitOrder(
        this.orderInfo.OrderNumber, +this.selectedPaymentOptionId, this.languageTranslationService.GetLanguageParams().SelectedLangCode
      )
      .subscribe(result => {
        if (result.HasError) {
          this.ErrorMsg = result.ErrorMessage;
        }
        else if (this.lstPaymentTypes.find(x => x.PaymentOptionId == this.selectedPaymentOptionId).PaymentOptionCode == "PTCC")
          this.router.navigate(["createOrder/CardSelection"]);
        else
          this.router.navigate(["createOrder/OrderSummary"]);
      });
  }
  onCancelClick(orderDetail: OrderDetail, index: string) {
    document.getElementById('dvChangePayType_' + index).style.display = 'none'

  }
  openModal(template: TemplateRef<any>, LocationID: number, locationName: String) {
    this.locationID = LocationID;
    this.locationName = locationName;
    this.orderFlowService.getLocationImages(LocationID).subscribe(s => {
      console.log(s);
      if (s != -1) {
        this.Locations = s;
        this.IsImageError = false;
      }
      else {
        this.IsImageError = true;
        this.Locations = null;
      }
      this.modalRef = this.modalService.show(template);
    });

  }
  refreshModel() {
    this.orderFlowService.getLocationImages(this.locationID).subscribe(s => {
      console.log(s);
      if (s != -1) {
        this.Locations = s;
        this.locationName = this.locationName;
        this.IsImageError = false;
      }
      else {
        this.IsImageError = true;
        this.Locations = null;
      }

    });
  }
}
