import { Component, OnInit, OnDestroy, TemplateRef } from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { UserInfo, PersonAliasContract } from "../../../models/user/user.model";
import { Router } from "@angular/router";
import { OrderInfo } from "../../../models/order-flow/order.model";
import { AttributesForCustomForm } from "../../../models/custom-forms/custom-attribute";
import { Validators } from "@angular/forms";
import { UtilityService } from "../../../services/shared-services/utility.service";
import { DatePipe } from '@angular/common';
import { ModalContentComponent } from "../../confirmation-box/confirmation-modal.component";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { CommonService } from "../../../services/shared-services/common.service";
import { KeyedCollection } from "../../../models/common/dictionary.model";
import { conformToMask } from "angular2-text-mask";
import { Observable, BehaviorSubject } from "rxjs";
import { basename } from "path";
import { AppConsts } from "../../../../environments/constants/appConstants";
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
@Component({
  selector: "order-review",
  templateUrl: "order-review.component.html"
})
export class OrderReviewComponent implements OnInit, OnDestroy {
  refresh: any;
  IsImageError: boolean;
  locationID: number;
  refreshMsg: any;
  locationName: String;
  NoImgMsg: any;
  Locations: any;
  public mask = ['(', /[0-9]/, /\d/, /\d/, ')', '-', /\d/, /\d/, /\d/, '-', /\d/, /\d/, /\d/, /\d/]
  IsNextButtonDisable: boolean = true;
  IsZipCodeLabel: boolean = true;
  orderReviewForm = this.formBuilder.group({
    confirmCheckbox: ["", Validators.requiredTrue]
  });

  constructor(private formBuilder: FormBuilder, private orderFlowService: OrderFlowService, private router: Router,
    private utilityService: UtilityService,
    private datepipe: DatePipe, private modalService: BsModalService,
    private languageTranslationService: LanguageTranslationService,
    private commonService: CommonService,private modelService:BsModalService
  ) {
    this.modalService.onHide.subscribe((result: any) => {
      if (document.getElementById('YES') != undefined) {
        if (result === document.getElementById('YES').textContent) {
          this.confirmPopUpClick();
        }
      } { }
    });
  }
  modalRef: BsModalRef;
  userInfo: UserInfo;
  lstAlias: Array<PersonAliasContract>;
  lstCustomAttribute: Array<AttributesForCustomForm>;
  lstCustomAttributeGroup: Array<{ EditLabel: string, GroupName: string, Attributes: Array<AttributesForCustomForm> }>;
  orderInfo: OrderInfo;
  FirstName: string;
  MiddleName: string;
  LastName: string;
  Gender: string = "";
  DOB: string;
  SSN: string;
  PrimaryEmail: string;
  SecondaryEmail: string;
  PrimaryPhone: string;
  SecondaryPhone: string;
  Address: string;
  Country: string;
  State: string;
  City: string;
  ZipCode: string;
  PackagePrice: string;
  PackageName: string;
  AppointmentName: string = "";
  AppointmentTime: string = "";
  AppointmentAddress: string = "";
  Description: string = "";
  HideShowOutOfState: boolean = true;
  HideShowNonOutOfState: boolean = true;
  HideShowAppointmentEdit: boolean = true;
  HideSSN: boolean = false;
  HideState: Boolean = false;
  order_review_validation_messages: any;
  validationMessages: KeyedCollection<string> = new KeyedCollection<string>();
  dataSource = new BehaviorSubject<KeyedCollection<string>>(
    new KeyedCollection<string>()
  );
  ValidationMsgsObservable = this.dataSource.asObservable();
  PaymentByInstitutionPrice: string;
  BalanceAmountPrice: string;
  HidePaymentByInst: boolean = true;
  HideBalAmount: boolean = true;
  IsEventType: boolean = false;
  setLocalization() {
    var lstKeys = ["CREATODR", "STEP", "ORDCON_ORDSELDTLS", "CHNGORDERSELECTION", "ORDERSELECTION", "MAILINPROCESS",
      "TOTALPRICE", "OF", "PROFILEDETAILS", "FIRSTNAME", "MIDDLENAME", "LASTNAME", "ALIASFIRSTNAME",
      "ALIASMIDDLENAME", "PLZENTRALSLASTNAME", "GENDER", "DOB", "EMAILADD", "SECEMAIL", "PHONENUM", "SECPHONE",
      "COUNTRY", "STATE", "APPMNTDETAILS", "APPMNTSTATUS", "NAME", "APPMNTTIME", "DESCRIPTION",
      "PRVCYACTSTMNT", "ORDREVPRVCYSTMNT_PONE", "ORDREVPRVCYSTMNT_PTWO", "ORDREVPRVCYSTMNT_PTHREE", "ORDREVPRVCYSTMNT_PFOUR",
      "READANDACCPTPRVCYACTSTMNT", "PREVIOUS", "NEXT", "CNFMCNLORDER", "ALIASSUFFIX", "ALIASLASTNAME", "CNCLORDR", "YES",
      "LSTITMN", "CNCL", "CONFIRMMSGCABS", "OK", "ORDERREVIEWTITLE", "BALANCEAMT", "PAYMENTBYINST","VWLOCIMAGES","IMGSNOTAVAIL","RFRESHMSG","REFRESH"];

    this.utilityService.SubscribeLocalList(
      this.languageTranslationService,
      lstKeys
    );
    var lstKeysDuplicates = ["EDIT", "SSN", "CITY", "ZIPCODE", "ADDRESS", "POSTALCODE", "SUFFIX", "ALIASSUFFIX"];
    this.utilityService.SubscribeLocalListWithDupNames(
      this.languageTranslationService,
      lstKeysDuplicates
    );

    var lstValidationKeys = ["CRDCODE","IMGSNOTAVAIL","RFRESHMSG","REFRESH"];
    this.utilityService.PopulateValidationCollectionFromKeys(lstValidationKeys, this.validationMessages);

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
    this.order_review_validation_messages = {
      confirmCheckbox: [{ type: "confirmCheckbox", message: validationMsgs["CRDCODE"] }],
    }
    this.NoImgMsg=validationMsgs["IMGSNOTAVAIL"];
    this.refreshMsg=validationMsgs["RFRESHMSG"];
    this.refresh=validationMsgs["REFRESH"];
  }

