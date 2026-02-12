using NUnit.Framework;
using Moq;
using Microsoft.EntityFrameworkCore;

using RideFlow;
using RideFlow.Models;
using RideFlow.Repositories;
namespace RideFlow.Repositories;

[TestFixture]
public class UserRepositoryTests
{
    private Mock<RideflowContext> _contextMock;
    private Mock<DbSet<TbUser>> _dbSetMock;
    private UserRepository _repository;

    [SetUp]
    public void Setup()
    {
        _contextMock = new Mock<RideflowContext>();
        _dbSetMock = new Mock<DbSet<TbUser>>();

        _contextMock.Setup(c => c.TbUsers).Returns(_dbSetMock.Object);

        _repository = new UserRepository(_contextMock.Object);
    }

    [Test]
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