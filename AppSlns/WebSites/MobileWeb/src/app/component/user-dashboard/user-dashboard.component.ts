import { Component, OnInit } from "@angular/core";
import { AdbApiService } from "../../services/shared-services/adb-api-data.service";
import { Router } from "@angular/router";
import { OrderFlowService } from "../../services/order-flow/order-flow.service";
import { CommonService } from "../../services/shared-services/common.service";
import { LanguageTranslationService } from "../../services/language-translations/language-translations.service";
import { UtilityService } from "../../services/shared-services/utility.service";

@Component({
  selector: "user-dashboard",
  templateUrl: "user-dashboard.component.html"
})
export class UserDashboardComponent implements OnInit {
  private applicantName: string;
  private tenantName: string;
  public IsOrderAvailable: boolean = true;
  constructor(
    public router: Router,
    private adbApiService: AdbApiService,
    public orderFlowService: OrderFlowService,
    private commonService: CommonService,
    private languageTranslationService: LanguageTranslationService,
    private utilityService: UtilityService
  ) { }

  ngOnInit() {
    this.adbApiService.getUserInfo().subscribe(userData => {
      if (userData != undefined) {
        this.commonService.SetLoggedIn(true); //Used to set header
        this.applicantName = userData.ApplicantName;
      } else {
        this.router.navigate([""]);
      }
    });

    this.adbApiService.getTenantName().subscribe(userData => {
      if (userData != undefined) {
        this.tenantName = userData.TenantName;
      } else {
        this.router.navigate([""]);
      }
    });

    this.orderFlowService.GetOrderHistoryDetails().subscribe((values: any) => {
      if (values != undefined && values.length > 0) {
        this.orderFlowService.setOrderDetail(values);
      }
      else {
        this.IsOrderAvailable = false;
      }
    });

    var lstKeys = ["APPLICANT", "INSTITUTE", "PLACEORDER", "ODRHISTORY","EDITPROFILE"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
  }

  createOrder() {
    this.orderFlowService.isSendForOnline=false;
    this.router.navigate(["createOrder"]);
  }
  OrderHistory() {
    this.router.navigate(["OrderHistory"]);
  }
  EditProfile() {
    this.orderFlowService.getOrganizationUser().then(()=>{
      this.orderFlowService.IsEditProfile=true;
      this.router.navigate(["createOrder/personalInfo"]);     
     
    });    
  }
}
