using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.API.Dto;
using Shop.API.Helpers;
using Shop.API.Interfaces;
using Shop.API.Models;

namespace Shop.API.Controllers;

[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }


    [HttpPost("add-cart-item")]
    public async Task<IActionResult> AddCartItem([FromBody] CartItem cartItem)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrWhiteSpace(userEmail)) return Unauthorized("Email address should not be null");

        return await _cartService.AddCartItem(cartItem, userEmail);
    }

    [HttpGet("get-cart-items")]
    public async Task<IActionResult> GetCartItems([FromQuery] CartItemsParams cartItemsParams)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value.ToLower().Trim();

        if (string.IsNullOrWhiteSpace(userEmail)) return Unauthorized("Email address should not be null");

        var cartItems = await _cartService.GetCartItems(cartItemsParams, userEmail);

        return Ok(cartItems);
    }

    [HttpPost("add-cart-item-image")]
    public async Task<IActionResult> AddCartItemImage(int cartItemId, [FromForm] ImageDto image)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value.ToLower().Trim();

        if (string.IsNullOrWhiteSpace(userEmail)) return Unauthorized("Email address should not be null");

        return await _cartService.AddCartItemImage(cartItemId, image, userEmail);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value.ToLower().Trim();

        if (string.IsNullOrWhiteSpace(userEmail)) return Unauthorized("Email address should not be null");

        return await _cartService.DeleteCartItem(id, userEmail);
    }
}