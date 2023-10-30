﻿namespace Shop.API.Models;

public class CartItem
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required float Price { get; set; }
    public string? ImageId { get; set; }
    public required string UserId { get; set; }
}