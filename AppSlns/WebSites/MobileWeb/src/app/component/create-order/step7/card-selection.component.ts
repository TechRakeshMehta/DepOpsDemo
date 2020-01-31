import { Component, OnInit, ElementRef, ViewChild } from "@angular/core";
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { Router } from "@angular/router";
import {
  OrderInfo,
  AuthorizeNetInfo
} from "../../../models/order-flow/order.model";
import { StorageService } from "../../../services/shared-services/storage.service";
import { UtilityService } from "../../../services/shared-services/utility.service";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { AuthService } from "../../../services/guard/auth-guard.service";
import { CommonService } from "../../../services/shared-services/common.service";
import { LanguageParams } from "../../../models/language-translation/language-translation-contract.model";
import { AppConsts } from "../../../../environments/constants/appConstants";

@Component({
  selector: "app-card-selection",
  templateUrl: "./card-selection.component.html",
  styleUrls: ["./card-selection.component.css"]
})
export class CardSelectionComponent implements OnInit {
  errorMsg: any;
  orderId: number;
  invoiceNumber: string;
  lstCards: any;
  orderInfo: OrderInfo;
  selectedCustomerPaymentProfileId: number;
  IsError: boolean = false;
  authorizeNetInfo: AuthorizeNetInfo;
  authNetToken: string;
  authNetAction: string;
  languageParams: LanguageParams;
  IsAvailableCard: boolean = false;
  @ViewChild("formAuthorizeNetForm") formAuthorizeNetForm: ElementRef;
  element: HTMLFontElement;
  isAlreadyPlacedOrder: boolean = false;
  IsSlotExp: boolean = false;
  ReserverSlotID: number;


  constructor(
    private orderFlowService: OrderFlowService,
    private router: Router,
    private storageService: StorageService,
    private languageTranslationService: LanguageTranslationService,
    private utilityService: UtilityService,
    private authService: AuthService,
    private commonService: CommonService
  ) { }

  ngOnInit() {
    //Disable Browser's back button code starts
    history.pushState(null, null, document.URL);
    window.addEventListener('popstate', function () {
      history.pushState(null, null, document.URL);
    });
    //Disable Browser's back button code ends
    var token = sessionStorage.getItem("auth_token");
    if (token != null) {
      this.authService.setLoggedIn(true);
      this.commonService.SetLoggedIn(true);
    }
    var alreadyPlacedOrderStorage = this.storageService.GetItem(
      AppConsts.IsAlreadyPlacedOrder
    );
    if (
      alreadyPlacedOrderStorage != undefined &&
      alreadyPlacedOrderStorage != ""
    ) {
      this.isAlreadyPlacedOrder =
        alreadyPlacedOrderStorage == AppConsts.TrueLiteral;
    }
    this.orderInfo = this.isAlreadyPlacedOrder
      ? this.orderFlowService.getAlreadyPlacedOrder()
      : this.orderFlowService.getOrderInfo();
    this.lstCards = new Array<any>();
    this.GetCustomerpaymentProfiles();
    if (
      this.orderInfo.OrderNumber != null &&
      this.orderInfo.OrderNumber.trim() != ""
    ) {
      this.storageService.SetItem(
        AppConsts.OrderNumber,
        this.orderInfo.OrderNumber
      );
      if (!this.isAlreadyPlacedOrder)
        this.storageService.SetItem(
          AppConsts.TotalStepsHeader,
          this.orderInfo.TotalSteps.toString()
        );
    }
    var lstKeys = [
      "CRDTYPE",
      "NAMEONCARD",
      "ADDNEWCARD",
      "SUBMIT",
      "CREATODR",
      "AVLBLCRD",
      "SLOTEXPBEFOREPAYMENTALRTMSG",
      "GOTODASHBOARD"
    ];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
  }

