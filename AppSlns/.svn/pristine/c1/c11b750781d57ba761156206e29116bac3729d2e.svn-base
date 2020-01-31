import { Injectable } from "@angular/core";
import { Observable, Observer } from "rxjs";
import "rxjs/add/observable/of";
import "rxjs/add/operator/map";

/*User defined service*/
import { HttpService } from "../shared-services/http.service";
import { ContentType } from "../../models/common/content-type.model";
import { environment } from "../../../environments/environment";

import {
  OrderInfo,
  ApplicantOrderPaymentOptions,
  LocationDetail,
  AuthorizeNetInfo,
  OrderDetail,
  RescheduleAppointmentInfo,
  ScheduleSlots
} from "../../models/order-flow/order.model";

import {
  AttributesForCustomForm,
  mailingInfoAtt
} from "../../models/custom-forms/custom-attribute";

import { UserInfo, UserContract } from "../../models/user/user.model";
import { userInfo } from "os";
import { map } from "rxjs/operators";
import {
  LanguageTranslation,
  LanguageTranslationContract,
  LanguageParams
} from "../../models/language-translation/language-translation-contract.model";
@Injectable()
export class OrderFlowService {
  lstCards: any;
  DefaultZipcode: any;
  constructor(
    public httpService: HttpService,
    public contentType: ContentType
  ) { }
  private languageParams: LanguageParams = new LanguageParams();
  private OrderInfo: OrderInfo = new OrderInfo();
  private aleadyPlacedOrder: OrderInfo = new OrderInfo();
  private orderDetails: Array<OrderDetail>;
  isOrderlInfoAvailable: boolean = false;
  lstLocations = new Array<any>();
  private userInfo: UserInfo = new UserInfo();
  isPersonalInfoAvailable: boolean = false;
  isAddresslInfoAvailable: boolean = false;
  isContactlInfoAvailable: boolean = false;
  isAliasInfoAvailable: boolean = false;
  hideShowAliasButton: boolean = true;
  IsEditProfile: boolean = false;
  IsPasswordExpiry: boolean = false;
  EditProfileEmail: string = "";
  IsDobAvailable: boolean = false;
  DobAvailableValue: string = "";
  selectedCBIUniqueId: string;
  
  //AlreadyPlacedOrders
  isOrderTypeChange: boolean = false;
  isLocationChange: boolean = false;
  isOrderAlreadyPlaced: boolean = false;
  isSendForOnline: boolean = false;
  private selectedOrderDetail: OrderDetail;
  ErrorMsg: string = "";
  SuccessMsg: string = "";
  IsSuffixDropDown: boolean = false;

