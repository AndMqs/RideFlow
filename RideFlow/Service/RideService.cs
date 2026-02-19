using RideFlow.Models;


public class RideService
{
    private readonly RideRepository _rideRepository;
    private readonly DriverRepository _driverRepository;

    public RideService(RideRepository rideRepository, DriverRepository driverRepository)
    {
        _rideRepository = rideRepository;
        _driverRepository = driverRepository;
    }

/*
    public async Task<TbRide> StartRide( string Start, string Destiny, Guid UserId, string Category, decimal DistanceKm, PaymentMethod Payment)
    {
        //buscar motoristas da categoria
        var driver = await _driverRepository.D;

        // pegar primeiro disponível

        // calcular valor

         //  criar ride
    }

    */
}