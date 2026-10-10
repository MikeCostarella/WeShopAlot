import { Component, Input, ChangeDetectionStrategy } from '@angular/core';
import { BasketService } from '../../basket/basket.service';
import { Product } from '../../shared/models/product';

@Component({
    selector: 'app-product-item',
    templateUrl: './product-item.component.html',
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class ProductItemComponent {
  @Input() product?: Product;

  constructor(private basketService: BasketService) {}

  addItemToBasket() {
    this.product && this.basketService.addItemToBasket(this.product);
  }
}
