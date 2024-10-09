using Api.Controllers;
using BL.Interfaces;
using DA.Entites;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Api.Tests;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _mockProductService = new Mock<IProductService>();
        _controller = new ProductsController(_mockProductService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithListOfProducts()
    {
        // Arrange
        var products = new List<Product> { new Product { Id = 1, Name = "Product 1" }, new Product { Id = 2, Name = "Product 2" } };
        _mockProductService.Setup(service => service.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
        Assert.Equal(2, returnValue.Count());
    }

    [Fact]
    public async Task Get_ReturnsOkResult_WithProduct_WhenProductExists()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Product 1" };
        _mockProductService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _controller.Get(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Product>(okResult.Value);
        Assert.Equal(1, returnValue.Id);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        _mockProductService.Setup(service => service.GetByIdAsync(1)).ReturnsAsync((Product)null);

        // Act
        var result = await _controller.Get(1);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction_WithCreatedProduct()
    {
        // Arrange
        var newProduct = new Product { Id = 1, Name = "New Product" };
        _mockProductService.Setup(service => service.CreateAsync(It.IsAny<Product>())).ReturnsAsync(newProduct);

        // Act
        var result = await _controller.Create(new Product { Name = "New Product" });

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<Product>(createdAtActionResult.Value);
        Assert.Equal(1, returnValue.Id);
        Assert.Equal("New Product", returnValue.Name);
    }

    [Fact]
    public async Task Update_ReturnsOk_WithUpdatedProduct_WhenValidId()
    {
        // Arrange
        var updatedProduct = new Product { Id = 1, Name = "Updated Product" };
        _mockProductService.Setup(service => service.UpdateAsync(It.IsAny<Product>())).ReturnsAsync(updatedProduct);

        // Act
        var result = await _controller.Update(1, updatedProduct);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<Product>(okResult.Value);
        Assert.Equal("Updated Product", returnValue.Name);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdDoesNotMatch()
    {
        // Arrange
        var productToUpdate = new Product { Id = 1, Name = "Product" };

        // Act
        var result = await _controller.Update(2, productToUpdate);

        // Assert
        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenProductDeleted()
    {
        // Arrange
        _mockProductService.Setup(service => service.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}

