import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { of } from 'rxjs';
import 'rxjs/add/operator/switchMap';
import { Subscription } from 'rxjs/Subscription';
import { AppState, UpsertOrder } from '../../core';
import { OrderDto, OrdersService, OrderEditModel } from '../../domain';

@Component({
  selector: 'app-order-edit-page',
  templateUrl: './order-edit-page.component.html',
  styleUrls: ['./order-edit-page.component.scss']
})
export class OrderEditPageComponent implements OnInit, OnDestroy {
  private subscription: Subscription;
  model: OrderEditModel;
  orderId: string;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly ordersService: OrdersService,
    private readonly store: Store<AppState>
  ) { }

  ngOnInit() {
    this.subscription = this.route.paramMap
      .switchMap(params => {
        this.orderId = params.get('id');
        if (this.orderId) {
          return this.ordersService.order(this.orderId);
        }

        return of({});
      })
      .subscribe(data => this.model = data);
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
      this.subscription = undefined;
    }
  }

  onSubmit() {
    this.store.dispatch(new UpsertOrder({
      order: this.model,
      orderId: this.orderId
    }));
    this.router.navigate(['/orders']);
  }

  invalid(control: AbstractControl) {
    return control && control.invalid && (control.dirty || control.touched);
  }
}
