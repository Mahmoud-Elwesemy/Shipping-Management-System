using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ITI.Shipping.APIs.Controllers;
using ITI.Shipping.Core.Application.Abstraction;
using ITI.Shipping.Core.Application.Abstraction.Product.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITI.Shipping.Core.Domin.Pramter_Helper;

public class ProductControllerTests
{
    private readonly Mock<IServiceManager> _serviceManagerMock;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _serviceManagerMock = new Mock<IServiceManager>();
        _controller = new ProductController(_serviceManagerMock.Object);
    }

    [Fact]
    public async Task GetProducts_ReturnsOkResult_WithListOfProducts()
    {
        // Arrange
        var mockProducts = new List<ProductDTO>
       {
           new ProductDTO ( Name : "Product1", Weight : 1.0m, Quantity : 10 ),
           new ProductDTO (Name : "Product2", Weight : 2.0m, Quantity : 20)
       };
        _serviceManagerMock
            .Setup(s => s.productService.GetProductsAsync(It.IsAny<Pramter>()))
            .ReturnsAsync(mockProducts);

        // Act
        var result = await _controller.GetProducts(new Pramter());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedProducts = Assert.IsType<List<ProductDTO>>(okResult.Value);
        Assert.Equal(2,returnedProducts.Count);
    }
}
