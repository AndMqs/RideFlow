using Moq;
using Microsoft.EntityFrameworkCore;
using RideFlow.Models;
using RideFlow.Repositories;
using Xunit;

namespace Tests.Repositories;

public class RideRepositoryTests
{
    private readonly Mock<RideflowContext> _contextMock;
    private readonly Mock<DbSet<TbRide>> _dbSetMock;
    private readonly RideRepository _repository;

    public RideRepositoryTests()
    {
        _contextMock = new Mock<RideflowContext>();
        _dbSetMock = new Mock<DbSet<TbRide>>();

        _contextMock.Setup(c => c.TbRides).Returns(_dbSetMock.Object);

        _repository = new RideRepository(_contextMock.Object);
    }

    [Fact]
    public void CreateRide_ShouldAddRideToDatabase()
    {
        // Arrange
        var ride = new TbRide
        {
            Id = Guid.NewGuid(),
            DriverId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Startpoint = "Location A",
            Destiny = "Location B",
            DistanceKm = 10,
            TotalValue = 20,
            Status = RideStatus.in_progress,
            ServicetypeId = Guid.NewGuid(),
            PaymentMethod = PaymentMethod.credit_card
        };

        // Act
        _repository.CreateRide(ride);

        // Assert
        _dbSetMock.Verify(x => x.Add(ride), Times.Once);
        _contextMock.Verify(x => x.SaveChanges(), Times.Once);
    }
}