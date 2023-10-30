using Xunit;
using Moq;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Shop.API.Models;
using Shop.API.Dto;
using Shop.API.Services;
using Shop.API.Data;
using Shop.API.Helpers;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace UnitTest;
public class CartServiceTests
{
    [Fact]
    public async Task AddCartItem_ValidInput_ReturnsOkResult()
    {
        // Arrange
        var cartItem = new CartItem { Name = "test", Description = "testing", Price = 9, UserId = "email.com" };
        var userId = "testUserId";
        var cartRepository = new Mock<ICartRepository>();
        var s3Client = new Mock<IAmazonS3>();
        var cartService = new CartService(cartRepository.Object, s3Client.Object);

        // Act
        var result = await cartService.AddCartItem(cartItem, userId);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteCartItem_ValidInput_ReturnsOkResult()
    {
        // Arrange
        var cartItemId = 1; 
        var userId = "testUserId";
        var cartItem = new CartItem { Name = "test", Description="testing", Price=9, UserId= "email.com" };
        var cartRepository = new Mock<ICartRepository>();
        cartRepository.Setup(repo => repo.GetById(cartItemId)).ReturnsAsync(cartItem);
        var s3Client = new Mock<IAmazonS3>();
        var cartService = new CartService(cartRepository.Object, s3Client.Object);

        // Act
        var result = await cartService.DeleteCartItem(cartItemId, userId);

        // Assert
        Assert.IsType<OkResult>(result);
    }

}

