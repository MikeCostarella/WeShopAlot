import { Component, ChangeDetectionStrategy } from '@angular/core';
import { AccountService } from '../../account/account.service';
import { BasketService } from '../../basket/basket.service';
import { BasketItem } from '../../shared/models/basket';
import { MenuService } from '../services/menu.service';

@Component({
    selector: 'app-nav-bar',
    templateUrl: './nav-bar.component.html',
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class NavBarComponent {

  constructor(public basketService: BasketService, public accountService: AccountService, public menu: MenuService) {}

  getCount(items: BasketItem[]) {
    return items.reduce((sum, item) => sum + item.quantity, 0);
  }

}
