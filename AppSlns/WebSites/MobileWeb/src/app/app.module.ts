import { NgModule, enableProdMode } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ROUTING } from './app.routing'
import { ReactiveFormsModule } from '@angular/forms';  // <-- #1 import module
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { TextMaskModule } from 'angular2-text-mask';
import { FullCalendarModule } from 'ng-fullcalendar';
import { CommonModule } from '@angular/common';
import { DatePipe, Location } from '@angular/common';
import { MyDatePickerModule } from 'mydatepicker';
/*custome component import*/
import { AppComponent } from './app.component';
import { LoginComponent } from './component/login/login.component';
import { LogoutComponent } from './component/logout/logout.component';
import { WelcomeComponent } from './component/welcome/welcome.component';
import { FormFieldComponent } from './component/form-field/form-field.component';
import { AuthGuardService } from './services/guard/activation-guard';
import { AuthService } from './services/guard/auth-guard.service';
import { DataSharingService } from './services/shared-services/data-sharing.service'
import { HttpService } from './services/shared-services/http.service'
import { LoaderService } from './services/shared-services/loader.service'
import { AdbApiService } from './services/shared-services/adb-api-data.service'
import { ContentType } from '../app/models/common/content-type.model'
import { PackageDetail } from '../app/models/applicant/package-contract.model'
import { CategoryDetail } from '../app/models/applicant/package-contract.model'
import { ItemDetail } from '../app/models/applicant/package-contract.model'
import { AttributeDetail } from '../app/models/applicant/package-contract.model'
import { AppConstant } from '../app/models/common/AppConst'
import { LoaderComponent } from './component/loader/loader.component';
import { CompliancePackage } from '../app/component/package/package.component';
import { ComplianceCategory } from '../app/component/category/category.component';
import { ComplianceItem } from '../app/component/item/item.component';
import { ComplianceAttribute } from '../app/component/attribute/attribute.component'
import { StatusDescription } from '../app/component/status-description/statusdescription.component'
import { SuccessComponent } from '../app/component/success/success.component';
import { PictureUploadComponent } from '../app/component/picture-upload/picture-upload.component'
import { FileUploadComponent } from '../app/component/file-upload/file-upload.component'
import { UtilityService } from './services/shared-services/utility.service'
import { LookupService } from './services/shared-services/lookup.service'
import { PersonalInfo, AddressInfo, ContactInfo, UserInfo } from '../app/models/user/user.model'
import { RegisterUserFormService } from './services/register-user/register-user-form.service'
import { FieldErrorDisplayComponent } from "./component/shared/field-error-display/field-error-display.component"
import { AddressComponent } from './component/register-user/address.component';
import { PersonalComponent } from './component/register-user/personal.component';
import { AliasComponent } from './component/register-user/alias.component';
import { ContactComponent } from './component/register-user/contact.component';
import { lookup } from 'dns';
import { LsAliasComponent } from './component/register-user/ls-alias/ls-alias.component';
import { AgmCoreModule } from '@agm/core';
import { ServiceDetailsComponent } from './component/create-order/step4/service-details.component';
import { UserDashboardComponent } from './component/user-dashboard/user-dashboard.component';
import { CreateOrderComponent } from './component/create-order/step1/create-order.component';
import { LocationComponent } from './component/create-order/step1/location.component';
import { GeocodeService } from './services/shared-services/google-map.service'
import { OrderFlowService } from './services/order-flow/order-flow.service';
import { MaildetailsComponent } from './component/create-order/step4/maildetails.component';
import { EventDetailComponent } from "./component/create-order/step1/event-detail.component";
import { OrderFlowPackagesComponent } from "./component/create-order/step2/order-flow-packages.component";
import { FingerPrintingComponent } from "./component/create-order/step4/fingerprinting.component";
import { OutOfStateComponent } from "./component/create-order/step1/out-of-state.component"
import { OrderReviewComponent } from './component/create-order/step6/order-review.component';
import { OrderSummaryComponent } from './component/create-order/step8/order-summary.component';
import { PurchaseDetailComponent } from './component/create-order/step7/purchase-detail.component';
import { OrderHistoryComponent } from './component/OrderHistory/order-history.component';
import { ScheduleCalenderComponent } from './component/create-order/step5/schedulecalender.component';
import { ScheduleslotComponent } from './component/create-order/step5/scheduleslot.component';
import { CardSelectionComponent } from './component/create-order/step7/card-selection.component';
import { PersonalInfoComponent } from "./component/create-order/step3/personalInfo.component";
import { AliasInfoComponent } from "./component/create-order/step3/aliasInfo.component";
import { ContactInfoComponent } from "./component/create-order/step3/contactInfo.component";
import { LsAliasInfoComponent } from "./component/create-order/step3/ls-aliasInfo/ls-aliasInfo.component";
import { AddressInfoComponent } from "./component/create-order/step3/addressInfo.component";
import { HeaderComponent } from './component/shared/layout/header.component';
import { HeaderRegisterComponent } from './component/shared/layout/header-register.component'
import { CommonService } from './services/shared-services/common.service';
import { StorageService } from "./services/shared-services/storage.service";
import { EventDetailConfirmComponent } from "./component/create-order/step1/event-detail-confirm.component";
import { ModalModule } from 'ngx-bootstrap/modal';
import { ModalContentComponent } from "./component/confirmation-box/confirmation-modal.component";
import { CancelOrderButtonComponent } from "./component/create-order/cancelOrder/cancel-order.component";
import { LanguageTranslationComponent } from './component/common/languageTranslation.component';
import { LanguageTranslationService } from './services/language-translations/language-translations.service'
import { CancelRegistrationButtonComponent } from "./component/register-user/cancelRegistration/cancel-registration.component";
import { TimerComponent } from './component/login/timer/timer.component';
import { DummyComponent } from './component/Dummy/dummy.component';
import { OrderRescheduleComponent } from './component/OrderHistory/rescheduling/order-reschedule.component';
import { LocationRescheduleComponent } from './component/OrderHistory/rescheduling/location-reschedule.component';
import { EventRescheduleComponent } from './component/OrderHistory/rescheduling/event-reschedule.component';
import { CalenderRescheduleComponent } from './component/OrderHistory/rescheduling/calender-reschedule.component';
import { SlotRescheduleComponent } from './component/OrderHistory/rescheduling/slot-reschedule.component';
import { CancelReschedulingButtonComponent } from './component/OrderHistory/cancelRescheduling/cancel-rescheduling.component';
import { CantAccessAccountComponent } from './component/login/cant-access-account.component';
import { ForgotPasswordComponent } from './component/login/forgot-password.component';
import { LinkAccountComponent } from './component/register-user/link-account/link-account.component'
import { AccountLinkedComponent } from "./component/register-user/link-account/account-linked.component"
import { PasswordVerificationComponent } from "./component/register-user/link-account/password-verification.component";
import { AdditionAccountVerification } from "./component/login/additionalAccountVerification.component";
import { RecapComponent } from './recap/recap.component';
import { RecaptchaModule, RECAPTCHA_SETTINGS, RecaptchaSettings } from 'ng-recaptcha';
import { RecaptchaFormsModule } from 'ng-recaptcha/forms'; 
import { CarouselModule } from 'ngx-bootstrap';
import { DynamicFieldDirective } from "./component/custom-component/dynamic-field/dynamic-field.directive";
import { DynamicFormComponent } from "./component/custom-component/dynamic-form/dynamic-form.directive";
import { InputComponent } from "./component/custom-component/input-text/input.component";
import { InputNumericComponent } from "./component/custom-component/input-numeric/input.numeric.component";
import { BooleanComponent } from "./component/custom-component/boolean/booleans.component";
import { SelectComponent } from "./component/custom-component/select.component";
import { DateComponent } from "./component/custom-component/date/date.component";


