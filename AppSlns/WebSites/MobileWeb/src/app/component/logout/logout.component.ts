import { Component } from '@angular/core';
import { Router } from '@angular/router'
import { AuthService } from '../../services/guard/auth-guard.service'
import { DataSharingService } from '../../services/shared-services/data-sharing.service'
import { Subscription } from "rxjs";
import { AppConstant } from '../../models/common/AppConst';
import { OrderFlowService } from '../../services/order-flow/order-flow.service';
import { StorageService } from "../../services/shared-services/storage.service";

@Component({
    selector: 'logout-app',
    templateUrl: './logout.component.html',
    styleUrls: ['./logout.component.scss']
})
export class LogoutComponent {
    private userAuthenticationData: any;
    public complioLogo: string;
    public errorMsg: string = '';
    constructor(private loginDataSharingServive: AuthService, public appConstant: AppConstant,
        public dataSharingService: DataSharingService, public router: Router
        , public orderFlowService: OrderFlowService,
        private storageService: StorageService) {
    }
    logout() {
        sessionStorage.clear();
        this.orderFlowService.setOrderInfoToEmpty();
        this.storageService.Clear();
        this.router.navigate(['']);
    }
}
