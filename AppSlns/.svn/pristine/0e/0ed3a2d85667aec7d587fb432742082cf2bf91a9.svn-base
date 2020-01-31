import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { AuthService } from "../../../services/guard/auth-guard.service";
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { CommonService } from "../../../services/shared-services/common.service";
import { Observable, BehaviorSubject, Subscription } from "rxjs";
import { AdbApiService } from "../../../services/shared-services/adb-api-data.service";
import { StorageService } from "../../../services/shared-services/storage.service";

@Component({
  selector: "app-layout-header",
  templateUrl: "./header.component.html"
})
export class HeaderComponent implements OnInit {
  LogOutText: string;
  private IsLocationSpecificTenant: boolean;
  
  constructor(private storageService:StorageService,
     private router: Router, private authservice: AuthService
    , public orderFlowService: OrderFlowService, private commonService: CommonService
    , private adbApiService: AdbApiService) {
    this.adbApiService.getCbiUrlInfo().subscribe(userData => {
      this.IsLocationSpecificTenant = userData.Message;
    });
  }

  subscriptionLanguage: Subscription;

  ngOnInit() {
    if (!this.subscriptionLanguage || this.subscriptionLanguage.closed) {
      this.subscriptionLanguage = this.commonService.isLanguageSpanish.subscribe(value => {
        if (!value) {
          this.LogOutText = "Logout";
        }
        else {
          this.LogOutText = "Cerrar sesión";
        }
      })
    }
  }
  onSubmit() {
    sessionStorage.clear();
    this.orderFlowService.setOrderInfoToEmpty();
    this.orderFlowService.isSendForOnline=false;
    this.router.navigate([""]);
    this.authservice.logoutUser();
    this.storageService.Clear();
  }
  ngOnDestroy() {
    if (this.subscriptionLanguage && !this.subscriptionLanguage.closed) {
      this.subscriptionLanguage.unsubscribe();
    }
  }
}
