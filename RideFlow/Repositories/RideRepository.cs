using Microsoft.EntityFrameworkCore;
using RideFlow.Models;
using RideFlow.DTOs;


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

    public async Task<List<HistoricoCorridaDto>> GetHistoricoByUserId(Guid userId)
{
    var historico = await dbContext.TbRides
        .Where(r => r.UserId == userId)
        .Include(r => r.User)
        .Include(r => r.Driver)
        .Include(r => r.Servicetype)
        .Include(r => r.TbRatings) // Isso traz as avaliações
        .Select(r => new HistoricoCorridaDto
        {
            Id = r.Id,
            NomePassageiro = r.User.Nameuser,
            NomeMotorista = r.Driver.Namedriver,
            Placa = r.Driver.Plate,
            Categoria = r.Servicetype.Category.ToString(),
            Valor = r.TotalValue,
            Origem = r.Startpoint,
            Destino = r.Destiny,
            Status = r.Status.ToString(),
            Data = r.CreatedAt ?? DateTime.MinValue,
            Avaliacao = r.TbRatings.FirstOrDefault() != null ? r.TbRatings.First().Rate : (int?)null,
            Comentario = r.TbRatings.FirstOrDefault() != null ? r.TbRatings.First().Comment : null
        })
        .ToListAsync();
    
    return historico;
}

}