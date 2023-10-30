using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Mvc;
using Shop.API.Data;
using Shop.API.Dto;
using Shop.API.Helpers;
using Shop.API.Interfaces;
using Shop.API.Models;

namespace Shop.API.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;

    //private readonly DataContext _context;
    private readonly IAmazonS3 _s3Client;

    public CartService(ICartRepository cartRepository, IAmazonS3 s3Client)
    {
        _cartRepository = cartRepository;
        _s3Client = s3Client;
    }

    public async Task<IActionResult> AddCartItem([FromBody] CartItem cartItem, string userId)
    {
        cartItem.UserId = userId;

        await _cartRepository.AddAsync(cartItem);

        return new OkResult();
    }

    public async Task<IActionResult> AddCartItemImage(int cartItemId, [FromForm] ImageDto image, string userId)
    {
        var cartItem = await _cartRepository.GetById(cartItemId);

        if (cartItem == null) return new BadRequestResult();
        if (string.Equals(cartItem.UserId.ToLower().Trim(), userId)) return new UnauthorizedResult();

        var file = image.File;

        if (file.Length == 0)
            return new BadRequestResult();

        using (var fileStream = file.OpenReadStream())
        {
            var key = Guid.NewGuid().ToString(); // Unique key for the S3 object
            const string bucketName = "cart-item-images";

            var fileTransferUtility = new TransferUtility(_s3Client);
            await fileTransferUtility.UploadAsync(fileStream, bucketName, key);

            await _cartRepository.AddCartItemImage(cartItemId, key);
            return new OkResult();
        }
    }

    public async Task<IActionResult> DeleteCartItem(int id, string userId)
    {
        var cartItem = await _cartRepository.GetById(id);

        if (cartItem == null) return new BadRequestResult();
        if (!string.Equals(cartItem.UserId.ToLower().Trim(), userId)) return new UnauthorizedResult();

        _cartRepository.Delete(cartItem);
        return new OkResult();
    }

    public async Task<List<CartItemDto>> GetCartItems([FromQuery] CartItemsParams cartItemsParams, string userId)
    {
        cartItemsParams.UserId = userId;

        const string bucketName = Constants.ImagesBucketName;
        var c = await _cartRepository.GetCartItems(cartItemsParams);

        var cartItems = c.Select(x =>
        {
            var presignedUrl = "";
            if (x.ImageId != null)
            {
                // url to display
                var urlRequest = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = x.ImageId,
                    Expires = DateTime.UtcNow.AddHours(1)
                };

                presignedUrl = _s3Client.GetPreSignedURL(urlRequest);
            }

            var item = new CartItemDto
                {Description = x.Description, Price = x.Price, Name = x.Name, Url = presignedUrl};

            return item;
        }).ToList();

        return cartItems;
    }
}