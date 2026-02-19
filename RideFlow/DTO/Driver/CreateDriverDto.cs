public class CreateDriverDto
{
    public Guid Id { get; set; }

    public string Namedriver { get; set; } = null!;

    public string Cnh { get; set; } = null!;

    public string Plate { get; set; } = null!;

    public int Yearcar { get; set; }

    public string Modelcar { get; set; } = null!;

}