import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { environment } from '../../environments/environment';
import { Basket, BasketItem } from '../shared/models/basket';
import { BasketService } from './basket.service';

const api = environment.apiUrl;

function item(id: number, price: number, quantity = 1): BasketItem {
  return { id, productName: `Product ${id}`, price, quantity, pictureUrl: '', brand: 'Brand', type: 'Type' };
}

describe('BasketService', () => {
  let service: BasketService;
  let http: HttpTestingController;

  beforeEach(() => {
    localStorage.clear();
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });
    service = TestBed.inject(BasketService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  it('creates a basket with a UUID id, remembers it, and saves it with POST /basket', () => {
    service.addItemToBasket(item(1, 10), 2);

    const req = http.expectOne(api + 'basket');
    expect(req.request.method).toBe('POST');
    const sent = req.request.body as Basket;
    expect(sent.id).toMatch(/^[0-9a-f-]{36}$/);
    expect(localStorage.getItem('basket_id')).toBe(sent.id);
    expect(sent.items).toEqual([expect.objectContaining({ id: 1, quantity: 2 })]);
    req.flush(sent);
  });

  it('computes totals from the basket the API returns', () => {
    let totals: unknown = null;
    service.basketTotalSource$.subscribe((t) => (totals = t));

    service.addItemToBasket(item(1, 10), 2);
    const req = http.expectOne(api + 'basket');
    req.flush({ ...(req.request.body as Basket), shippingPrice: 5 });

    expect(totals).toEqual({ shipping: 5, subtotal: 20, total: 25 });
  });

  it('adds to the quantity when the same product is added again', () => {
    service.addItemToBasket(item(1, 10));
    const first = http.expectOne(api + 'basket');
    first.flush(first.request.body);

    service.addItemToBasket(item(1, 10));
    const second = http.expectOne(api + 'basket');
    expect((second.request.body as Basket).items[0].quantity).toBe(2);
    second.flush(second.request.body);
  });

  it('deletes the basket on the server when the last item is removed', () => {
    service.addItemToBasket(item(1, 10));
    const save = http.expectOne(api + 'basket');
    const basket = save.request.body as Basket;
    save.flush(basket);

    service.removeItemFromBasket(1);
    const del = http.expectOne(api + 'basket?id=' + basket.id);
    expect(del.request.method).toBe('DELETE');
    del.flush(null);

    expect(service.getCurrentBasketValue()).toBeNull();
    expect(localStorage.getItem('basket_id')).toBeNull();
  });

  it('creates a payment intent at POST /payment/{basketId}, the route the API serves', () => {
    service.addItemToBasket(item(1, 10));
    const save = http.expectOne(api + 'basket');
    const basket = save.request.body as Basket;
    save.flush(basket);

    service.createPaymentIntent().subscribe();
    const req = http.expectOne(api + 'payment/' + basket.id);
    expect(req.request.method).toBe('POST');
    req.flush({ ...basket, clientSecret: 'secret', paymentIntentId: 'pi_1' });

    expect(service.getCurrentBasketValue()?.paymentIntentId).toBe('pi_1');
  });
});
