using RideFlow.Models;

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
}