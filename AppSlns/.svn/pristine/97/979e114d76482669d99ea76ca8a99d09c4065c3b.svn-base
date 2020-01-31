import { Component , OnInit } from '@angular/core';
import { Router } from '@angular/router'
import {AuthService} from '../../services/guard/auth-guard.service'
import {DataSharingService} from '../../services/shared-services/data-sharing.service'
import { Subscription } from "rxjs";
import {AppConstant} from '../../models/common/AppConst'
import { AdbApiService } from '../../services/shared-services/adb-api-data.service'
@Component({
    selector: 'file-upload-app',
    templateUrl: './file-upload.component.html',
    styleUrls:['./file-upload.scss']
})
export class FileUploadComponent implements OnInit {
    private applicantName: string;
    constructor(public appConstant : AppConstant, 
        public dataSharingService: DataSharingService, public router: Router, private adbApiService: AdbApiService ) {
        
    }

    ngOnInit(){
        
    }
}
