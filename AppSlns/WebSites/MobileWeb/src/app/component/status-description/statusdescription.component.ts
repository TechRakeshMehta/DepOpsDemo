import { Component, Input, OnChanges, SimpleChange, OnInit, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'status-description-app',
    templateUrl: './StatusDescription.html' 
})

export class StatusDescription implements OnInit {
    @Output() show = new EventEmitter<boolean>();
    constructor( private router: Router) { }
    ngOnInit() {
    }
    close(){
        this.show.emit(true);
    }
}