  setCustomFormsLocalization(isSpanishLanguage: boolean) {
    this.lstCustomAttributeGroup = new Array<{ EditLabel: string, GroupName: string, Attributes: Array<AttributesForCustomForm> }>();
    if (isSpanishLanguage) {
      this.orderInfo.lstCustomAttributeInSpanish.forEach(element => {
        var res = this.lstCustomAttributeGroup.find(z => z.GroupName == element.SectionTitle);
        var sectiontitleInEnglish = this.lstCustomAttribute.filter(x => x.AttributeGroupId == element.AttributeGroupId)[0].SectionTitle;
        if (res == null) {
          this.lstCustomAttributeGroup.push({ 'EditLabel': sectiontitleInEnglish, 'GroupName': element.SectionTitle, 'Attributes': this.orderInfo.lstCustomAttributeInSpanish.filter(x => x.AttributeGroupId == element.AttributeGroupId && !x.IsHiddenFromUI) });
        }
      });

      this.orderFlowService.GetLocalizedGenderName(this.orderInfo.userInfo.SelectedGenderDefaultKeyID, 'AAAB').subscribe((values: any) => {
        if (values != undefined) {
          this.Gender = values;
        }
      }); 
    }
    else {
      this.lstCustomAttribute.forEach(element => {
        var res = this.lstCustomAttributeGroup.find(z => z.GroupName == element.SectionTitle);
        if (res == null) {
          this.lstCustomAttributeGroup.push({ 'EditLabel': element.SectionTitle, 'GroupName': element.SectionTitle, 'Attributes': this.lstCustomAttribute.filter(x => x.AttributeGroupId == element.AttributeGroupId && !x.IsHiddenFromUI) });
        }
      });
      this.orderFlowService.GetLocalizedGenderName(this.orderInfo.userInfo.SelectedGenderDefaultKeyID, 'AAAA').subscribe((values: any) => {
        if (values != undefined) {
          this.Gender = values;
        }
      });
    }
  }
  ngOnInit() {

    this.orderInfo = this.orderFlowService.getOrderInfo();
    this.setLocalization()
    this.populateFormControls();
    this.lstCustomAttribute = this.orderFlowService.getCustomAttributeInfo();
    this.orderInfo.lstCustomAttributeInSpanish.forEach(element => {
      element.AttributeDataValue = this.lstCustomAttribute.filter(x => x.AttributeGroupId == element.AttributeGroupId
        && x.AttributeId == element.AttributeId)[0].AttributeDataValue;
    });

    this.commonService.isLanguageSpanish.subscribe(isSpanishLang => {
      this.setCustomFormsLocalization(isSpanishLang);
    });


    this.orderFlowService.getPrice().subscribe(data => {

      this.PackagePrice = data;
      this.orderInfo.IsPaymentByInstAlone = false;
      this.orderInfo.IsPaymentByInst = false;
      if (this.orderInfo.CbiBillingCode != "" && this.orderInfo.CbiBillingCode != null) {
        if (this.orderInfo.BillingCodeAmount != undefined && this.orderInfo.BillingCodeAmount != null && this.orderInfo.BillingCodeAmount != "") {
          this.HidePaymentByInst = false;
          this.HideBalAmount = false;
          if (this.PackagePrice <= this.orderInfo.BillingCodeAmount) {
            this.PaymentByInstitutionPrice = this.PackagePrice;
            this.BalanceAmountPrice = "0";
            this.orderInfo.IsPaymentByInstAlone = true;
            this.orderInfo.IsPaymentByInst = true;
          }
          else {
            this.PaymentByInstitutionPrice = this.orderInfo.BillingCodeAmount;
            var balAmount = parseFloat(this.PackagePrice) - parseFloat(this.orderInfo.BillingCodeAmount);
            this.BalanceAmountPrice = balAmount.toString();
            this.orderInfo.IsPaymentByInst = true;
            this.orderInfo.IsPaymentByInstAlone = false;
          }
        }
      }

    });
    document.getElementById("CONFIRMMSGCABS").style.display = 'none';
    document.getElementById("YES").style.display = 'none';
    document.getElementById("LSTITMN").style.display = 'none';
  }
  ngOnDestroy() {
    //this.commonService.isLanguageSpanish=null;
  }

