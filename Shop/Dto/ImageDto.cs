namespace Shop.API.Dto;

public class ImageDto
{
    public string? Url { get; set; }
    public required IFormFile File { get; set; }
    public string? PublicId { get; set; }
}