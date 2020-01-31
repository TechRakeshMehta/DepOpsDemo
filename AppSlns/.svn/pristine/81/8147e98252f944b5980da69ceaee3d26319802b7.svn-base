import { Component, OnInit, EventEmitter, Output } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { OrderInfo } from "../../../models/order-flow/order.model";
import { Router } from "@angular/router";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { UtilityService } from "../../../services/shared-services/utility.service";
import { debug } from "util";
import { StorageService } from "../../../services/shared-services/storage.service";
import { AppConsts } from "../../../../environments/constants/appConstants";

@Component({
  selector: "create-order",
  templateUrl: "create-order.component.html"
})
export class CreateOrderComponent implements OnInit {
  CreatOrderForm = this.formBuilder.group({
    selectedOrderType: [""]
  });

  constructor(
    private formBuilder: FormBuilder,
    private OrderFlowService: OrderFlowService,
    private router: Router,
    private languageTranslationService: LanguageTranslationService,
    private utilityService: UtilityService,
    private orderFlowService:OrderFlowService,
    private storageService:StorageService
  ) {}

  orderInfo: OrderInfo;

  ngOnInit() {
        
    this.storageService.SetItem(AppConsts.IsAlreadyPlacedOrder, "");
    this.storageService.SetItem(AppConsts.IsSendForOnline, "");

    this.orderFlowService.IsEditProfile=false;
    var lstKeys = ["CLRDFNGRPRNTNGST", "HVEVENTCODE","OUTOFSTATE","PREVIOUS","NEXT","CREATODR"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
    this.orderInfo = this.OrderFlowService.getOrderInfo();
    this.populateFormControls();
    
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
      this.OrderFlowService.setOrderInfo(new OrderInfo());
      this.orderInfo = this.OrderFlowService.getOrderInfo();
    }
    this.orderInfo.selectedOrderType = orderObject.selectedOrderType;
    this.orderInfo.CurrentStep = 1;
    this.OrderFlowService.setOrderInfo(this.orderInfo);
    switch (orderObject.selectedOrderType) {
      case "1":
        this.router.navigate(["createOrder/location"]);
        this.orderInfo.TotalSteps = 8;
        this.OrderFlowService.setOrderInfo(this.orderInfo);
        break;
      case "2":
        this.router.navigate(["createOrder/eventdetails"]);
        this.orderInfo.TotalSteps = 7;
        this.OrderFlowService.setOrderInfo(this.orderInfo);
        break;
      case "3":
        this.router.navigate(["createOrder/OutOfState"]);
        this.orderInfo.TotalSteps = 7;
        this.OrderFlowService.setOrderInfo(this.orderInfo);
        break;
      default:
    }
  }

  goBack() {
    this.OrderFlowService.setOrderInfoToEmpty();
    this.router.navigate(["userDashboard"]);
  }
}
