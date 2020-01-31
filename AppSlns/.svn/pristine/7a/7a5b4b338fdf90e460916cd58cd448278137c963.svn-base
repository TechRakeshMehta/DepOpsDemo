
/* System defined core library */
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

/* User defined library/components */
import { LoaderService } from '../../services/shared-services/loader.service';
import { LoaderState } from '../../models/common/loader.model';

@Component({
    selector: 'loader-app',
    templateUrl: './loader.component.html',
    styleUrls: ['./loader.component.scss'],
})

export class LoaderComponent implements OnInit {

    /* User defined private/public variables  */
    public enableSpinner: Boolean = false;

    /* Subscriptions */
    private subscription: Subscription;

    /* Constructor declaration with required injected services */
    constructor(
        private router: Router,
        private loaderService: LoaderService) {
    }

    /* Initialize the component & call require methods when component loaded first time. */
    ngOnInit() {
        this.subscription = this.loaderService.loaderState
            .subscribe((state: LoaderState) => {
                this.enableSpinner = state.show;
            });
    }

    /* Un subscribe the subscription when component is unloaded */
    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}

