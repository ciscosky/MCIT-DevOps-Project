import { Component, OnInit } from '@angular/core';
import { ProductService, Product } from '../../services/product.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './product.component.html',
  styleUrl: './product.component.css'
})
export class ProductComponent implements OnInit {
  products: Product[] = [];
  product: Product = { id: 0, name: '', price: 0 };
  editMode = false;

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getProducts().subscribe((data) => {
      this.products = data;
    });
  }

  createProduct(): void {
    this.productService.createProduct(this.product).subscribe(() => {
      this.loadProducts();
      this.product = { id: 0, name: '', price: 0 };
    });
  }

  editProduct(product: Product): void {
    this.product = { ...product };
    this.editMode = true;
  }

  updateProduct(): void {
    this.productService.updateProduct(this.product.id, this.product).subscribe(() => {
      this.loadProducts();
      this.product = { id: 0, name: '', price: 0 };
      this.editMode = false;
    });
  }

  deleteProduct(id: number): void {
    this.productService.deleteProduct(id).subscribe(() => {
      this.loadProducts();
    });
  }
}
