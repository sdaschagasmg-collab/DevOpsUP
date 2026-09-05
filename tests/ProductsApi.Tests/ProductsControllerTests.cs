using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProductsApi.Controllers;
using ProductsApi.Models;
using ProductsApi.Services;

namespace ProductsApi.Tests;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _serviceMock;
    private readonly Mock<ILogger<ProductsController>> _loggerMock;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _serviceMock = new Mock<IProductService>();
        _loggerMock = new Mock<ILogger<ProductsController>>();
        _controller = new ProductsController(_serviceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void GetAll_ReturnsOkWithProductList()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Teclado", Description = "Teclado RGB", Price = 45000m, Stock = 15 },
            new() { Id = 2, Name = "Mouse", Description = "Mouse inalámbrico", Price = 18000m, Stock = 30 }
        };
        _serviceMock.Setup(s => s.GetAll()).Returns(products);

        // Act
        var result = _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
        Assert.Equal(2, returnedProducts.Count());
    }

    [Fact]
    public void GetById_ExistingId_ReturnsOkWithProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Teclado", Description = "Teclado RGB", Price = 45000m, Stock = 15 };
        _serviceMock.Setup(s => s.GetById(1)).Returns(product);

        // Act
        var result = _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProduct = Assert.IsType<Product>(okResult.Value);
        Assert.Equal(1, returnedProduct.Id);
        Assert.Equal("Teclado", returnedProduct.Name);
    }

    [Fact]
    public void GetById_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.GetById(It.IsAny<int>())).Returns((Product?)null);

        // Act
        var result = _controller.GetById(999);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public void Create_ValidProduct_ReturnsCreatedAtAction()
    {
        // Arrange
        var newProduct = new Product { Name = "Monitor", Description = "Monitor 27\"", Price = 210000m, Stock = 8 };
        var createdProduct = new Product { Id = 3, Name = "Monitor", Description = "Monitor 27\"", Price = 210000m, Stock = 8 };
        _serviceMock.Setup(s => s.Create(It.IsAny<Product>())).Returns(createdProduct);

        // Act
        var result = _controller.Create(newProduct);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedProduct = Assert.IsType<Product>(createdResult.Value);
        Assert.Equal(3, returnedProduct.Id);
        _serviceMock.Verify(s => s.Create(It.IsAny<Product>()), Times.Once);
    }

    [Fact]
    public void Create_EmptyName_ReturnsBadRequest()
    {
        // Arrange
        var invalidProduct = new Product { Name = "", Description = "Sin nombre", Price = 100m, Stock = 1 };

        // Act
        var result = _controller.Create(invalidProduct);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
        _serviceMock.Verify(s => s.Create(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public void Update_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var product = new Product { Name = "Teclado actualizado", Description = "Nueva desc", Price = 50000m, Stock = 10 };
        _serviceMock.Setup(s => s.Update(1, It.IsAny<Product>())).Returns(true);

        // Act
        var result = _controller.Update(1, product);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Update_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var product = new Product { Name = "Producto", Description = "Desc", Price = 100m, Stock = 1 };
        _serviceMock.Setup(s => s.Update(999, It.IsAny<Product>())).Returns(false);

        // Act
        var result = _controller.Update(999, product);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public void Delete_ExistingId_ReturnsNoContent()
    {
        // Arrange
        _serviceMock.Setup(s => s.Delete(1)).Returns(true);

        // Act
        var result = _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Delete_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.Delete(999)).Returns(false);

        // Act
        var result = _controller.Delete(999);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}