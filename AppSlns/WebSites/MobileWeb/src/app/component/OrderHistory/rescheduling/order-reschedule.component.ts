import { Component, OnInit, EventEmitter, Output } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { OrderInfo, LocationDetail } from "../../../models/order-flow/order.model";
import { Router } from "@angular/router";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { UtilityService } from "../../../services/shared-services/utility.service";
import { debug } from "util";

@Component({
  selector: "order-reschedule",
  templateUrl: "order-reschedule.component.html"
})
export class OrderRescheduleComponent implements OnInit {
  CreatOrderForm = this.formBuilder.group({
    selectedOrderType: [""]
  });

  constructor(
    private formBuilder: FormBuilder,
    private orderFlowService: OrderFlowService,
    private router: Router,
    private languageTranslationService: LanguageTranslationService,
    private utilityService: UtilityService
  ) { }

  orderInfo: OrderInfo;
  isSendForOnline: boolean = false;
  PackagePrice: string;

  ngOnInit() {
    var lstKeys = ["RSCHDLAPPNMNT", "OUTOFSTATE", "CLRDFNGRPRNTNGST", "HVEVENTCODE", "NEXT",
      "CNFMCNLRESCHEDULING", "CNCLRESCHEDULING", "LSTITMY", "LSTITMN", "CNCL"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );

    this.orderInfo = this.orderFlowService.getAlreadyPlacedOrder();
    this.isSendForOnline = this.orderFlowService.isSendForOnline;
    this.populateFormControls();
    if (this.isSendForOnline) {
      this.orderFlowService.getPrice().subscribe(data => {
        this.PackagePrice = data;
        this.orderFlowService.setTotalPackagePriceInfo(this.PackagePrice);
        this.orderInfo.IsPaymentByInstAlone = false;
        this.orderInfo.IsPaymentByInst = false;
        if (this.orderInfo.CbiBillingCode != "" && this.orderInfo.CbiBillingCode != null) {
          if (this.orderInfo.BillingCodeAmount != undefined && this.orderInfo.BillingCodeAmount != null && this.orderInfo.BillingCodeAmount != "") {
            if (this.PackagePrice <= this.orderInfo.BillingCodeAmount) {
              this.orderInfo.IsPaymentByInstAlone = true;
              this.orderInfo.IsPaymentByInst = true;
            }
            else {
              this.orderInfo.IsPaymentByInst = true;
              this.orderInfo.IsPaymentByInstAlone = false;
            }
          }
        }
      });
    }
  }

  populateFormControls() {
    var formControls = this.CreatOrderForm.controls;
    if (
      this.orderInfo.selectedOrderType != undefined &&
      this.orderInfo.selectedOrderType != ""
    ) {
      formControls["selectedOrderType"].setValue(
        this.orderInfo.selectedOrderType
      );
    } else {
      formControls["selectedOrderType"].setValue("1");
    }
  }

  onSubmit() {
    var orderObject = this.CreatOrderForm.value;
    if (
      this.orderInfo.selectedOrderType != undefined &&
      this.orderInfo.selectedOrderType != "" &&
      orderObject.selectedOrderType != this.orderInfo.selectedOrderType
    ) {
      this.orderInfo.selectedOrderType = "";
      this.orderInfo.LocationDetail = new LocationDetail();
      //this.orderFlowService.setAlreadyPlacedOrder(new OrderInfo());
      //this.orderInfo = this.orderFlowService.getAlreadyPlacedOrder();
    }
    this.orderInfo.selectedOrderType = orderObject.selectedOrderType;
    this.orderFlowService.setAlreadyPlacedOrder(this.orderInfo);
    switch (orderObject.selectedOrderType) {
      case "1":
        this.router.navigate(["orderDetail/locationReschedule"]);
        this.orderFlowService.setAlreadyPlacedOrder(this.orderInfo);
        break;
      case "2":
        this.router.navigate(["orderDetail/eventReschedule"]);
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
}
