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

    public List<DriverResponseDto> GetDrivers()
    {
        List<TbDriver> tbDrivers = _repository.GetAllDrives();
        List<DriverResponseDto> driversDto = new List<DriverResponseDto>();

        foreach(TbDriver tbDriver in tbDrivers)
        {
            DriverResponseDto dto = new DriverResponseDto();
            dto.Id = tbDriver.Id;
            dto.Nome = tbDriver.Namedriver;
            dto.CNH = tbDriver.Cnh;
            dto.Placa = tbDriver.Plate;
            dto.ModeloDoCarro = tbDriver.Modelcar;
            dto.AnoDoCarro = tbDriver.Yearcar;
            dto.DataDeCriacao = tbDriver.CreatedAt;

            driversDto.Add(dto);
        }

        return driversDto;
    }
}
   