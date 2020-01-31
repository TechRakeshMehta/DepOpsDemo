import { RouterModule, Routes } from "@angular/router";
import { ModuleWithProviders } from "@angular/core/src/metadata/ng_module";
import { HashLocationStrategy, LocationStrategy } from "@angular/common";

import { AppComponent } from "./app.component";
import { LoginComponent } from "./component/login/login.component";
import { WelcomeComponent } from "./component/welcome/welcome.component";
import { FormFieldComponent } from "./component/form-field/form-field.component";
import { AuthGuardService } from "./services/guard/activation-guard";
import { ComplianceCategory } from "../app/component/category/category.component";
import { CompliancePackage } from "../app/component/package/package.component";
import { PictureUploadComponent } from "../app/component/picture-upload/picture-upload.component";
import { SuccessComponent } from "../app/component/success/success.component";
import { PersonalComponent } from "./component/register-user/personal.component";
import { AddressComponent } from "./component/register-user/address.component";
import { ContactComponent } from "./component/register-user/contact.component";
import { LinkAccountComponent } from "./component/register-user/link-account/link-account.component"
import { AccountLinkedComponent } from "./component/register-user/link-account/account-linked.component"
import { PasswordVerificationComponent } from "./component/register-user/link-account/password-verification.component"
import { AliasComponent } from "./component/register-user/alias.component";
import { LsAliasComponent } from "./component/register-user/ls-alias/ls-alias.component";
import { CreateOrderComponent } from './component/create-order/step1/create-order.component';
import { UserDashboardComponent } from "./component/user-dashboard/user-dashboard.component";
import { LocationComponent } from "./component/create-order/step1/location.component";
import { EventDetailComponent } from "./component/create-order/step1/event-detail.component";
import { OrderFlowPackagesComponent } from "./component/create-order/step2/order-flow-packages.component";
import { FingerPrintingComponent } from "./component/create-order/step4/fingerprinting.component";
import { ServiceDetailsComponent } from "./component/create-order/step4/service-details.component";
import { OutOfStateComponent } from "./component/create-order/step1/out-of-state.component";
import { OrderReviewComponent } from './component/create-order/step6/order-review.component';
import { OrderSummaryComponent } from "./component/create-order/step8/order-summary.component";
import { PurchaseDetailComponent } from "./component/create-order/step7/purchase-detail.component";
import { MaildetailsComponent } from "./component/create-order/step4/maildetails.component";
import { OrderHistoryComponent } from "./component/OrderHistory/order-history.component";
import { ScheduleCalenderComponent } from './component/create-order/step5/schedulecalender.component';
import { ScheduleslotComponent } from './component/create-order/step5/scheduleslot.component';
import { CardSelectionComponent } from "./component/create-order/step7/card-selection.component";
import { PersonalInfoComponent } from "./component/create-order/step3/personalInfo.component";
import { AddressInfoComponent } from "./component/create-order/step3/addressInfo.component";
import { ContactInfoComponent } from "./component/create-order/step3/contactInfo.component";
import { AliasInfoComponent } from "./component/create-order/step3/aliasInfo.component";
import { LsAliasInfoComponent } from "./component/create-order/step3/ls-aliasInfo/ls-aliasInfo.component";
import { EventDetailConfirmComponent } from "./component/create-order/step1/event-detail-confirm.component";
import { LanguageTranslationComponent } from './component/common/languageTranslation.component';
import { TimerComponent } from "./component/login/timer/timer.component";
import { DummyComponent } from "./component/Dummy/dummy.component";
import { OrderRescheduleComponent } from './component/OrderHistory/rescheduling/order-reschedule.component';
import { LocationRescheduleComponent } from './component/OrderHistory/rescheduling/location-reschedule.component';
import { EventRescheduleComponent } from './component/OrderHistory/rescheduling/event-reschedule.component';
import { CalenderRescheduleComponent } from './component/OrderHistory/rescheduling/calender-reschedule.component';
import { SlotRescheduleComponent } from './component/OrderHistory/rescheduling/slot-reschedule.component'
import { CantAccessAccountComponent } from "./component/login/cant-access-account.component";
import { ForgotPasswordComponent } from "./component/login/forgot-password.component";
import { AdditionAccountVerification } from "./component/login/additionalAccountVerification.component";
import { RecapComponent } from "./recap/recap.component";
// import { ContactComponent } from './component/register-user/contact.component';
export const AppRoutes: Routes = [
  { path: "login", component: LoginComponent },
  { path: "login/:id", component: LoginComponent },
  { path: "", component: LoginComponent },
  { path: 'welcome', component: WelcomeComponent, canActivate: [AuthGuardService] },
  { path: "category", component: ComplianceCategory },
  { path: "package", component: CompliancePackage },
  { path: "welcome/imageupload", component: PictureUploadComponent },
  { path: "welcome/imageupload/success", component: SuccessComponent },
  { path: "registerUser", component: PersonalComponent },
  { path: "registerUser/alias", component: LsAliasComponent },
  { path: "registerUser/address", component: AddressComponent },
  { path: "registerUser/contact", component: ContactComponent },
  { path: "registerUser/link-account", component: LinkAccountComponent },
  { path: "registerUser/account-linked", component: AccountLinkedComponent },
  { path: "registerUser/password-verification", component: PasswordVerificationComponent },
  { path: 'userDashboard', component: UserDashboardComponent, canActivate: [AuthGuardService] },
  { path: 'createOrder', component: CreateOrderComponent, canActivate: [AuthGuardService] },
  { path: 'createOrder/location', component: LocationComponent, canActivate: [AuthGuardService] },
  { path: 'createOrder/eventdetails', component: EventDetailComponent, canActivate: [AuthGuardService] },
  { path: 'createOrder/Orderflowpackages', component: OrderFlowPackagesComponent, canActivate: [AuthGuardService] },
  { path: 'createOrder/fingerPrinting', component: FingerPrintingComponent, canActivate: [AuthGuardService] },
  { path: 'createOrder/OutOfState', component: OutOfStateComponent, canActivate: [AuthGuardService] },
  { path: 'createOrder/OrderReview', component: OrderReviewComponent, canActivate: [AuthGuardService] },
  { path: 'createOrder/serviceDetails', component: ServiceDetailsComponent, canActivate: [AuthGuardService] },
  { path: 'createOrder/OrderSummary', component: OrderSummaryComponent },
  { path: 'createOrder/PaymentDetail', component: PurchaseDetailComponent, canActivate: [AuthGuardService] },
  { path: 'createOrder/MailingInfo', component: MaildetailsComponent, canActivate: [AuthGuardService] },
  { path: 'OrderHistory', component: OrderHistoryComponent, canActivate: [AuthGuardService] },
  { path: 'createOrder/schedulecalender', component: ScheduleCalenderComponent, canActivate: [AuthGuardService] },
  { path: 'createOrder/scheduleslot', component: ScheduleslotComponent, canActivate: [AuthGuardService] },
  { path: 'createOrder/CardSelection', component: CardSelectionComponent },
  { path: 'createOrder/scheduleslot', component: ScheduleslotComponent, canActivate: [AuthGuardService] },
  { path: 'createOrder/personalInfo', component: PersonalInfoComponent, canActivate: [AuthGuardService] },
  { path: 'createOrder/addressInfo', component: AddressInfoComponent, canActivate: [AuthGuardService] },
  { path: 'createOrder/contactInfo', component: ContactInfoComponent, canActivate: [AuthGuardService] },
  { path: "createOrder/aliasInfo", component: LsAliasInfoComponent, canActivate: [AuthGuardService] },
  { path: "createOrder/EventDetailConfirm", component: EventDetailConfirmComponent, canActivate: [AuthGuardService] },
  { path: 'LanguageTranslation', component: LanguageTranslationComponent },
  { path: 'Timer', component: TimerComponent },
  { path: 'Dummy', component: DummyComponent },
  { path: 'orderDetail/orderReschedule', component: OrderRescheduleComponent, canActivate: [AuthGuardService] },
  { path: 'orderDetail/locationReschedule', component: LocationRescheduleComponent, canActivate: [AuthGuardService] },
  { path: 'orderDetail/eventReschedule', component: EventRescheduleComponent, canActivate: [AuthGuardService] },
  { path: 'orderDetail/calenderReschedule', component: CalenderRescheduleComponent, canActivate: [AuthGuardService] },
  { path: 'orderDetail/scheduleSlots', component: SlotRescheduleComponent, canActivate: [AuthGuardService] },
  { path: 'forgotPassword', component: CantAccessAccountComponent },
  { path: 'changePassword', component: ForgotPasswordComponent },
  {path:'AdditionAccountVerification',component:AdditionAccountVerification},
  { path: 'recap', component: RecapComponent }
];



export const ROUTING: ModuleWithProviders = RouterModule.forRoot(AppRoutes, {
  useHash: true
});