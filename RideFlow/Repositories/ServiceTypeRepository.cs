using Microsoft.EntityFrameworkCore;
using RideFlow.Models;

namespace RideFlow.Repositories;

public class ServiceTypeRepository
{
    private readonly RideflowContext dbContext;

    public ServiceTypeRepository(RideflowContext context)
    {
        dbContext = context;
    }

     public TbServicetype? GetByCategory(ServiceCategory category)
    {
        return dbContext.TbServicetypes
            .FirstOrDefault(s => s.Category == category);
    }

    public TbServicetype? GetByCategoryString(string category)
    {
        return dbContext.TbServicetypes
            .AsEnumerable() 
            .FirstOrDefault(s => s.Category.ToString().ToLower() == category.ToLower());
    }

    public List<TbServicetype> GetAll()
    {
        return dbContext.TbServicetypes.ToList();
    }

    public TbServicetype? GetById(Guid id)
    {
        return dbContext.TbServicetypes.Find(id);
    }

    public async Task<TbServicetype?> GetByIdAsync(Guid id)
    {
        return await dbContext.TbServicetypes.FindAsync(id);
    }

    // método para verificar se existem registros
    public bool Exists()
    {
        return dbContext.TbServicetypes.Any();
    }
}