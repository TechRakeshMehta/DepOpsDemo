import { Component , OnInit } from '@angular/core';
import { Router } from '@angular/router'
import {AuthService} from '../../services/guard/auth-guard.service'
import {DataSharingService} from '../../services/shared-services/data-sharing.service'
import { Subscription } from "rxjs";
import {AppConstant} from '../../models/common/AppConst'
import { AdbApiService } from '../../services/shared-services/adb-api-data.service'
declare var $ :any;

@Component({
    selector: 'success-app',
    templateUrl: './success.component.html'
})
export class SuccessComponent implements OnInit {
    private fileCollection: any;
    urls = new Array<string>();
    private fileData: any;
    private description: any;
    private successMsg: any;
    constructor(public appConstant : AppConstant, 
        public dataSharingService: DataSharingService, public router: Router, private adbApiService: AdbApiService ) {
        
    }

    ngOnInit(){
        this.successMsg = this.dataSharingService.successMsg;
    }
    getApplicantPackages(){
        this.router.navigate(['/welcome']);
    }
}
