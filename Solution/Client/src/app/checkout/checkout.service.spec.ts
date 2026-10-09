import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { environment } from '../../environments/environment';
import { CheckoutService } from './checkout.service';

const api = environment.apiUrl;

describe('CheckoutService', () => {
  let service: CheckoutService;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });
    service = TestBed.inject(CheckoutService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  it('creates an order with POST /order, the route the API serves', () => {
    const order = { basketId: 'b1', deliveryMethodId: 1, shipToAddress: {} } as never;
    service.createOrder(order).subscribe();

    const req = http.expectOne(api + 'order');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(order);
    req.flush({});
  });

  it('loads delivery methods from GET /order/deliveryMethods, most expensive first', () => {
    let names: string[] = [];
    service.getDeliveryMethods().subscribe((dm) => (names = dm.map((d) => d.shortName)));

    http.expectOne(api + 'order/deliveryMethods').flush([
      { id: 1, shortName: 'Free', price: 0, deliveryTime: '', description: '' },
      { id: 2, shortName: 'Fast', price: 10, deliveryTime: '', description: '' },
      { id: 3, shortName: 'UPS', price: 5, deliveryTime: '', description: '' },
    ]);

    expect(names).toEqual(['Fast', 'UPS', 'Free']);
  });
});
