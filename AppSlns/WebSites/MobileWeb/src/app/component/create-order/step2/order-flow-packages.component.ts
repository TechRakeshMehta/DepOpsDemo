import { Component, OnInit } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { OrderInfo } from "../../../models/order-flow/order.model";
import { UserInfo } from "../../../models/user/user.model";
import { Router } from "@angular/router";
import { Validators } from "@angular/forms";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { UtilityService } from "../../../services/shared-services/utility.service";
import { KeyedCollection } from "../../../models/common/dictionary.model";
import { Observable, BehaviorSubject } from "rxjs";
import { basename } from "path";

@Component({
  selector: "order-flow-packages",
  templateUrl: "order-flow-packages.component.html"
})
export class OrderFlowPackagesComponent implements OnInit {
  lstPackages: Array<any>;
  uniqueIdInfo: Array<any>;
  isValidCbiUniqueId: string = "True";
  SelectedPackageID: number;
  packageName: string = "";
  SelectedHierarchyNodeID: number;
  bkgPkgHierarchyMappingID: number;
  hdnBillingCode: Boolean = true;
  CbiUniqueID: string = "";
  isValidBillingCode: boolean = true;
  ServiceDescription = "";
  CBIUniqueIDCusMsg: string;
  CBIBillingCodeCusMsg: string;
  lstCBIUniqueIds: Array<any>;
  selectedCBIUniqueId: string;
  isValidAcctNameOrNum: Boolean = true;
  AcctNameOrNumCusMsg: string;
  CbiUniqueIdBillingCodeMapping = {
    CbiUniqueId: "",
    BillingCode: ""
  }
  orderFlowPackageForm = this.formBuilder.group({
    cbiUniqueId: ["", Validators.required],
    packageId: ["", Validators.required],
    CbiBillingCode: [""],
    AcctNameOrNumber: ["", Validators.minLength(4)]
  });