  selectCard(CustomerPaymentProfileId: number) {
    this.selectedCustomerPaymentProfileId = CustomerPaymentProfileId;
  }
  GetCustomerpaymentProfiles() {
    this.orderFlowService.GetCustomerPaymentProfiles().then(profiles => {
      if (profiles.HasError) {
        this.errorMsg = profiles.ErrorMessage;
        this.IsError = true;
      } else {
        this.lstCards = profiles.ResponseObject;
        this.IsError = false;
        if (this.lstCards != undefined && this.lstCards.length > 0) {
          if (this.lstCards[0].CustomerPaymentProfileId > 0) {
            this.selectedCustomerPaymentProfileId = this.lstCards[0].CustomerPaymentProfileId;
            this.IsAvailableCard = true;


          }
        }
        else {
          this.selectedCustomerPaymentProfileId = 0;
          this.IsAvailableCard = false;
        }

        var lstKeysDuplicates = ["REMOVE"];
        this.utilityService.SubscribeLocalListWithDupNames(
          this.languageTranslationService,
          lstKeysDuplicates
        );
      }
      //console.log(this.lstCards);
    });
  }
  OrderHistory() {
    this.router.navigate(["userDashboard"]);
  }
  IsReservedSlotExpired() {
    if (this.storageService.GetItem("ReserverSlotID") != ""
      && this.orderInfo.LocationDetail.ReserverSlotID == undefined
    ) {
      this.orderInfo.LocationDetail.ReserverSlotID = Number(this.storageService.GetItem("ReserverSlotID"))
    }
    var result = true;
    if (!this.orderInfo.LocationDetail.IsOutOfState
      && this.orderInfo.LocationDetail.ReserverSlotID > 0) {
      this.orderFlowService.IsReservedSlotExpired(this.orderInfo.LocationDetail.ReserverSlotID, true)
        .then(result => {
          if (result === true) {
            this.IsAvailableCard = false;
            this.IsSlotExp = true;
          }
          else {
            this.onSubmit();
          }
        })
    }
    else {
      this.onSubmit();
    }
    return result;
  }
  onSubmit() {
    if (
      this.selectedCustomerPaymentProfileId != undefined &&
      this.selectedCustomerPaymentProfileId > 0
    ) {
      var totalStep = this.storageService.GetItem(AppConsts.TotalStepsHeader);
      var orderInfo = this.orderFlowService.getOrderInfo();
      orderInfo.OrderNumber = this.storageService.GetItem(
        AppConsts.OrderNumber
      );
      if (totalStep != undefined && totalStep != "") {
        orderInfo.CurrentStep = parseInt(totalStep);
        orderInfo.TotalSteps = parseInt(totalStep);
      }
      if (!this.isAlreadyPlacedOrder) {
        this.orderFlowService.setOrderInfo(orderInfo);
      }
      else {
        this.orderFlowService.setAlreadyPlacedOrder(orderInfo);
      }
      this.orderFlowService
        .SubmitPayment(
          orderInfo.OrderNumber,
          this.selectedCustomerPaymentProfileId,
          this.isAlreadyPlacedOrder
        )
        .subscribe(result => {
          if (result.HasError) {
            this.errorMsg = result.ErrorMessage;
            this.IsError = true;
          } else this.router.navigate(["createOrder/OrderSummary"]);
          this.IsError = false;
          this.storageService.SetItem("ReserverSlotID", "")
        });
    }
  }

  addNewCard() {
    this.storageService.SetItem(
      "langCode",
      this.languageTranslationService.GetLanguageParams().SelectedLangCode
    );
    this.storageService.SetItem(
      "ReserverSlotID",
      String(this.orderInfo.LocationDetail.ReserverSlotID)

    );
    var t = location.href;
    this.authorizeNetInfo = new AuthorizeNetInfo();
    this.authorizeNetInfo.ReturnUrl = t;
    this.authorizeNetInfo.CurrentUrl = t;
    this.orderFlowService
      .GetAuthorizeNetToken(this.authorizeNetInfo)
      .subscribe(result => {
        this.authNetAction = result.ActionUrl + "addPayment";
        this.authNetToken = result.Token;

        setTimeout(() => {
          this.formAuthorizeNetForm.nativeElement.submit();
        }, 1);
      });
  }

  RemoveCard(CustomerPaymentProfileId: number) {
    this.orderFlowService
      .RemoveCard(
        CustomerPaymentProfileId,
        this.orderInfo.userInfo.OrganizationUserID
      )
      .subscribe(res => {
        if (res.HasError) {
          this.errorMsg = res.ErrorMessage;
          this.IsError = true;
        } else {
          this.IsError = false;
          this.GetCustomerpaymentProfiles();
        }
      });
  }

  ngOnDestroy() {
  }
}
