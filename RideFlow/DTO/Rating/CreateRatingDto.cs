namespace RideFlow.DTOs;

public class CreateRatingDto
{
    public Guid RideId { get; set; }
    public int Rate { get; set; } 
    public string? Comment { get; set; } 
}