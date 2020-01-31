import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ComplianceCategory } from "../category/category.component";
import { PackageDetail } from "../../Models/applicant/package-contract.model";
import { AdbApiService } from '../../services/shared-services/adb-api-data.service'

@Component({
    selector: 'compliance-package-aap',
    templateUrl: './package.html',
    styleUrls: ['./package.component.css']
})

export class CompliancePackage implements OnInit {

    constructor( private router: Router, private adbApiService: AdbApiService) { }

    /*Declaring Variables*/
    public SelectedPackageID: any;
    lstPackages = new Array<PackageDetail>();
  
    ngOnInit() {
        this.adbApiService.getApplicantPackages().subscribe(values => {
            if(values.length !=undefined){
                this.lstPackages = values;  
            } 
        });
    }

    ShowCategory(selectedPackageID: number) {
        sessionStorage.setItem('selectedPackageId', selectedPackageID.toString());
        this.router.navigate(['/category']);
    }

    ReturntodashBoard(){
         sessionStorage.removeItem('selectedPackageId');
         sessionStorage.removeItem('SelectedCategoryId');
         sessionStorage.removeItem('selectedItemId');
        this.router.navigate(['/welcome']);
    }
}