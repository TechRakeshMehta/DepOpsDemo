/* System defined core library */
import { Injectable, Pipe, PipeTransform, Optional } from '@angular/core';
import { DatePipe } from '@angular/common';
import { Observable, Observer } from 'rxjs';
import 'rxjs/add/operator/mergeMap';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
/*User defined service*/
import { ContentType } from '../../models/common/content-type.model'
@Injectable()
export class DataSharingService {
    constructor(public contentType: ContentType) {

    }

    // public loginAuthTokenSource = new BehaviorSubject<any>(null);
    // public loginAuthTokenObj = this.loginAuthTokenSource.asObservable();
    // setLoginAuthTokenObj(objArray: any) {
    //     this.loginAuthTokenSource.next(objArray);
    // }
    public loginDataObj: any;
    public fileUploadData: any;
    public fileCollection: any;
    public successMsg: any;
}
