import { Component, OnInit, } from "@angular/core";
import { Router } from "@angular/router";
import { OrderFlowService } from '../../services/order-flow/order-flow.service';

@Component({
    selector: 'dummy-order',
    templateUrl: 'dummy.component.html'
})

export class DummyComponent implements OnInit {
    constructor(private router: Router
        , private orderFlowService: OrderFlowService) { }
    ngOnInit(): void {
        if (this.orderFlowService.isOrderAlreadyPlaced) {
            this.router.navigate(["orderDetail/calenderReschedule"]);
        }
        else {
            this.router.navigate(["/createOrder/schedulecalender"]);
        }
    }
}