  populateFormControls() {
    if (this.orderInfo.userInfo != undefined) {
      this.userInfo = this.orderInfo.userInfo;
      this.FirstName = this.userInfo.FirstName;
      this.MiddleName = this.userInfo.MiddleName;
      this.LastName = this.userInfo.LastName;
      this.Gender = this.userInfo.Gender;
      this.lstAlias = this.userInfo.PersonAliasList;
      this.DOB = this.userInfo.DOB;
      this.SSN = this.userInfo.SSN;
      if (this.SSN == "") {
        this.HideSSN = true;
      }

      var primaryPhone = conformToMask(
        this.userInfo.PrimaryPhone,
        this.mask,
        { guide: false }
      )

      var secondaryPhone = conformToMask(
        this.userInfo.SecondaryPhone,
        this.mask,
        { guide: false }
      )

      this.PrimaryEmail = this.userInfo.PrimaryEmail;
      this.SecondaryEmail = this.userInfo.SecondaryEmail;
      this.PrimaryPhone = primaryPhone.conformedValue;
      this.SecondaryPhone = secondaryPhone.conformedValue;
      this.Address = this.userInfo.Address;
      this.Country = this.userInfo.CountryName;
      if (
        this.Country == "CANADA" ||
        this.Country == "UNITED STATES of AMERICA" ||
        this.Country == "UNITED STATES of AMERICA - STATE" ||
        this.Country == "MEXICO"
      ) {

        this.HideState = false;
        this.IsZipCodeLabel = true;
      }
      else {

        this.HideState = true;
        this.IsZipCodeLabel = false;
      }
      this.State = this.userInfo.StateName;
      this.City = this.userInfo.CityName;
      this.ZipCode = this.userInfo.PostalCode;
      this.PackageName = this.orderInfo.packageName;
    }
    if (this.orderInfo.LocationDetail != undefined) {
      //Event Order Type
      if (this.orderInfo.selectedOrderType == "2") {
        this.AppointmentName = this.orderInfo.LocationDetail.EventName;
        this.AppointmentAddress = this.orderInfo.LocationDetail.LocationAddress;
        this.AppointmentTime = this.datepipe.transform(this.orderInfo.LocationDetail.SlotDate, "MM/dd/yyyy") + " (" + this.datepipe.transform(this.orderInfo.LocationDetail.StartTime, "h:mm a") + " - " + this.datepipe.transform(this.orderInfo.LocationDetail.EndTime, "h:mm a") + ")";
        this.Description = this.orderInfo.LocationDetail.EventDescription;
        this.HideShowOutOfState = true;
        this.HideShowNonOutOfState = false;
        this.HideShowAppointmentEdit = true;
        this.IsEventType = true;

      }
      //Out of state order type
      else if (this.orderInfo.selectedOrderType == "3") {
        this.HideShowOutOfState = false;
        this.HideShowNonOutOfState = true;
        this.HideShowAppointmentEdit = true;
      }
      //Location order type
      else if (this.orderInfo.selectedOrderType == "1") {
        this.AppointmentName = this.orderInfo.LocationDetail.LocationName;
        this.AppointmentAddress = this.orderInfo.LocationDetail.LocationAddress;
        this.AppointmentTime = this.datepipe.transform(this.orderInfo.LocationDetail.SlotDate, "MM/dd/yyyy") + " (" + this.datepipe.transform(this.orderInfo.LocationDetail.StartTime, "h:mm a") + " - " + this.datepipe.transform(this.orderInfo.LocationDetail.EndTime, "h:mm a") + ")";
        this.Description = this.orderInfo.LocationDetail.LocationDescription;
        this.HideShowOutOfState = true;
        this.HideShowNonOutOfState = false;
        this.HideShowAppointmentEdit = false;
      }
    }
  }
  SetEnableDisable(e: any) {
    if (e.target.checked)
      this.IsNextButtonDisable = false;
    else
      this.IsNextButtonDisable = true;
  }
  onSubmit() {
    if (this.orderReviewForm.valid) {

      const initialState = {

        message: document.getElementById('CONFIRMMSGCABS').textContent,
        title: '',
        confirmBtnName: document.getElementById('YES').textContent,
        closeBtnName: document.getElementById('LSTITMN').textContent,
      };
      this.modalService.show(ModalContentComponent, { initialState });
    }
    else {
      this.utilityService.validateAllFormFields(this.orderReviewForm);
    }
  }

