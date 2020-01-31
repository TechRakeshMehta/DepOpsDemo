import { Component , OnInit } from '@angular/core';
import { Router } from '@angular/router'
import {AuthService} from '../../services/guard/auth-guard.service'
import {DataSharingService} from '../../services/shared-services/data-sharing.service'
import { Subscription } from "rxjs";
import {AppConstant} from '../../models/common/AppConst'
import { AdbApiService } from '../../services/shared-services/adb-api-data.service'
declare var $ :any;

@Component({
    selector: 'picture-upload-app',
    templateUrl: './picture-upload.component.html',
    styleUrls:['./picture-upload.scss']
})
export class PictureUploadComponent implements OnInit {
    private fileCollection: any;
    urls = new Array<string>();
    private fileData: any;
    private description: any;
    constructor(public appConstant : AppConstant, 
        public dataSharingService: DataSharingService, public router: Router, private adbApiService: AdbApiService ) {
        
    }

    ngOnInit(){
        this.fileCollection = this.dataSharingService.fileCollection;
        this.urls = [];
        let files = this.fileCollection.target.files;
        let formData: FormData = new FormData();  
        if (files) {
        for (let file of files) {
                formData.append('uploadFile', file, file.name);  
                let reader = new FileReader();
                reader.onload = (e: any) => {
                  this.urls.push(e.target.result);
                }
            reader.readAsDataURL(file);
        }
      this.fileData = formData;
    }

    }
    postUploadData(descriptionData: any){
        var description = '';
        for(let keys in descriptionData){ 
            description = description + descriptionData[keys]+',';
        }
        description = description.substring(0,description.length-1);
        description = '['+description+']';
        this.adbApiService.postUploadDocument(this.fileData,description).subscribe(userData => {
            this.dataSharingService.successMsg = userData;
            this.router.navigate(['/welcome/imageupload/success']);
        });
    }
    getApplicantPackages(){
        this.router.navigate(['/welcome']);
    }
    removeSection(section: any){
        var target = section.target || section.srcElement || section.currentTarget;
        var idAttr = target.attributes;
        $(idAttr).parent().parent().remove();
    }
}
