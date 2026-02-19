using RideFlow.Models;

public class RideRepository
{
    private readonly RideflowContext dbContext;

    public RideRepository(RideflowContext context)
    {
        dbContext = context;
    }

    public async Task Add(TbRide ride)
    {
        dbContext.TbRides.Add(ride);
        await dbContext.SaveChangesAsync();
    }

    
    public async Task<TbRide?> GetById(Guid id)
    {
        return await dbContext.TbRides.FindAsync(id);
    }

    
    public async Task<List<TbRide>> GetAll()
    {
        return dbContext.TbRides.ToList();
    }

    
    public async Task Update(TbRide ride)
    {
        dbContext.TbRides.Update(ride);
        await dbContext.SaveChangesAsync();
    }

   
  public bool DriverIsBusy(Guid driverId)
    {
        var ride = dbContext.TbRides
            .FirstOrDefault(r => r.DriverId == driverId 
                              && r.Status == RideStatus.in_progress);

        if (ride == null)
            return false;

        return true;
    }

}