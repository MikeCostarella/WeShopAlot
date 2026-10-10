import { Injectable, signal } from '@angular/core';

/** Opens and closes the hamburger menu (MainMenuComponent). */
@Injectable({
  providedIn: 'root'
})
export class MenuService {
  readonly isOpen = signal(false);

  open() {
    this.isOpen.set(true);
  }

  close() {
    this.isOpen.set(false);
  }
}
