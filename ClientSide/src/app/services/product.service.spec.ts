import { TestBed } from '@angular/core/testing';
import { HttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { ProductService, Product } from './product.service';
import { environment } from '../../environments/environment';
import { provideHttpClient } from '@angular/common/http';

describe('ProductService', () => {
  let service: ProductService;
  let httpTestingController: HttpTestingController;
  let httpClient: HttpClient;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ProductService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(ProductService);
    httpClient = TestBed.inject(HttpClient);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getProducts', () => {
    it('should return an Observable<Product[]>', () => {
      const dummyProducts: Product[] = [
        { id: 1, name: 'Product 1', price: 100 },
        { id: 2, name: 'Product 2', price: 200 }
      ];

      service.getProducts().subscribe(products => {
        expect(products.length).toBe(2);
        expect(products).toEqual(dummyProducts);
      });

      const req = httpTestingController.expectOne(`${environment.apiUrl}/Products`);
      expect(req.request.method).toBe('GET');
      req.flush(dummyProducts);
    });
  });

  describe('getProduct', () => {
    it('should return an Observable<Product>', () => {
      const dummyProduct: Product = { id: 1, name: 'Product 1', price: 100 };

      service.getProduct(1).subscribe(product => {
        expect(product).toEqual(dummyProduct);
      });

      const req = httpTestingController.expectOne(`${environment.apiUrl}/Products/1`);
      expect(req.request.method).toBe('GET');
      req.flush(dummyProduct);
    });
  });

  describe('createProduct', () => {
    it('should return an Observable<Product>', () => {
      const newProduct: Product = { id: 1, name: 'New Product', price: 150 };

      service.createProduct(newProduct).subscribe(product => {
        expect(product).toEqual(newProduct);
      });

      const req = httpTestingController.expectOne(`${environment.apiUrl}/Products`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(newProduct);
      req.flush(newProduct);
    });
  });

  describe('updateProduct', () => {
    it('should return an Observable<Product>', () => {
      const updatedProduct: Product = { id: 1, name: 'Updated Product', price: 200 };

      service.updateProduct(1, updatedProduct).subscribe(product => {
        expect(product).toEqual(updatedProduct);
      });

      const req = httpTestingController.expectOne(`${environment.apiUrl}/Products/1`);
      expect(req.request.method).toBe('PUT');
      expect(req.request.body).toEqual(updatedProduct);
      req.flush(updatedProduct);
    });
  });

  describe('deleteProduct', () => {
    it('should send a DELETE request', () => {
      service.deleteProduct(1).subscribe();

      const req = httpTestingController.expectOne(`${environment.apiUrl}/Products/1`);
      expect(req.request.method).toBe('DELETE');
      req.flush(null);
    });
  });
});