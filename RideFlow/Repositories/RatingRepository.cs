using RideFlow.Models;
using Microsoft.EntityFrameworkCore;

namespace RideFlow.Repositories;

public class RatingRepository
{
    private RideflowContext dbContext;

    public RatingRepository(RideflowContext context)
    {
        dbContext = context;
    }

     public async Task<TbRating> AddAsync(TbRating rating)
    {
        await dbContext.TbRatings.AddAsync(rating);
        await dbContext.SaveChangesAsync();
        return rating;
    }

    public List<TbRating> GetAllRatings()
    {
        return dbContext.TbRatings.ToList();
    }

    public TbRating? GetById(Guid id)
    {
        return dbContext.TbRatings.Find(id);
    }

    public async Task<TbRating?> GetByIdAsync(Guid id)
    {
        return await dbContext.TbRatings.FindAsync(id);
    }

    public void UpdateRating(TbRating rating)
    {
        dbContext.TbRatings.Update(rating);
        dbContext.SaveChanges();
    }

    public TbRating? DeleteRating(Guid Id)
    {
        var rating = dbContext.TbRatings.Find(Id);

        if(rating == null)
            return null;
        
        dbContext.TbRatings.Remove(rating);
        dbContext.SaveChanges();

        return rating;
    }

        public async Task<List<TbRating>> GetByRideId(Guid rideId)
    {
        return await dbContext.TbRatings
            .Where(r => r.RideId == rideId)
            .Include(r => r.Ride)
                .ThenInclude(r => r.User)
            .Include(r => r.Ride)
                .ThenInclude(r => r.Driver)
            .ToListAsync();
    }

    public async Task<List<TbRating>> GetByDriverId(Guid driverId)
    {
         return await dbContext.TbRatings
        .Include(r => r.Ride)  
            .ThenInclude(r => r.User) 
        .Include(r => r.Ride) 
            .ThenInclude(r => r.Driver)
        .Where(r => r.Ride.DriverId == driverId)
        .ToListAsync();  
    }

     public async Task<bool> ExistsForRide(Guid rideId)
    {
        return await dbContext.TbRatings.AnyAsync(r => r.RideId == rideId);
    }

}