  confirmPopUpClick() {
    this.orderFlowService.setTotalPackagePriceInfo(this.PackagePrice);
    this.router.navigate(["createOrder/PaymentDetail"]);
  }

  goBack() {
    this.orderFlowService.DecrementStep();
    if (this.orderInfo.selectedOrderType == "1") {
      this.router.navigate(["createOrder/scheduleslot"]);
    }
    else {
      if (this.orderInfo.isEmpoyerDetailsMendatory) {
        this.router.navigate(["createOrder/MailingInfo"]);
      }
      else {
        this.router.navigate(["createOrder/serviceDetails"]);
      }
    }
  }

  goToPackageSelectionScreen() {
    //this.OrderFlowService.setOrderInfoFeildsEmpty();
    this.orderInfo.CurrentStep = 2;
    this.router.navigate(["createOrder/Orderflowpackages"]);
  }

  isError(field: string) {
    return this.utilityService.isError(this.orderReviewForm, field, this.order_review_validation_messages[field]);
  }
  errorMessage(field: string) {
    return this.utilityService.ErrorMessage(this.orderReviewForm, field, this.order_review_validation_messages[field]);
  }

  isFieldValid(field: string) {
    return this.utilityService.isFieldValid(this.orderReviewForm, field);
  }

  displayFieldCss(field: string) {
    return this.utilityService.displayFieldCss(this.orderReviewForm, field);
  }

  switchScreen(screenCriteria: string) {
    //route to given path
    switch (screenCriteria) {
      case 'EditProfile':
        this.orderFlowService.IsDobAvailable = true;
        this.orderInfo.CurrentStep = 3;        
        this.orderFlowService.setOrderInfo(this.orderInfo);
        this.router.navigate(["createOrder/personalInfo"]);
        break;
      case 'Service Details':
        this.orderInfo.CurrentStep = 4;
        this.orderFlowService.setOrderInfo(this.orderInfo);
        this.router.navigate(["createOrder/serviceDetails"]);
        break;
      case 'Information for Finger Printing':
        this.orderInfo.CurrentStep = 4;
        this.orderFlowService.setOrderInfo(this.orderInfo);
        this.router.navigate(["createOrder/fingerPrinting"]);
        break;
      case 'Employer Details':
      case 'Business Address':
      case 'Facility Address':
      case 'Mailing Address':
        this.orderInfo.CurrentStep = 4;
        this.orderFlowService.setOrderInfo(this.orderInfo);
        this.router.navigate(["createOrder/MailingInfo"]);
        break;
      case 'EditAppointment':
        this.orderInfo.CurrentStep = 5;
        this.orderFlowService.setOrderInfo(this.orderInfo);
        this.router.navigate(["createOrder/schedulecalender"]);
        break;
      default:
    }

   
      
  }
  openModal(template: TemplateRef<any>,locationName:String) {
    this.locationID=this.orderInfo.LocationDetail.LocationID;
    this.locationName=locationName;
    this.orderFlowService.getLocationImages(this.orderInfo.LocationDetail.LocationID).subscribe(s => {
      console.log(s);
      if (s != -1) {
        this.Locations = s;
        this.IsImageError=false;
      }
      else
      {
        this.IsImageError=true;
        this.Locations=null;
      }
    
      this.modalRef = this.modalService.show(template);
    });
  }
  refreshModel()
  {
    this.orderFlowService.getLocationImages(this.locationID).subscribe(s => {
      console.log(s);
      if (s != -1) {
        this.Locations = s;
        this.locationName = this.locationName;
        this.IsImageError=false;
      }
      else
      {
        this.IsImageError=true;
        this.Locations=null;
      }
  
    });
  }

}
