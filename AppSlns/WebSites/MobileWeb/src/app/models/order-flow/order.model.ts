import { AttributesForCustomForm, PaymentTypeDetails } from "../custom-forms/custom-attribute";
import { mailingInfoAtt } from "../custom-forms/custom-attribute";
import { UserInfo } from "../user/user.model";

export class LocationDetail {
    LocationID: number;
    SlotID: number = 0;
    StartTime: Date;
    EndTime: Date;
    ReserverSlotID: number;
    LocationName: string = "";
    SlotDate: string = "";
    LocationAddress: string = "";
    IsLocationServiceTenant: boolean;
    LocationDescription: string;
    IsEventCode: boolean = false;
    EventID: number;
    EventName: string = "";
    EventDescription: string = "";
    EventDate: string = "";
    EventCode: string = "";
    CBIUniqueID: string = "";
    IsSSNRequired: boolean;
    IsOutOfState: boolean = false;
    IsLocationUpdate : boolean = false;
}

export class ScheduleSlots{
    LocationID: number;
    SlotID: number = 0;
    SlotDate: Date;
    StartDateTime: Date;
    EndDateTime: Date; 
    IsAvailable: boolean;
    SlotAppointment:number;
    ReserverSlotID: number;
}

export class EventSlots{
    LocationID: number;
    SlotID: number = 0;
    StartTime: Date;
    EndTime: Date;
    ReserverSlotID: number;
    LocationName: string = "";
    SlotDate: string = "";
    LocationAddress: string = "";
    IsLocationServiceTenant: boolean;
    LocationDescription: string;
    IsEventCode: boolean = false;
    EventID: number;
    EventName: string = "";
    EventDescription: string = "";
    EventDate: string = "";
    EventCode: string = "";
    CBIUniqueID: string = "";
    IsSSNRequired: boolean;
    IsOutOfState: boolean = false;
}

export class ApplicantOrderPaymentOptions {
    pkgid: number;
    poid: number;
    isbkg: boolean;
    isZP: boolean;
    additionalPoid: string;
}

export class OrderInfo {
    TotalSteps: number;
    CurrentStep: number;
    OrderID: number;
    InvoiceNumber: string = "";
    lstCustomAttribute: Array<AttributesForCustomForm> = [];
    lstCustomAttributeInSpanish: Array<AttributesForCustomForm> = [];
    lstMailingAttribute: Array<mailingInfoAtt> = [];
    lstMailingAttributeSpanish: Array<mailingInfoAtt> = [];
    selectedPaymentModeData: Array<ApplicantOrderPaymentOptions> = [];
    LocationDetail: LocationDetail;
    lstScheduleSlots: Array<ScheduleSlots> = [];
    lstEventSlots: Array<EventSlots> = [];
    lstPaymentDataDetail: Array<PaymentTypeDetails> =[];
    userInfo: UserInfo;
    customFormID: number;
    customFormGroupId: number;
    customFormInstanceId: number;
    CbiUniqueId: string = "";
    bkgPackageId: number;
    packageName: string = "";
    SelectedHierarchyNodeID: number;
    bkgPkgHierarchyMappingID: number;
    GrandTotal: string = "";
    TotalPrice: string = "";
    OrderNumber: string = "";
    selectedOrderType: string = "";
    DefaultZipCode: string = "";
    isEmpoyerDetailsMendatory: boolean = false;
    CbiBillingCode: string = "";
    PaymentType: string = "";
    PaymentInstructions: string = "";
    IsSSNRequired: string = "";
    LanguageCode:string="";
    IsLegalNameChange: string="";
    BillingCodeAmount: string="";
    IsPaymentByInstAlone: boolean=false;
    IsPaymentByInst: boolean=false;
    lstCBIUniqueIds: Array<any>;
    AcctNameOrNum: string;    
    constructor() {
        this.userInfo = new UserInfo();
        this.LocationDetail = new LocationDetail();
        this.lstScheduleSlots = new Array<ScheduleSlots>();
        this.lstCustomAttribute = new Array<AttributesForCustomForm>();
        this.selectedPaymentModeData = new Array<ApplicantOrderPaymentOptions>();
        this.lstMailingAttribute = new Array<mailingInfoAtt>();
        this.lstMailingAttributeSpanish=new Array<mailingInfoAtt>();
        this.lstPaymentDataDetail = new Array<PaymentTypeDetails>();
    }
}

export class OrderDetail {
    OrderId: number;
    LocationID:number;
    DeptProgramPackageID: number;
    OrderPackageType: string;
    InstituteHierarchy: string;
    PackageName: string;
    PackageID: number;
    BackageOrderPackageID:number;
    PaymentTypeCode: string;
    PaymentType: string;
    OrderStatusCode: string;
    OrderStatusName: string;
    OrderDate: string;
    Amount: string;
    DeptProgramMappingID: number;
    OrderNumber: string;
    BkgOrderStatusID: number;
    BkgOrderStatusCode: string;
    BkgOrderStatus: string;
    AppointmentStatusID: number;
    AppointmentStatusCode: string;
    AppointmentStatus: string;
    IsFileSentToCBI: boolean;
    IsRescheduleAvailable: boolean;
    IsOnsiteAppointment: boolean;
    IsColoradoFingerPrinting: boolean;
    IsOutOfState: boolean;
    BkgPackageHierarchyMappingId:string;
    IsBillingCode:boolean;
    IsChangePaymentTypeVisible:boolean;
    IsSendForOnline:boolean;
    StartDateTime:Date;
    EndDateTime:Date;
    LocationName:String;
    BillingCode: string = "";
}

export class RescheduleAppointmentInfo {
    locationDetail: LocationDetail;
    orderDetail: OrderDetail;   
    constructor() {
        this.locationDetail = new LocationDetail();
        this.orderDetail = new OrderDetail();
    }
}

export class AuthorizeNetInfo {
    CurrentUrl: string;
    ReturnUrl: string;
    Token: string;
    ActionUrl: string;
}


