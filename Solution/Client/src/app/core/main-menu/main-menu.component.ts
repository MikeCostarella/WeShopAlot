import { ChangeDetectionStrategy, Component, ElementRef, OnInit, ViewChild, computed, effect } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { AccountService } from '../../account/account.service';
import { BasketService } from '../../basket/basket.service';
import { Type } from '../../shared/models/type';
import { ShopService } from '../../shop/shop.service';
import { buildTime } from '../build-info';
import { MenuService } from '../services/menu.service';
import { ViewModeService } from '../services/view-mode.service';

type Section = 'shop' | 'account' | 'view' | 'links' | 'about';

/** The hamburger menu: an accordion drawer, the same sections as the WPF client's menu. */
@Component({
  selector: 'app-main-menu',
  templateUrl: './main-menu.component.html',
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class MainMenuComponent implements OnInit {
  @ViewChild('closeButton') closeButton?: ElementRef<HTMLButtonElement>;
  types: Type[] = [];
  readonly apiUrl = new URL(environment.apiUrl, document.baseURI).href;
  readonly swaggerUrl = this.apiUrl.replace(/api\/?$/, 'swagger');
  readonly buildText = buildTime ? `Built ${buildTime.toLocaleString()}` : 'Development build';
  private openSections = new Set<Section>(['shop', 'account']);

  private basket = toSignal(this.basketService.basketSource$);
  basketCount = computed(() => (this.basket()?.items ?? []).reduce((sum, item) => sum + item.quantity, 0));

  constructor(public menu: MenuService, public viewMode: ViewModeService, public accountService: AccountService,
    private basketService: BasketService, private shopService: ShopService, private router: Router) {
    // Move keyboard focus into the menu when it opens.
    effect(() => {
      if (this.menu.isOpen()) setTimeout(() => this.closeButton?.nativeElement.focus());
    });
  }

  ngOnInit(): void {
    this.shopService.getTypes().subscribe({
      next: types => this.types = types,
      error: () => this.types = []
    });
  }

  isOpen(section: Section) {
    return this.openSections.has(section);
  }

  toggle(section: Section) {
    if (this.openSections.has(section)) this.openSections.delete(section);
    else this.openSections.add(section);
  }

  showType(typeId: number) {
    this.shopService.showType(typeId);
    this.menu.close();
    this.router.navigateByUrl('/shop');
  }

  logout() {
    this.menu.close();
    this.accountService.logout();
  }
}
