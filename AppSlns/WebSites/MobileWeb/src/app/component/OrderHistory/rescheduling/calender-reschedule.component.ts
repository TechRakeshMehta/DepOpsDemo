import { Component, OnInit } from '@angular/core';
import { OrderFlowService } from "../../../services/order-flow/order-flow.service";
import { FormBuilder, FormControl } from "@angular/forms";
import { Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { OrderInfo, ScheduleSlots } from '../../../models/order-flow/order.model';
import { CalendarComponent } from 'ng-fullcalendar';
import { DatePipe } from '@angular/common';
import { UtilityService } from "../../../services/shared-services/utility.service";
import { LanguageTranslationService } from "../../../services/language-translations/language-translations.service";
import { CommonService } from '../../../services/shared-services/common.service';

@Component({
    selector: 'calender-reschedule',
    templateUrl: './calender-reschedule.component.html',
    styleUrls: ['./calender-reschedule.component.css']
})
export class CalenderRescheduleComponent implements OnInit {
    orderInfo: OrderInfo
    calendarOptions: object;
    IsScheduleDateSelected: boolean = false;
    lstscheduleslots: Array<ScheduleSlots> = [];
    SelectedDate: string = "";
    IsPreviousButtonVisible: boolean = true;
    isSendForOnline: boolean = false;
    scheudlecalender = this.formBuilder.group({
        calender: ["", Validators.required]
    });

    constructor(
        private formBuilder: FormBuilder,
        private router: Router,
        private orderFlowService: OrderFlowService,
        private datepipe: DatePipe,
        private languageTranslationService: LanguageTranslationService,
        private utilityService: UtilityService,
        private commonService: CommonService
    ) {
    }
    setSchedulerLocalization(isSpanishLanguage: boolean) {
        if (isSpanishLanguage) {
            this.calendarOptions = {
                editable: false,
                eventLimit: false,
                locale: 'es',
                header: {
                    left: 'prev,next',
                    center: 'title',
                    right: ''
                },
                defaultView: 'month',
                defaultDate:this.SelectedDate,
            };
        }
        else {
            this.calendarOptions = {
                editable: false,
                eventLimit: false,
                header: {
                    left: 'prev,next',
                    center: 'title',
                    right: ''
                },
                defaultView: 'month',
                defaultDate:this.SelectedDate,
            };
        }
    }
    ngOnInit() {
        this.IsPreviousButtonVisible = this.orderFlowService.isLocationChange;
        this.isSendForOnline = this.orderFlowService.isSendForOnline;
        this.orderInfo = this.orderFlowService.getAlreadyPlacedOrder();
        this.lstscheduleslots = new Array<ScheduleSlots>();
        this.lstscheduleslots = this.orderInfo.lstScheduleSlots;
        this.SelectedDate = this.orderInfo.LocationDetail.SlotDate;
        if (this.SelectedDate == "" || this.SelectedDate == undefined) {
            var todayDate = this.datepipe.transform(new Date(), 'yyyy-MM-dd');
            var availableDate = this.lstscheduleslots.filter(x => x.SlotDate.toString() > (todayDate + 'T00:00:00') && x.IsAvailable == true)[0];
            if (availableDate != undefined) {
                this.SelectedDate = this.datepipe.transform(availableDate.SlotDate, 'yyyy-MM-dd').toString();
            }
        }
        this.commonService.isLanguageSpanish.subscribe(isSpanishLang => {
            this.setSchedulerLocalization(isSpanishLang);
        });

        var lstKeys = ["SELECTED", "AVAILABLE", "OF", "NOTAVAILABLE", "PREVIOUS", "NEXT", "RSCHDLAPPNMNT",
            "CNFMCNLRESCHEDULING", "CNCLRESCHEDULING", "LSTITMY", "LSTITMN", "CNCL"];
        this.utilityService.SubscribeLocalList(
            this.languageTranslationService,
            lstKeys
        );
    }

    dayRender(daydata: any) {
        var date = daydata.cell[0].dataset.date;
        $('[data-date=' + date + ']').removeClass('fc-today');
        var SlotAvailablity = this.lstscheduleslots.filter(x => x.SlotDate.toString() == (date + 'T00:00:00') && x.IsAvailable == true)[0];
        var SlotExists = this.lstscheduleslots.filter(x => x.SlotDate.toString() == (date + 'T00:00:00'))[0];
        if (SlotExists != undefined) {
            if (this.SelectedDate != '' && this.SelectedDate == date) {
                daydata.cell[0].bgColor = "#3f98e3";
                this.IsScheduleDateSelected = true;
            }
            else {
                if (SlotAvailablity != undefined) {
                    $('[data-date=' + date + ']').removeClass('fc-other-month');
                    daydata.cell[0].bgColor = "#abd686";
                }
                else {
                    daydata.cell[0].bgColor = "#cbcbb4";
                    $('[data-date=' + date + ']').addClass("disabled-day");
                }
            }
        }
        else {
            $('[data-date=' + date + ']').addClass("disabled-day");
        }
    }

    dayClick(daydata: any) {
        var dd = daydata.date._d.getUTCDate() < 10 ? "0" + daydata.date._d.getUTCDate() : daydata.date._d.getUTCDate();
        var mm = +(daydata.date._d.getUTCMonth() + 1) < 10 ? "0" + (daydata.date._d.getUTCMonth() + 1) : +(daydata.date._d.getUTCMonth() + 1);
        var tempDate = daydata.date._d.getUTCFullYear() + "-" + mm + "-" + dd;
        if (this.lstscheduleslots.filter(x => x.SlotDate.toString() == (tempDate + 'T00:00:00') && x.IsAvailable == true)[0] != undefined) {
            this.SelectedDate = tempDate;
            $('[bgcolor="#3f98e3"]').attr("bgcolor", "#abd686");
            $('[data-date=' + this.SelectedDate + ']').attr("bgcolor", "#3f98e3");
            this.IsScheduleDateSelected = true;
        }
        else if (this.SelectedDate == "") {
            this.IsScheduleDateSelected = false;
        }
    }
    onSubmit() {
        if (this.orderInfo.LocationDetail.SlotDate != this.SelectedDate) {
            this.orderInfo.LocationDetail.StartTime = null;
            this.orderInfo.LocationDetail.EndTime = null;
            this.orderInfo.LocationDetail.SlotID = 0;
            this.orderInfo.LocationDetail.SlotDate = this.SelectedDate;
        }
        else {
            this.orderInfo.LocationDetail.SlotDate = this.SelectedDate;
        }
        this.orderFlowService.setAlreadyPlacedOrder(this.orderInfo);
        this.router.navigate(["orderDetail/scheduleSlots"]);
    }
    goBack() {
        this.router.navigate(["orderDetail/locationReschedule"]);
    }
}