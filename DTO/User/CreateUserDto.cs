public class CreateUserDto
{
    public Guid Id { get; set; }
    
    public string Nameuser { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public string? Phoneuser { get; set; }

}