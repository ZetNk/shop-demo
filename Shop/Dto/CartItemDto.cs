namespace Shop.API.Dto;

public class CartItemDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public float Price { get; set; }
    public string? Url { get; set; }
}