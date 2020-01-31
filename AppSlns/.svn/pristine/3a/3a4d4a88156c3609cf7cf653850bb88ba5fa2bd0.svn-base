import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router'
import { AuthService } from '../../services/guard/auth-guard.service'
import { DataSharingService } from '../../services/shared-services/data-sharing.service'
import { Subscription } from "rxjs";
import { AppConstant } from '../../models/common/AppConst'
import { AdbApiService } from '../../services/shared-services/adb-api-data.service'


@Component({
    selector: 'welcome-app',
    templateUrl: './welcome.component.html',
    styleUrls: ['./welcome.scss']
})
export class WelcomeComponent implements OnInit {
    private applicantName: string;

    constructor(public appConstant: AppConstant,
        public dataSharingService: DataSharingService, public router: Router, private adbApiService: AdbApiService) {

    }

    ngOnInit() {
        this.adbApiService.getUserInfo().subscribe(userData => {
            //console.log(userData);
            this.applicantName = userData.ApplicantName;
        });
    }

    fileUploadEvent(event: any) {
        //console.log(event);
        this.dataSharingService.fileCollection = event;
        this.router.navigate(['/welcome/imageupload']);
    }
    getApplicantPackages() {

       
        this.adbApiService.getApplicantPackages().subscribe(values => {
            if (values.length != undefined) {
                sessionStorage.setItem('selectedPackageId', values[0].PackageId.toString());
                this.router.navigate(['/category']);
            }
        });

    }

}
