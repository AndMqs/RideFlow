using RideFlow.Models;

public class DriverService
{
    private readonly DriverRepository _repository;

    public DriverService(DriverRepository repository)
    {
        _repository = repository;
    }

    public void CreateDriver(CreateDriverDto dto)
    {
        var existingDriver = _repository.GetByPlate(dto.Plate);

        if(existingDriver != null)
            throw new Exception("Erro. Já existe um motorista cadastrado com essa placa.");

        var driver = new TbDriver
        {
            Namedriver = dto.Namedriver,
            Cnh = dto.Cnh,
            Plate = dto.Plate,
            Yearcar = dto.Yearcar,
            Modelcar = dto.Modelcar,

        };
        _repository.Add(driver);
    }
}