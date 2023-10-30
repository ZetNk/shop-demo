using Shop.API.Helpers;
using Shop.API.Models;

namespace Shop.API.Data;

public interface ICartRepository
{
    public Task AddAsync(CartItem cartItem);
    void Add<T>(T entity) where T : class;
    void Delete<T>(T entity) where T : class;
    Task<CartItem?> GetById(int cartItemId);
    Task<PagedList<CartItem>> GetCartItems(CartItemsParams cartParams);
    Task<bool> AddCartItemImage(int cartItemId, string imageId);
}