  private userContract: UserContract = new UserContract();
  getUserContract(): UserContract {
    this.populateUserInfo();
    this.userContract.SelectedGenderId = this.userInfo.SelectedGenderId;
    this.userContract.PrimaryEmail = this.userInfo.PrimaryEmail;
    this.userContract.SecondaryEmail = this.userInfo.SecondaryEmail;
    this.userContract.SSN = this.userInfo.SSN;
    this.userContract.UserName = this.userInfo.UserName;
    this.userContract.OrganizationUserID =
      this.userInfo.OrganizationUserID != undefined
        ? this.userInfo.OrganizationUserID.toString()
        : "";
    this.userContract.UserID = this.userInfo.UserID;
    this.userContract.InstitutionName = this.userInfo.InstitutionName;
    this.userContract.ApplicantFirstName = this.userInfo.ApplicantFirstName;
    this.userContract.ApplicantLastName = this.userInfo.ApplicantLastName;
    this.userContract.DOB = this.userInfo.DOB;
    this.userContract.PrimaryPhoneNumber = this.userInfo.PrimaryPhoneNumber;
    this.userContract.Password = this.userInfo.Password;
    this.userContract.FirstName = this.userInfo.FirstName;
    this.userContract.FirstName = this.userInfo.FirstName;
    this.userContract.MiddleName = this.userInfo.MiddleName;
    this.userContract.LastName = this.userInfo.LastName;
    this.userContract.FilePath = this.userInfo.FilePath;
    this.userContract.OriginalFileName = this.userInfo.OriginalFileName;
    this.userContract.PrimaryPhone = this.userInfo.PrimaryPhone;
    this.userContract.SecondaryPhone = this.userInfo.SecondaryPhone;
    this.userContract.SelectedCommLang = this.userInfo.SelectedCommLang;
    this.userContract.ErrorMessage = this.userInfo.ErrorMessage;
    this.userContract.IsAutoActive = this.userInfo.IsAutoActive;
    this.userContract.MasterZipcodeID = this.userInfo.MasterZipcodeID;
    this.userContract.CountryId = this.userInfo.CountryId;
    this.userContract.Address = this.userInfo.Address;
    this.userContract.ZipId = this.userInfo.ZipId;
    if (this.userInfo.StateName == "SITI@4863") {
      this.userContract.StateName = "";
    } else {
      this.userContract.StateName = this.userInfo.StateName;
    }
    this.userContract.CityName = this.userInfo.CityName;
    this.userContract.PostalCode = this.userInfo.PostalCode;
    this.userContract.IsLocationServiceTenant = this.userInfo.IsLocationServiceTenant;
    this.userContract.NoMiddleNameText = this.userInfo.NoMiddleNameText;
    this.userContract.IsMaskingOfPrimaryPhoneNumber = this.userInfo.IsMaskingOfPrimaryPhoneNumber;
    this.userContract.IsMaskingOfSecondaryPhoneNumber = this.userInfo.IsMaskingOfSecondaryPhoneNumber;
    this.userContract.SelectedSuffixID = this.userInfo.SelectedSuffixID;
    this.userContract.Suffix = this.userInfo.Suffix;
    this.userContract.PersonAliasList = this.userInfo.PersonAliasList;
    return this.userContract;
  }
  public GetLanguageParams(): LanguageParams {
    return this.languageParams;
  }

  public SetLanguageParams(value: LanguageParams) {
    this.languageParams = value;
  }
  populateUserInfo() {
    //personal info
    this.userInfo.ApplicantFirstName = this.userInfo.PersonalInfo.FirstName;
    this.userInfo.ApplicantLastName = this.userInfo.PersonalInfo.LastName;
    this.userInfo.FirstName = this.userInfo.PersonalInfo.FirstName;
    this.userInfo.MiddleName = this.userInfo.PersonalInfo.MiddleName;
    this.userInfo.LastName = this.userInfo.PersonalInfo.LastName;
    this.userInfo.DOB = this.userInfo.PersonalInfo.DOB;
    this.userInfo.SelectedGenderId = parseInt(
      this.userInfo.PersonalInfo.GenderID
    );
    //aliases

    //address
    this.userInfo.Address = this.userInfo.AddressInfo.Address;
    this.userInfo.CityName = this.userInfo.AddressInfo.City;
    this.userInfo.CountryId = parseInt(this.userInfo.AddressInfo.CountryId);
    this.userInfo.StateName = this.userInfo.AddressInfo.StateName;
    this.userInfo.PostalCode = this.userInfo.AddressInfo.ZipCode;
    // contact
    this.userInfo.PrimaryEmail = this.userInfo.ContactInfo.PrimaryEmail;
    this.userInfo.SecondaryEmail = this.userInfo.ContactInfo.SecondaryEmail;
    this.userInfo.PrimaryPhone = this.userInfo.ContactInfo.PrimaryPhone;
    this.userInfo.SecondaryPhone = this.userInfo.ContactInfo.SecondaryPhone;
    this.userInfo.UserName = this.userInfo.ContactInfo.UserName;
    this.userInfo.Password = this.userInfo.ContactInfo.Password;
    this.userInfo.PersonAliasList = this.userInfo.PersonAliasList;
  }
  getServiceDetails(pkgId: number, CBIUniId: string, LangCode: string) {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl +
          environment.GetServiceDetails +
          "/" +
          pkgId +
          "/" +
          CBIUniId +
          "/" +
          LangCode
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }
  getServiceDescription(): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(environment.baseApiUrl + environment.GetServiceDescription)
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  setOrganizationUserInfoDetails(userInfo: UserInfo) {
    this.OrderInfo.userInfo = userInfo;
  }

