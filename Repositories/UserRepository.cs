using RideFlow.Models;
using Microsoft.EntityFrameworkCore;

public class UserRepository
{
    private RideflowContext dbContext;

    public UserRepository(RideflowContext context)
    {
        dbContext = context;
    }

    public void Add(TbUser user)
    {
        dbContext.TbUsers.Add(user);
        dbContext.SaveChanges();
    }

    public List<TbUser> GetAllUsers()
    {
        return dbContext.TbUsers.ToList();
    }

    public TbUser? GetById(Guid id)
    {
        return dbContext.TbUsers.Find(id);
    }

    public void UpdateUsers(TbUser user)
    {
        dbContext.TbUsers.Update(user);
        dbContext.SaveChanges();
    }

    public TbUser? DeleteUsers(Guid Id)
    {
        var user = dbContext.TbUsers.Find(Id);

        if(user == null)
            return null;
        
        dbContext.TbUsers.Remove(user);
        dbContext.SaveChanges();

        return user;
    }

}