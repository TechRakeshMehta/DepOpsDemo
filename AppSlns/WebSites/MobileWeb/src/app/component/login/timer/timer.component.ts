import { Component, OnInit } from '@angular/core';
import { Observable, Subscription } from 'rxjs/Rx';
import { CommonService } from '../../../services/shared-services/common.service';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/guard/auth-guard.service';
import { OrderFlowService } from '../../../services/order-flow/order-flow.service';
import { timerValue } from '../../../../environments/environment'
@Component({
    selector: 'timer',
    templateUrl: "./timer.component.html",
    
})
export class TimerComponent implements OnInit {



    ticks = 0;
    totalSeconds:number=timerValue;
    minutesDisplay: number = 0;
    hoursDisplay: number = 0;
    secondsDisplay: number = 0;
    startTime:Date;

    sub: Subscription;

    ngOnInit() {
        this.startTime = new Date();
        this.startTimer();
        this.commonService.timer.subscribe(value=>{
          
          if(value==true)
          this.startTime = new Date();
          this.totalSeconds=timerValue;
            });
        }
    constructor(private commonService:CommonService,private router:Router,private authService:AuthService,private orderFlowService:OrderFlowService)
    {
    }

    private startTimer() {
        
        let timer = Observable.timer(0, 1000);
        this.sub = timer.subscribe(
            t => {                
                var currentTime = new Date();                
                var diff = Math.floor((currentTime.valueOf() - this.startTime.valueOf())/1000);
                var currentTick = this.totalSeconds - diff;                
                if (currentTick<=0)
                {
                    sessionStorage.clear();
                    this.orderFlowService.setOrderInfoToEmpty();
                    this.sub.unsubscribe();
                    this.router.navigate([""]);
                    this.authService.logoutUser();
                }
                this.secondsDisplay = this.getSeconds(currentTick);
                this.minutesDisplay = this.getMinutes(currentTick);
                //this.totalSeconds--; 
            }
        );
    }

    private getSeconds(ticks: number) {
        return this.pad(ticks % 60);
    }

    private getMinutes(ticks: number) {
         return this.pad((Math.floor(ticks / 60)) );
    }

  

    private pad(digit: any) { 
        return digit <= 9 ? '0' + digit : digit;
    }
}