  getOrganizationUserInfoDetails() {
    if (
      this.OrderInfo.userInfo != undefined &&
      this.OrderInfo.userInfo.OrganizationUserID > 0
    ) {
      return this.OrderInfo.userInfo;
    } else {
      this.getOrganizationUser().then(res => {
        this.OrderInfo.userInfo.PersonAliasList.forEach(element => {
          element.IdForIndex = element.ID;
        });
        this.setOrderInfo(this.OrderInfo);
      });
    }
  }
  getOrgUsrInfoDetForAlredyPlacedOrder() {

    if (
      this.aleadyPlacedOrder.userInfo != undefined &&
      this.aleadyPlacedOrder.userInfo.OrganizationUserID > 0
    ) {
      return this.aleadyPlacedOrder.userInfo;
    } else {
      this.getOrgUsrForAlredyPlacedOrder().then(res => {

        this.aleadyPlacedOrder.userInfo.PersonAliasList.forEach(element => {
          element.IdForIndex = element.ID;
        });
        this.setAlredyPlacedOrderInfo(this.aleadyPlacedOrder);
      });
    }
  }
  setCreateOrderInfo(orderInfo: OrderInfo) {
    this.OrderInfo.selectedOrderType = orderInfo.selectedOrderType;
  }
  private getRawResult(apiResponse: any): any[] {
    return apiResponse;
  }
  GetLocations(lng: string, lat: string) {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl +
          environment.GetLocations +
          "/" +
          lng +
          "/" +
          lat +
          "/"
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  GetLocation(lng: string, lat: string) {
    let promise = new Promise((resolve, reject) => {
      this.httpService
        .get(
          environment.baseApiUrl +
          environment.GetLocations +
          "/" +
          lng +
          "/" +
          lat +
          "/"
        )
        .toPromise()
        .then(res => {
          // Success
          this.lstLocations = res;
          resolve();
        });
    });
    return promise;
  }
  // populateMailinginfo(): Array<AttributesForCustomForm> {
  //   return this.OrderInfo.lstCustomAttribute;
  // }
  // setMailinginfo(mailinginfo: MailingDetails) {
  //   this.OrderInfo.mailinginfo = mailinginfo
  // }

  setCustomFromAttributeInfo(AttributesForCustomForm: AttributesForCustomForm) {
    var res = this.OrderInfo.lstCustomAttribute.find(
      x => x.AttributeId == AttributesForCustomForm.AttributeId
    );
    if (res != null) {
      var index = this.OrderInfo.lstCustomAttribute.indexOf(res);
      this.OrderInfo.lstCustomAttribute[index] = AttributesForCustomForm;
    } else {
      this.OrderInfo.lstCustomAttribute.push(AttributesForCustomForm);
    }
  }

  getCustomAttributeInfo(): Array<AttributesForCustomForm> {
    return this.OrderInfo.lstCustomAttribute;
  }
  setmailingAttributeInfo(MalingAttribute: mailingInfoAtt) {
    this.OrderInfo.lstMailingAttribute.push(MalingAttribute);
  }

  // getMailingAttributeInfo(): Array<mailingInfoAtt> {
  //   return this.OrderInfo.lstMailingAttribute;
  // }
  validateEventCode(eventCode: string, langCode: string): Observable<any> {
    var orderInfo: OrderInfo = new OrderInfo();
    orderInfo.LocationDetail.EventCode = eventCode;
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .post(
          environment.baseApiUrl + environment.ValidateEventCodeStatusAndEventDetails + '/' + langCode,
          orderInfo,
          this.contentType.jsonType
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }
  getEventCode() {
    return this.OrderInfo.LocationDetail.EventCode;
  }
  setEventCode(eventCode: string) {
    this.OrderInfo.LocationDetail.EventCode;
  }
  setLocationId(locationId: number) {
    this.OrderInfo.LocationDetail.LocationID = locationId;
  }

  getOrderFlowPackages(LocationId: number): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl +
          environment.GetOrderFlowPackages +
          "/" +
          LocationId
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  validateUniqueCbiId(CBIUniId: string): Observable<any> {
    var orderInfo: OrderInfo = new OrderInfo();
    orderInfo.CbiUniqueId = CBIUniId;
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .post(
          environment.baseApiUrl + environment.ValidateUniqueCbiId,
          orderInfo,
          this.contentType.jsonType
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  getOrganizationUser(): Promise<any> {
    let promise = new Promise((resolve, reject) => {
      this.httpService
        .get(environment.baseApiUrl + environment.GetOrganizationUser)
        .toPromise()
        .then(res => {
          // Success
          if (this.OrderInfo.userInfo == undefined) {
            this.OrderInfo.userInfo = new UserInfo();
          }
          this.OrderInfo.userInfo = res;
          resolve();
        });
    });
    return promise;
  }
  getOrgUsrForAlredyPlacedOrder(): Promise<any> {
    let promise = new Promise((resolve, reject) => {
      this.httpService
        .get(environment.baseApiUrl + environment.GetOrganizationUser)
        .toPromise()
        .then(res => {
          // Success
          if (this.aleadyPlacedOrder.userInfo == undefined) {
            this.aleadyPlacedOrder.userInfo = new UserInfo();
          }
          this.aleadyPlacedOrder.userInfo = res;
          resolve();
        });
    });
    return promise;
  }

  setTotalPackagePriceInfo(PackagePrice: string) {
    this.OrderInfo.GrandTotal = PackagePrice;
    this.OrderInfo.TotalPrice = PackagePrice;
  }

  getOutOfStateLocation(): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(environment.baseApiUrl + environment.GetOutOfStateLocation)
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  GetDefaultZipcode(): Promise<any> {
    let promise = new Promise((resolve, reject) => {
      this.httpService
        .get(environment.baseApiUrl + environment.GetDefaultZipcode)
        .toPromise()
        .then(res => {
          // Success
          this.OrderInfo.DefaultZipCode = res;
          resolve();
        });
    });
    return promise;
  }

  getMailingInfo(langCode: string) {
    var jsonStringData = [];
    for (var i = 0; i < this.OrderInfo.lstCustomAttribute.length; i++) {
      jsonStringData.push({
        //  "Attribute": [{
        InstanceID: this.OrderInfo.lstCustomAttribute[i].InstanceID,
        AttributeID: this.OrderInfo.lstCustomAttribute[i]
          .AtrributeGroupMappingId,
        AttributeValue: this.OrderInfo.lstCustomAttribute[i].AttributeDataValue
        // }]
      });
    }
    var contentType = new ContentType();
    var jsonData = JSON.stringify(jsonStringData);
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .post(
          environment.baseApiUrl + environment.GetMailingInfo + "/" + langCode,
          jsonData,
          contentType.jsonType
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  getPaymentOptions(
    dppIds: number,
    bphmIds: string,
    dpmId: number,
    validBC: boolean
  ): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl +
          environment.GetPaymentOptions +
          "/" +
          dppIds +
          "/" +
          bphmIds +
          "/" +
          dpmId +
          "/" +
          validBC +
          "/"
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  getOrderInfo() {
    if (
      this.OrderInfo.userInfo.OrganizationUserID == undefined ||
      this.OrderInfo.userInfo.OrganizationUserID <= 0
    ) {
      this.getOrganizationUserInfoDetails();
    }
    return this.OrderInfo;
  }
  setOrderInfo(orderInfo: OrderInfo) {
    this.OrderInfo = orderInfo;
  }
  setAlredyPlacedOrderInfo(aleadyPlacedOrder: OrderInfo) {
    this.aleadyPlacedOrder = aleadyPlacedOrder;
  }
  getInstructionTexts(
    billingCodeAmt: string,
    isBillingCode: boolean
  ): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl +
          environment.GetInstructionText +
          "/" +
          billingCodeAmt +
          "/" +
          isBillingCode +
          "/"
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }
  getAgreementText(languageCode: string): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl +
          environment.GetAgreementText +
          "/" +
          languageCode
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  getPrice(): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .post(
          environment.baseApiUrl + environment.GetPrice,
          this.getOrderInfo(),
          this.contentType.jsonType
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }
  SetSelectedPaymentModeData(paymentDetail: ApplicantOrderPaymentOptions): any {
    this.OrderInfo.selectedPaymentModeData.push(paymentDetail);
  }

  GetOrderHistoryDetails() {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .post(
          environment.baseApiUrl + environment.GetOrderHistoryDetails,
          this.GetOrderHistoryDetails(),
          this.contentType.jsonType
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  GetSelectedZipCode(): any {
    return this.DefaultZipcode;
  }
  SetZipCode(zipcode: string) {
    this.DefaultZipcode = zipcode;
  }

  GetAppointmentSlots(locationId: number): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl +
          environment.GetAppointmentSlots +
          "/" +
          locationId
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  SubmitOrder(orderInfo: OrderInfo): Promise<any> {
    let promise = new Promise((resolve, reject) => {
      this.httpService
        .post(
          environment.baseApiUrl + environment.SubmitOrder,
          orderInfo,
          this.contentType.jsonType
        )
        .toPromise()
        .then(res => {
          // Success
          this.OrderInfo.OrderNumber = res.ResponseObject;
          resolve();
        });
    });
    return promise;
  }

  CompleteApplicantOrder(orderInfo: OrderInfo): Promise<any> {
    let promise = new Promise((resolve, reject) => {
      this.httpService
        .post(
          environment.baseApiUrl + environment.CompleteApplicantOrder,
          orderInfo,
          this.contentType.jsonType
        )
        .toPromise()
        .then(res => {
          // Success
          resolve();
        });
    });
    return promise;
  }

  IsReservedSlotExpired(reservedSlotId:number,isCCPayment:boolean): Promise<any> {
    let promise = new Promise((resolve, reject) => {
      this.httpService
        .get(
          environment.baseApiUrl + environment.IsReservedSlotExpired +'/'+reservedSlotId +'/'+isCCPayment
        )
        .toPromise()
        .then(res => {
          // Success
          resolve(res);
        });
    });
    return promise;
  }
  

  setOrderInfoToEmpty() {    
    this.OrderInfo = new OrderInfo();    
  }

  setOrderInfoFeildsEmpty() {
    var selectedOrderType = this.OrderInfo.selectedOrderType;
    var locationDetail = this.OrderInfo.LocationDetail;
    var totalSteps = this.OrderInfo.TotalSteps;
    var scheduleSlots = this.OrderInfo.lstScheduleSlots;
    this.OrderInfo = new OrderInfo();
    this.OrderInfo.selectedOrderType = selectedOrderType;
    this.OrderInfo.LocationDetail = locationDetail;
    this.OrderInfo.CurrentStep = 2;
    this.OrderInfo.TotalSteps = totalSteps;
    this.OrderInfo.lstScheduleSlots = scheduleSlots;
  }

  GetCustomerPaymentProfiles(): Promise<any> {
    let promise = new Promise((resolve, reject) => {
      this.httpService
        .get(environment.baseApiUrl + environment.GetCustomerPaymentProfiles)
        .toPromise()
        .then(res => {
          // Success

          resolve(res);
        });
    });
    return promise;
  }

  GetAuthorizeNetToken(authorizeNetInfo: AuthorizeNetInfo): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .post(
          environment.baseApiUrl + environment.GetAuthNetToken,
          authorizeNetInfo,
          this.contentType.jsonType
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(null);
            observer.complete();
          }
        );
    });
  }

  RemoveCard(

    paymentProfileId: number,
    organizationUserID: number
  ): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl +
          environment.RemoveCustomerCard +
          "/" +
          organizationUserID +
          "/" +
          paymentProfileId
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }
  SubmitPayment(
    orderNumber: string,
    paymentProfileId: number,
    isAlreadyPlacedOrder: boolean
  ): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl +
          environment.SubmitPayment +
          "/" +
          orderNumber +
          "/" +
          paymentProfileId +
          "/" +
          isAlreadyPlacedOrder
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  ChangePaymentTypeSubmitOrder(
    orderNumber: string,
    paymentoptionId: number,
    langCode: string
  ): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl +
          environment.ChangePaymentTypeSubmitOrder +
          "/" + orderNumber +
          "/" + paymentoptionId +
          "/" + langCode
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  ReserveSlot(locationDetail: LocationDetail): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .post(
          environment.baseApiUrl + environment.ReserveSlot,
          locationDetail,
          this.contentType.jsonType
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  GetLocalizedDateFormat(dateSlot: string, langCode: string): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl +
          environment.GetLocalizedDateFormat +
          "/" +
          dateSlot +
          "/" +
          langCode
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  //Create Receipt

  CreateReceiptFile(orderInfo: OrderInfo): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .post(
          environment.baseApiUrl + environment.CreateHtmlFile,
          orderInfo,
          this.contentType.jsonType
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  DownloadOrderReceipt(orderNumber: string): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl +
          environment.DownloadOrderReceipt +
          "/" +
          orderNumber
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  IncrementStep() {
    this.OrderInfo.CurrentStep = this.OrderInfo.CurrentStep + 1;
  }

  DecrementStep() {
    this.OrderInfo.CurrentStep = this.OrderInfo.CurrentStep - 1;
  }
  GetOrderNumberDetails(orderNumber: string): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl +
          environment.GetOrderNumberDetails +
          "/" +
          orderNumber
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }
  getBillingCodeAmount(
    CBIUniId: string,
    CBIBillingCOde: string
  ): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl +
          environment.GetBillingCodeAmount +
          "/" +
          CBIUniId +
          "/" +
          CBIBillingCOde
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  GetLocalizedGenderName(
    defaultLanguageKey: number,
    langCode: string
  ): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl +
          environment.GetLocalizedGenderName +
          "/" +
          defaultLanguageKey +
          "/" +
          langCode
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  CancelOrder(orderDetails: OrderDetail) {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .post(
          environment.baseApiUrl + environment.CancelOrder,
          orderDetails,
          this.contentType.jsonType
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  ///// Order History Methods - Rescheduling, Cancel, Change Payment Type

  getOrderDetail() {
    return this.orderDetails;
  }
  setOrderDetail(OrderDetail: Array<OrderDetail>) {
    this.orderDetails = OrderDetail;
  }

  public getAlreadyPlacedOrder(): OrderInfo {
    if (
      this.aleadyPlacedOrder.userInfo.OrganizationUserID == undefined ||
      this.aleadyPlacedOrder.userInfo.OrganizationUserID <= 0
    ) {
      this.getOrgUsrInfoDetForAlredyPlacedOrder();
    }
    return this.aleadyPlacedOrder;

  }
  public setAlreadyPlacedOrder(orderInfo: OrderInfo) {
    this.aleadyPlacedOrder = orderInfo;
  }
  public setAlreadyPlacedOrderInfoToEmpty() {    
    this.aleadyPlacedOrder = new OrderInfo();
    this.isOrderAlreadyPlaced = false;
    this.isSendForOnline = false;
  }

  getSelectedOrderDetail() {
    return this.selectedOrderDetail;
  }

  setSelectedOrderDetail(orderDetail: OrderDetail) {
    this.selectedOrderDetail = orderDetail;
  }

  RescheduleOrder(
    rescheduleAppointmentInfo: RescheduleAppointmentInfo,
    langCode: string
  ): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .post(
          environment.baseApiUrl +
          environment.RescheduleAppointment +
          "/" +
          langCode,
          rescheduleAppointmentInfo,
          this.contentType.jsonType
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  initializelstScheduleSlot() {
    return (this.OrderInfo.lstScheduleSlots = new Array<ScheduleSlots>());
  }
  UpdateUserDetails(orderInfo: OrderInfo): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .post(
          environment.baseApiUrl + environment.SubmitUserDetails,
          orderInfo,
          this.contentType.jsonType
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }


  getLocationImages(LocationId: number): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl +
          environment.GetLocationImages +
          "/" +
          LocationId
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  GetCustomAppributesForOrderID(orderID: number, packageID: number): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl + environment.GetCustomAppributesForOrderID + "/" + orderID + "/" + packageID
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }

  GetCBIUniqueIdByAcctNameOrNumber (acctNameOrNumber: string): Observable<any> {
    return Observable.create((observer: Observer<any>) => {
      this.httpService
        .get(
          environment.baseApiUrl + environment.GetCBIUniqueIdByAcctNameOrNumber + "/" + acctNameOrNumber
        )
        .map(this.getRawResult)
        .subscribe(
          (rNArray: any) => {
            if (rNArray != null && rNArray != undefined) {
              observer.next(rNArray);
              observer.complete();
            }
          },
          (err: any) => {
            console.log("call failed");
            observer.next(-1);
            observer.complete();
          }
        );
    });
  }
}
