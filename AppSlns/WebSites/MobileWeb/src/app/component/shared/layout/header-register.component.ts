import { Component, OnInit } from '@angular/core';
import { AdbApiService } from '../../../services/shared-services/adb-api-data.service';

@Component({
  selector: 'app-header-register',
  templateUrl: './header-register.component.html',
  styleUrls: ['./header-register.component.css']
})
export class HeaderRegisterComponent implements OnInit {

  private IsLocationSpecificTenant:boolean;
  constructor(private adbApiService: AdbApiService) { 
    this.adbApiService.getCbiUrlInfo().subscribe(userData => {   
      this.IsLocationSpecificTenant = userData.Message;
  });
  }

  ngOnInit() {
  }

}
