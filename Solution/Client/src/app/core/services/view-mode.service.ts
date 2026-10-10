import { Injectable, signal } from '@angular/core';

export type ShopViewMode = 'cards' | 'list';

/** Cards or List on the shop page; remembered in localStorage (the WPF client keeps the same choice). */
@Injectable({
  providedIn: 'root'
})
export class ViewModeService {
  private static readonly key = 'shop_view';
  readonly mode = signal<ShopViewMode>(ViewModeService.load());

  set(mode: ShopViewMode) {
    this.mode.set(mode);
    try {
      localStorage.setItem(ViewModeService.key, mode);
    } catch {
      // Storage can be unavailable (private windows); the choice then lasts until reload.
    }
  }

  private static load(): ShopViewMode {
    try {
      return localStorage.getItem(ViewModeService.key) === 'list' ? 'list' : 'cards';
    } catch {
      return 'cards';
    }
  }
}
