using Moq;
using Microsoft.EntityFrameworkCore;
using RideFlow.Models;
using RideFlow.Repositories;
using Xunit;

namespace Tests.Repositories;

public class UserRepositoryTests
{
    private readonly Mock<RideflowContext> _contextMock;
    private readonly Mock<DbSet<TbUser>> _dbSetMock;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        _contextMock = new Mock<RideflowContext>();
        _dbSetMock = new Mock<DbSet<TbUser>>();

        _contextMock.Setup(c => c.TbUsers).Returns(_dbSetMock.Object);

        _repository = new UserRepository(_contextMock.Object);
    }

    [Fact]
    public void Add_ShouldAddUser()
    {
        // Arrange
        var user = new TbUser 
        { 
            Id = Guid.NewGuid(), 
            Nameuser = "Test User", 
            Cpf = "12345678900", 
            Phoneuser = "555-1234" 
        };

        // Act
        _repository.Add(user);

        // Assert
        _dbSetMock.Verify(x => x.Add(user), Times.Once);
        _contextMock.Verify(x => x.SaveChanges(), Times.Once);
    }
}