import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OrderFlowService } from '../../../services/order-flow/order-flow.service';
import { OrderInfo } from '../../../models/order-flow/order.model';
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { UtilityService } from "../../../services/shared-services/utility.service";

@Component({
  selector: 'app-out-of-state',
  templateUrl: './out-of-state.component.html',
  styleUrls: ['./out-of-state.component.css']
})
export class OutOfStateComponent implements OnInit {

  orderInfo: OrderInfo;
  isSendForOnline: boolean = false;
  constructor(
    private router: Router,
    private orderFlowService: OrderFlowService,
    private languageTranslationService: LanguageTranslationService,
    private utilityService: UtilityService) { }

  ngOnInit() {
    this.orderInfo = this.orderFlowService.getOrderInfo();
    this.isSendForOnline = this.orderFlowService.isSendForOnline;
    var lstKeys = ["OFSTOPLABEL", "OFSFNGRPRNTCRD", "OFSFNGRPRNTCRDDATA",
      "CNFRMTN", "OFSCNFRMTNDATA", "OFSMNYORDR", "OFSMNYORDRDATA", "MAIL", "OFSMAILDATA",
      "OFSBOTTOMLABEL", "PREVIOUS", "NEXT", "CREATODR", "OUTOFSTATE", "STEP"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
  }
  onSubmit() {

    this.orderFlowService.getOutOfStateLocation().subscribe(values => {
      if (values != null && values != undefined && values > 0) {
        if(this.orderInfo.LocationDetail.LocationID != values)
        {
          this.orderInfo.LocationDetail.IsLocationUpdate =true;
        }
        this.orderInfo.LocationDetail.LocationID = values;
        this.orderInfo.LocationDetail.IsOutOfState = true;
        this.orderFlowService.setOrderInfo(this.orderInfo);
        this.orderFlowService.IncrementStep();
        if (this.isSendForOnline == true) {
          this.router.navigate(["createOrder/PaymentDetail"]);
        }
        else {
          this.router.navigate(["createOrder/Orderflowpackages"]);
        }
      }
    });
  }

  goBack() {
    if (this.isSendForOnline == true)
      this.router.navigate(["orderDetail/orderReschedule"]);
    else
      this.router.navigate(["createOrder"]);
  }
}
