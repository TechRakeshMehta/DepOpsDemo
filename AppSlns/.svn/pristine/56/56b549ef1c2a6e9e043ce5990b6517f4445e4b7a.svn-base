import { Component, OnInit,ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AdbApiService } from '../../services/shared-services/adb-api-data.service'
import { PackageDetail } from "../../models/applicant/package-contract.model"
import { ComplianceItem } from "../item/item.component";

@Component({
    selector: 'compliance-category-aap',
    templateUrl: './category.html',
    styleUrls: ['./category.component.css']
})

export class ComplianceCategory implements OnInit {      
    @ViewChild(ComplianceItem) child: ComplianceItem;
      constructor( private adbApiService: AdbApiService,public router: Router) { }
     
    /*Declaring Variables*/
    public showData: boolean = false;
    public  current: number = -1;
    public SelectedPackageID: number; 
    public SelectedCategoryID: number;
    SelectedPackage = new PackageDetail(); 
    public PackageDetail: any;
    public itemForCategory: any;
    ngOnInit() {
            this.SelectedPackageID = parseInt(sessionStorage.getItem('selectedPackageId'));
            this.adbApiService.getApplicantPackageCategoryDetails(this.SelectedPackageID).subscribe(values => {
            this.SelectedPackage = values;
            this.adbApiService.PackageDetail=values;
        });
    } 

    ShowItem(selectedCategoryID: number,siteIndex: number) {
        sessionStorage.setItem('SelectedCategoryId', selectedCategoryID.toString());
        if (this.current == siteIndex) {
            this.current = -1;
        } else {
            this.current = siteIndex;
        }
        this.SelectedCategoryID = selectedCategoryID;
        this.itemForCategory = this.SelectedPackage.lstCategory.filter(cat => cat.CategoryId == this.SelectedCategoryID)[0].lstItem;
    }
    ReturntodashBoard(){
        sessionStorage.removeItem('selectedPackageId');
        sessionStorage.removeItem('SelectedCategoryId');
        sessionStorage.removeItem('selectedItemId');
        this.router.navigate(['/welcome']); 
    }
    dataShow(){
        this.showData= true;
    }
    show(event: any){
        this.showData= false;
    }
}