import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from './component/shared/layout/header.component'
import { HeaderRegisterComponent } from './component/shared/layout/header-register.component'
import { CommonService } from './services/shared-services/common.service';
import { Router, NavigationStart } from '@angular/router';


@Component({
  selector: 'my-app',
  templateUrl: './app.html',
  styleUrls: ['./app.scss']
})
export class AppComponent implements OnInit {
  
  IsLogin: boolean = false;
  ngOnInit(): void {
    this.router.events.subscribe(event => {      
      if (event instanceof NavigationStart) {        
        if (window.location.href.indexOf('Login.aspx?UsrVerCode=') >= 0) {          
          window.location.href = window.location.href.replace('Login.aspx','#/login');          
        }
      }
    });

 this.commonService.data.subscribe(value=>{
  this.IsLogin=value;
    });

  }

  constructor(private router: Router
    ,private commonService: CommonService) {

  }

  setWindowScrollToTop()
  {
      //QA Bug Fix: Added Code for Scrolling Issue on navigation to new page 
      //and previous page scroll is at the end.
      window.scrollTo(0, 0);
  }
}
