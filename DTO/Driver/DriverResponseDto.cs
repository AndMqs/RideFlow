public class DriverResponseDto
{
     public Guid Id { get; set; }

    public string Nome { get; set; } = null!;

    public string CNH { get; set; } = null!;

    public string Placa { get; set; } = null!;

    public int AnoDoCarro { get; set; }

    public string ModeloDoCarro { get; set; } = null!;

    public DateTime? DataDeCriacao { get; set; }
}