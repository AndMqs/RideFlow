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

    public List<TbDriver> GetAllDrives()
    {
        return dbContext.TbDrivers.ToList();
    }

     public TbDriver? GetById(Guid id)
    {
        return dbContext.TbDrivers.Find(id);
    }

       
}