public class UserResponseDto
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public string? Telefone { get; set; }

    public DateTime? DataDeCriacao { get; set; }
}