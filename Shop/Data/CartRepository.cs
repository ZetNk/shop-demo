using Shop.API.Helpers;
using Shop.API.Models;

namespace Shop.API.Data;

public class CartRepository : ICartRepository
{
    private readonly DataContext _context;

    public CartRepository(DataContext context)
    {
        _context = context;
    }

    public void Add<T>(T entity) where T : class
    {
        _context.Add(entity);
        _context.SaveChanges();
    }

    public async Task AddAsync(CartItem caartItem)
    {
        await _context.AddAsync(caartItem);
        await _context.SaveChangesAsync();
    }

    public async Task<CartItem?> GetById(int cartItemId)
    {
        return await _context.CartItems.FindAsync(cartItemId);
    }

    public async Task<bool> AddCartItemImage(int cartItemId, string imageId)
    {
        var cartItemFromRepo = await _context.CartItems.FindAsync(cartItemId);

        if (cartItemFromRepo != null)
            cartItemFromRepo.ImageId = imageId;

        return await _context.SaveChangesAsync() > 0;
    }

    public void Delete<T>(T entity) where T : class
    {
        _context.Remove(entity);
        _context.SaveChanges();
    }

    public async Task<PagedList<CartItem>> GetCartItems(CartItemsParams cartParams)
    {
        var userCartItems = _context.CartItems.Where(i => i.UserId == cartParams.UserId);
        return await PagedList<CartItem>.CreateAsync(userCartItems, cartParams.PageNumber, cartParams.PageSize);
    }
}