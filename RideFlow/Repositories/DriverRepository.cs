using RideFlow.Models;

namespace RideFlow.Repositories;

public class DriverRepository
{
    private RideflowContext dbContext;

    public DriverRepository(RideflowContext context)
    {
        dbContext = context;
    }

    public void Add(TbDriver driver)
    {
        dbContext.TbDrivers.Add(driver);
        dbContext.SaveChanges();
    }

    public TbDriver? GetByPlate(string plate)
    {
        return dbContext.TbDrivers
            .FirstOrDefault(d => d.Plate == plate);
    }

    public List<TbDriver> GetAllDrivers()
    {
        return dbContext.TbDrivers.ToList();
    }

     public TbDriver? GetById(Guid id)
    {
        return dbContext.TbDrivers.Find(id);
    }

    public async Task<TbDriver?> GetByIdAsync(Guid id)
    {
        return await dbContext.TbDrivers.FindAsync(id);
    }

    public TbDriver? GetByCnh(string cnh)
    {
        return dbContext.TbDrivers
            .FirstOrDefault(d => d.Cnh == cnh);
    }

}