enableProdMode();
@NgModule({
    imports: [BrowserModule,
        ROUTING,
        ReactiveFormsModule,
        FormsModule,
        TextMaskModule,
        FullCalendarModule,
        CommonModule,
        HttpClientModule,
        MyDatePickerModule,
        ModalModule.forRoot(),
        RecaptchaModule,
        AgmCoreModule.forRoot({
            apiKey: 'AIzaSyCOASYYxjAtpbwP9vp55n8qcqjRTekMD_A'
        }),
        RecaptchaFormsModule,
        CarouselModule
        

    ],

    declarations:
        [
            AppComponent,
            LoginComponent,
            FormFieldComponent,
            WelcomeComponent,
            LogoutComponent,
            LoaderComponent,
            CompliancePackage,
            ComplianceCategory,
            ComplianceItem,
            ComplianceAttribute,
            StatusDescription,
            PictureUploadComponent,
            FileUploadComponent,
            SuccessComponent,
            FieldErrorDisplayComponent,
            AddressComponent,
            PersonalComponent,
            ServiceDetailsComponent,
            AliasComponent,
            ContactComponent,
            LsAliasComponent,
            UserDashboardComponent,
            CreateOrderComponent,
            MaildetailsComponent,
            LocationComponent,
            EventDetailComponent,
            OrderFlowPackagesComponent,
            FingerPrintingComponent,
            OutOfStateComponent,
            OrderReviewComponent,
            OrderSummaryComponent,
            PurchaseDetailComponent,
            OrderHistoryComponent,
            ScheduleCalenderComponent,
            ScheduleslotComponent,
            CardSelectionComponent,
            PersonalInfoComponent,
            AliasInfoComponent,
            ContactInfoComponent,
            LsAliasInfoComponent,
            AddressInfoComponent,
            HeaderComponent,
            EventDetailConfirmComponent,
            HeaderRegisterComponent,
            LanguageTranslationComponent,
            ModalContentComponent,
            CancelOrderButtonComponent,
            CancelRegistrationButtonComponent,
            TimerComponent,
            DummyComponent,
            OrderRescheduleComponent,
            LocationRescheduleComponent,
            EventRescheduleComponent,
            CalenderRescheduleComponent,
            SlotRescheduleComponent,
            CancelReschedulingButtonComponent,
            CantAccessAccountComponent,
            ForgotPasswordComponent, 
            LinkAccountComponent,
            AccountLinkedComponent,
            PasswordVerificationComponent,
            AdditionAccountVerification,
            InputComponent,
            SelectComponent,
            DynamicFieldDirective,
            DynamicFormComponent,
            RecapComponent,
            InputNumericComponent,
            BooleanComponent,
            DateComponent
        ],
    entryComponents: [ModalContentComponent,
        InputComponent,
        InputNumericComponent,
        BooleanComponent,
        DateComponent,
        SelectComponent
    ],
    bootstrap: [AppComponent],
    providers: [
        AuthService,
        AuthGuardService,
        DataSharingService,
        HttpService,
        ContentType,
        AppConstant,
        AdbApiService,
        LoaderService,
        PackageDetail,
        CategoryDetail,
        ItemDetail,
        AttributeDetail,
        UtilityService,
        AddressInfo,
        ContactInfo,
        PersonalInfo,
        UserInfo,
        LookupService,
        RegisterUserFormService,
        GeocodeService,
        OrderFlowService,
        DatePipe,
        Location,
        CommonService,
        StorageService,
        LanguageTranslationService,
        { provide: HTTP_INTERCEPTORS, useClass: HttpService, multi: true },
        {
            provide: RECAPTCHA_SETTINGS,
            useValue: { siteKey: '6LemfhsUAAAAAMa65oLhL9-w_Eo6wZVk8Th76LaG' } as RecaptchaSettings,
        }


    ]
})
export class AppModule { }
