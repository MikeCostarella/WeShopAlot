import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { environment } from '../../environments/environment';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

const api = environment.apiUrl;
const page = { pageIndex: 1, pageSize: 6, count: 0, data: [] };

describe('ShopService', () => {
  let service: ShopService;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting()],
    });
    service = TestBed.inject(ShopService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  it('sends only the filters that are set, plus sort and paging', () => {
    service.getProducts().subscribe();

    const req = http.expectOne((r) => r.url === api + 'product');
    expect(req.request.params.keys().sort()).toEqual(['pageIndex', 'pageSize', 'sort']);
    expect(req.request.params.get('sort')).toBe('name');
    req.flush(page);
  });

  it('maps every ShopParams field to the query-string names the API binds', () => {
    const p = new ShopParams();
    p.brandId = 2;
    p.typeId = 3;
    p.sort = 'priceAsc';
    p.pageNumber = 4;
    p.pageSize = 12;
    p.search = 'boots';
    service.setShopParams(p);
    service.getProducts().subscribe();

    const req = http.expectOne((r) => r.url === api + 'product');
    const q = req.request.params;
    expect([q.get('brandId'), q.get('typeId'), q.get('sort'), q.get('pageIndex'), q.get('pageSize'), q.get('search')])
      .toEqual(['2', '3', 'priceAsc', '4', '12', 'boots']);
    req.flush(page);
  });

  it('serves a repeated query from its cache without a second request', () => {
    service.getProducts().subscribe();
    http.expectOne((r) => r.url === api + 'product').flush(page);

    service.getProducts().subscribe();
    http.expectNone((r) => r.url === api + 'product');
  });
});
