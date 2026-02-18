using Microsoft.EntityFrameworkCore;
using RideFlow.Models;


namespace RideFlow.Repositories;

public class RideRepository
{
    private readonly RideflowContext dbContext;

    public RideRepository(RideflowContext context)
    {
        dbContext = context;
    }

    public void CreateRide(TbRide ride)
    {
        dbContext.TbRides.Add(ride);
        dbContext.SaveChanges();
    }
    
    public async Task<TbRide?> GetById(Guid id)
    {
        return await dbContext.TbRides
            .Include(r => r.User)
            .Include(r => r.Driver)
            .Include(r => r.Servicetype)
            .FirstOrDefaultAsync(r => r.Id == id);
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

    public TbRide? GetRideInProgressByDriver(Guid driverId)
    {
        return dbContext.TbRides
            .FirstOrDefault(r => r.DriverId == driverId 
                            && r.Status == RideStatus.in_progress);
    }

    public async Task<List<TbRide>> GetAllRiders()
    {
        return await dbContext.TbRides
            .Include(r => r.User)           
            .Include(r => r.Driver)          
            .Include(r => r.Servicetype)     
            .ToListAsync();
    }

    public List<TbRide> GetRideByStatus(RideStatus status)
    {
        return dbContext.TbRides
            .Where(r => r.Status == status)
            .ToList();
    }

}