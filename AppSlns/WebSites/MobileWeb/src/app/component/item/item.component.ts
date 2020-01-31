import { Component, OnInit, ViewChild,Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
 
import { ItemDetail } from "../../models/applicant/package-contract.model";
import { AdbApiService } from '../../services/shared-services/adb-api-data.service'
import { ComplianceAttribute } from '../attribute/attribute.component';

@Component({
    selector: 'compliance-item-aap',
    templateUrl: './item.html', 
    styleUrls: ['./item.component.css']
})

export class ComplianceItem implements OnInit {
    @ViewChild(ComplianceAttribute) child: ComplianceAttribute;
    @Input() itemForCategory: any;
    constructor(private adbApiService: AdbApiService) { }

    /*Declaring Variables*/
    private SelectedCategoryID: number = parseInt(sessionStorage.getItem('SelectedCategoryId'));
    private PkgSubscriptionId: number = parseInt(sessionStorage.getItem('SelectedPkgSubsID'));
    public SelectedItemID: number;
    public attrForItem: any;
    public current: number = -1;
    lstItem = new Array<ItemDetail>(); 

    ngOnInit() {
        this.itemForCategory = this.adbApiService.PackageDetail.lstCategory.filter(cat => cat.CategoryId == this.SelectedCategoryID)[0].lstItem;
    }

    //Show Attribute on item click 
    ShowAttribute(SelectedItemID: number, siteIndex: number)
    { 
        this.SelectedItemID = SelectedItemID;
        if (this.current == siteIndex) {
            this.current = -1;
        } else {
            this.current = siteIndex;
        }
        sessionStorage.setItem('selectedItemId', SelectedItemID.toString());
        this.attrForItem = this.adbApiService.PackageDetail.lstCategory.filter(cat => cat.CategoryId == this.SelectedCategoryID)[0]
            .lstItem.filter(itm => itm.ItemId == this.SelectedItemID)[0].lstAttribute;
      
    }
}