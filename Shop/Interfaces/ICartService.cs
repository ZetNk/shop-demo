using Microsoft.AspNetCore.Mvc;
using Shop.API.Dto;
using Shop.API.Helpers;
using Shop.API.Models;

namespace Shop.API.Interfaces;

public interface ICartService
{
    public Task<IActionResult> AddCartItem([FromBody] CartItem cartItem, string userId);
    public Task<List<CartItemDto>> GetCartItems([FromQuery] CartItemsParams cartItemsParams, string userId);
    public Task<IActionResult> AddCartItemImage(int cartItemId, [FromForm] ImageDto image, string userId);
    public Task<IActionResult> DeleteCartItem(int id, string userId);
}