  constructor(private formBuilder: FormBuilder, private OrderFlowService: OrderFlowService, private router: Router,
    private languageTranslationService: LanguageTranslationService,
    private utilityService: UtilityService

  ) { }
  userInfo: UserInfo;
  orderInfo: OrderInfo;
  Order_flow_validation_messages: any;
  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );
  ValidationMsgsObservable = this.dataSource.asObservable();
  setLocalization() {
    var lstKeys = ["CBIUNIQUEID", "BILLINGCODE", "PREVIOUS", "NEXT", "ENTRBILINGCODEIFHAVE",
      "UNIQUEIDINFOMSG", "CONTACTEMPLOYEE", "OF", "CREATODR", "STEP", "ORDERSELECTION", "LOOKUP", "ACCTNAMEORACCTNUMBER", "PLEASESELECT"];
    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
    var lstKeysDuplicates = ["PLEASESELECT"];
    this.utilityService.SubscribeLocalListWithDupNames(
      this.languageTranslationService,
      lstKeysDuplicates
    );
    var lstValidationKeys = ["CBIUNIQUEIDREQ", "INVALIDCBIID", "INVALIDBILLINGCODE","MINFOURCHAR"];
    this.utilityService.PopulateValidationCollectionFromKeys(lstValidationKeys, this.validationMessages);

    var lstKeysDuplicates=["MINFOURCHAR"];    
    this.utilityService.SubscribeLocalListWithDupNames(
      this.languageTranslationService,
      lstKeysDuplicates
    );

    this.dataSource.next(this.validationMessages);
    this.utilityService.SubscribeValidationMessages(
      this.languageTranslationService,
      this.dataSource
    );
    this.ValidationMsgsObservable.subscribe(result => {
      this.initializeValidationMessages(result);
    });
  }
  initializeValidationMessages(validationMsgs: KeyedCollection<string>) {
    this.Order_flow_validation_messages = {
      cbiUniqueId: [
        { type: "required", message: validationMsgs["CBIUNIQUEIDREQ"] }
        // { type: "pattern", message: validationMsgs[ "INVALIDCBIID"]}
      ],
      // CbiBillingCode: [
      //   { type: "pattern", message: validationMsgs["INVALIDBILLINGCODE"]}
      // ]
      AcctNameOrNumber: [
        { type: "minlength", message: validationMsgs["MINFOURCHAR"] }
      ]

    }
    this.CBIUniqueIDCusMsg = validationMsgs["INVALIDCBIID"];
    this.CBIBillingCodeCusMsg = validationMsgs["INVALIDBILLINGCODE"];
    this.AcctNameOrNumCusMsg = validationMsgs["MINFOURCHAR"];
  }
  ngOnInit() {
    this.setLocalization();
    this.orderInfo = this.OrderFlowService.getOrderInfo();

    this.OrderFlowService.getOrderFlowPackages(this.orderInfo.LocationDetail.LocationID).subscribe(data => {
      this.lstPackages = data;
      if (this.orderInfo.bkgPackageId > 0) {
        this.populateFormControls();
      }
      else {
        this.SelectedPackageID = this.lstPackages[0].BPAId;
      }
    });
    this.OrderFlowService.getServiceDescription().subscribe(values => {
      if (values != null && values != undefined && values != "") {
        this.ServiceDescription = values;
      }
    });

  }

  populateFormControls() {
    var formControls = this.orderFlowPackageForm.controls;
    formControls["cbiUniqueId"].setValue(this.orderInfo.CbiUniqueId);
    this.SelectedPackageID = this.orderInfo.bkgPackageId;
    this.CbiUniqueIdBillingCodeMapping.BillingCode = this.orderInfo.CbiBillingCode;
    this.CbiUniqueIdBillingCodeMapping.CbiUniqueId = this.orderInfo.CbiUniqueId;
    //this.lstCBIUniqueIds = this.orderInfo.lstCBIUniqueIds;
    //formControls["AcctNameOrNumber"].setValue(this.orderInfo.AcctNameOrNum);             
    // if(this.OrderFlowService.selectedCBIUniqueId==this.orderInfo.CbiUniqueId){
    // this.selectedCBIUniqueId = this.OrderFlowService.selectedCBIUniqueId;
    // }
    if (this.orderInfo.CbiBillingCode != "" && this.orderInfo.CbiBillingCode != undefined) {

      this.hdnBillingCode = false;

      formControls["CbiBillingCode"].setValue(this.orderInfo.CbiBillingCode);
    }
    else {
      this.hdnBillingCode = true;
      this.CbiUniqueIdBillingCodeMapping.BillingCode = "";
      this.CbiUniqueIdBillingCodeMapping.CbiUniqueId = "";
    }
  }

  onSubmit() { 
    if (this.orderFlowPackageForm.valid) {
      var OrderFlowPackageObject = this.orderFlowPackageForm.value;
      if (this.orderInfo.CbiUniqueId != undefined && this.orderInfo.CbiUniqueId != OrderFlowPackageObject.cbiUniqueId.trim()) {
        this.OrderFlowService.isOrderlInfoAvailable = false;
      }
      if (!this.hdnBillingCode && OrderFlowPackageObject.CbiBillingCode != undefined && this.CbiUniqueIdBillingCodeMapping.CbiUniqueId != "") {
        if ((!this.hdnBillingCode && OrderFlowPackageObject.CbiBillingCode == "") ||
          (OrderFlowPackageObject.CbiBillingCode.trim().toLowerCase() == this.CbiUniqueIdBillingCodeMapping.BillingCode.toLowerCase()) &&
          OrderFlowPackageObject.cbiUniqueId.trim() == this.CbiUniqueIdBillingCodeMapping.CbiUniqueId) {
          if (this.orderInfo.bkgPackageId != this.SelectedPackageID) {
            this.orderInfo.bkgPackageId = this.SelectedPackageID;
            this.orderInfo.SelectedHierarchyNodeID = this.lstPackages.find(x => x.BPAId == this.SelectedPackageID).InsitutionHierarchyNodeID;
            this.orderInfo.packageName = this.lstPackages.find(x => x.BPAId == this.SelectedPackageID).BPAName;
            this.orderInfo.bkgPkgHierarchyMappingID = this.lstPackages.find(x => x.BPAId == this.SelectedPackageID).BPHMId;
          }
          this.orderInfo.CbiBillingCode = OrderFlowPackageObject.CbiBillingCode.trim();
          if (this.orderInfo.CbiBillingCode != "" && this.orderInfo.CbiBillingCode != null) {
            this.OrderFlowService.getBillingCodeAmount(this.orderInfo.CbiUniqueId, this.orderInfo.CbiBillingCode).subscribe(values => {
              if (values != null && values != undefined && values != "") {
                this.orderInfo.BillingCodeAmount = values;
                this.OrderFlowService.setOrderInfo(this.orderInfo);
                this.OrderFlowService.IncrementStep();
                this.router.navigate(["createOrder/personalInfo"]);
              }
            });
          }
          else {
            this.orderInfo.BillingCodeAmount = "";
            this.OrderFlowService.setOrderInfo(this.orderInfo);
            this.OrderFlowService.IncrementStep();
            this.router.navigate(["createOrder/personalInfo"]);
          }

        }
        else {
          this.isValidBillingCode = false;
        }
      }
      else {
        this.OrderFlowService.validateUniqueCbiId(OrderFlowPackageObject.cbiUniqueId).subscribe(data => {

          this.uniqueIdInfo = data;
          this.isValidCbiUniqueId = this.uniqueIdInfo[0].Value;
          if (this.isValidCbiUniqueId == "True") {
            var cbiBillingCode = this.uniqueIdInfo.find(x => x.Key == "CBIBillingCode");
            this.orderInfo.CbiUniqueId = OrderFlowPackageObject.cbiUniqueId.trim();
            this.orderInfo.bkgPackageId = this.SelectedPackageID;
            this.orderInfo.SelectedHierarchyNodeID = this.lstPackages.find(x => x.BPAId == this.SelectedPackageID).InsitutionHierarchyNodeID;
            this.orderInfo.packageName = this.lstPackages.find(x => x.BPAId == this.SelectedPackageID).BPAName;
            this.orderInfo.bkgPkgHierarchyMappingID = this.lstPackages.find(x => x.BPAId == this.SelectedPackageID).BPHMId;

            this.orderInfo.IsSSNRequired = this.uniqueIdInfo.find(x => x.Key == "IsSSNRequired").Value;
            this.orderInfo.IsLegalNameChange = this.uniqueIdInfo.find(x => x.Key == "IsLegalNameChange").Value;
           // this.orderInfo.lstCBIUniqueIds = this.lstCBIUniqueIds;
           // this.orderInfo.AcctNameOrNum = OrderFlowPackageObject.AcctNameOrNumber;
            this.OrderFlowService.setOrderInfo(this.orderInfo);
            if (cbiBillingCode.Value != undefined && cbiBillingCode.Value != "") {

              this.CbiUniqueIdBillingCodeMapping.BillingCode = cbiBillingCode.Value;
              this.CbiUniqueIdBillingCodeMapping.CbiUniqueId = OrderFlowPackageObject.cbiUniqueId.trim();

              this.hdnBillingCode = false;
            }
            else {
              this.CbiUniqueIdBillingCodeMapping.BillingCode = "";
              this.CbiUniqueIdBillingCodeMapping.CbiUniqueId = "";
              this.OrderFlowService.IncrementStep();
              this.router.navigate(["createOrder/personalInfo"]);
            }

          }
        });
      }
    }
    else {
      this.utilityService.validateAllFormFields(this.orderFlowPackageForm);
    }
  }

  isError(field: string) {
    return this.utilityService.isError(
      this.orderFlowPackageForm,
      field,
      this.Order_flow_validation_messages[field]
    );
  }
  errorMessage(field: string) {
    return this.utilityService.ErrorMessage(
      this.orderFlowPackageForm,
      field,
      this.Order_flow_validation_messages[field]
    );
  }
  isFieldValid(field: string) {
    return this.utilityService.isFieldValid(this.orderFlowPackageForm, field);
  }

  displayFieldCss(field: string) {
    return this.utilityService.displayFieldCss(this.orderFlowPackageForm, field);
  }
  goBack() {
    this.OrderFlowService.DecrementStep();
    switch (this.orderInfo.selectedOrderType) {
      case '1':
        this.router.navigate(["createOrder/location"]);
        break;
      case '2':
        this.router.navigate(["createOrder/EventDetailConfirm"]);
        break;
      case '3':
        this.router.navigate(["createOrder/OutOfState"]);
        break;
      default:
    }
  }

  hideBillingCode() {

    if (!this.hdnBillingCode) {
      this.CbiUniqueIdBillingCodeMapping.BillingCode = "";
      this.CbiUniqueIdBillingCodeMapping.CbiUniqueId = "";
      this.orderInfo.CbiBillingCode = "";
      this.orderInfo.BillingCodeAmount = "";
      this.orderInfo.IsPaymentByInstAlone = false;
      this.orderInfo.IsPaymentByInst = false;
      this.orderFlowPackageForm.controls["CbiBillingCode"].setValue("");
      this.hdnBillingCode = true;
      this.OrderFlowService.setOrderInfo(this.orderInfo);
    }
  }

  LookUp() {    
    this.lstCBIUniqueIds = undefined;    
    var OrderFlowPackageObject = this.orderFlowPackageForm.value;
    var formControls = this.orderFlowPackageForm.controls;
    //formControls["cbiUniqueId"].setValue("");
    if (OrderFlowPackageObject.AcctNameOrNumber != "") {
      if(OrderFlowPackageObject.AcctNameOrNumber.length > 3){
      this.OrderFlowService.GetCBIUniqueIdByAcctNameOrNumber(OrderFlowPackageObject.AcctNameOrNumber).subscribe(values => {
        if (values.length != undefined) {
          this.lstCBIUniqueIds = values;          
        }
      });
    }
    } else {
      this.isValidAcctNameOrNum = false;
    }
  }

  onCBIUniqueIdChange(selectedCBIId: string) {
    if (selectedCBIId == "Please Select" || selectedCBIId == "Seleccione") {
      var formControls = this.orderFlowPackageForm.controls;
      formControls["cbiUniqueId"].setValue("");
    } else {
      var formControls = this.orderFlowPackageForm.controls;
      formControls["cbiUniqueId"].setValue(selectedCBIId);
      //this.OrderFlowService.selectedCBIUniqueId = selectedCBIId;
    }
  }

  onValChange(newValue: string) {
    this.isValidAcctNameOrNum = true;
  }
}
