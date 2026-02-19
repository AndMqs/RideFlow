using RideFlow.DTOs;
using RideFlow.Models;
using RideFlow.Repositories;

namespace RideFlow.Service;

public class RideService
{
    private readonly RideRepository _rideRepository;
    private readonly UserRepository _userRepository;
    private readonly DriverRepository _driverRepository;
    private readonly ServiceTypeRepository _serviceTypeRepository;

    private const decimal RATE_CANCEL = 0.3m;

    public RideService(RideRepository rideRepository, DriverRepository driverRepository, ServiceTypeRepository serviceTypeRepository, UserRepository userRepository)
    {
        _rideRepository = rideRepository;
        _driverRepository = driverRepository;
        _serviceTypeRepository = serviceTypeRepository;
        _userRepository = userRepository;
    }

    private bool DriverIsAvailable(Guid driverId)
    {
        var ride = _rideRepository.GetRideInProgressByDriver(driverId);
        return ride == null;
    }

    public RideResponseDto CreateRide(CreateRideDto dto)
    {
        if (dto.Km <= 0)
            throw new Exception("A distância deve ser maior que zero.");

        // 1 — buscar todos motoristas
        List<TbDriver> drivers = _driverRepository.GetAllDrivers();

        if (drivers.Count == 0)
            throw new Exception("Não há motoristas cadastrados.");

        TbDriver? driverChosen = null;

        // 2 — encontrar motorista da categoria e livre
        foreach (var driver in drivers)
        {
            string categoryDriver = DriverCategoryRules.GetCategoryByCarYear(driver.Yearcar);

            if (categoryDriver.Equals(dto.Category, StringComparison.OrdinalIgnoreCase))
            {
                bool isAvailable = DriverIsAvailable(driver.Id);

                if (isAvailable)
                {
                    driverChosen = driver;
                    break;
                }
            }
        }

        // 3 — se nenhum motorista disponível
        if (driverChosen == null)
        {
            throw new Exception("Nenhum motorista disponível para a categoria escolhida.");
        }

        // 4 — calcular preço
        decimal totalPrice = PriceRules.GetRidePrice(dto.Category, dto.Km);

        // 5 — obter o ServiceType baseado na categoria
        if (!Enum.TryParse<ServiceCategory>(dto.Category, true, out var categoryEnum))
        {
            throw new Exception($"Categoria '{dto.Category}' inválida. Use: basic, premium, vip");
        }

        var serviceType = _serviceTypeRepository.GetByCategory(categoryEnum);
        if (serviceType == null)
        {
            throw new Exception($"Categoria de serviço '{dto.Category}' não encontrada.");
        }

        // 6 — converter o método de pagamento para o enum
        PaymentMethod paymentMethod = dto.PaymentMethod?.ToLower() switch
        {
            "credit_card" => PaymentMethod.credit_card,
            "debit_card" => PaymentMethod.debit_card,
            "pix" => PaymentMethod.pix,
            _ => throw new Exception($"Método de pagamento '{dto.PaymentMethod}' inválido. Use: credit_card, debit_card, cash ou pix")
        };

        // 7 — buscar o nome do usuário no banco
        var user = _userRepository.GetById(dto.UserId); // <-- PRECISA INJETAR O UserRepository
        string nomePassageiro = user?.Nameuser ?? "Usuário não encontrado";

        // 8 — criar ride
        TbRide ride = new TbRide
        {
            Id = Guid.NewGuid(),
            Startpoint = dto.Startpoint,
            Destiny = dto.Destiny,
            UserId = dto.UserId,
            DriverId = driverChosen.Id,
            ServicetypeId = serviceType.Id,
            CreatedAt = DateTime.UtcNow,
            DistanceKm = dto.Km,
            TotalValue = totalPrice,
            PaymentMethod = paymentMethod,
            Status = RideStatus.in_progress
        };

        // 9 — salvar no banco
        _rideRepository.CreateRide(ride);

        // 10 — retorno estilo Uber
        RideResponseDto response = new RideResponseDto
        {
            Id = ride.Id,
            NomePassageiro = nomePassageiro,
            NomeMotorista = driverChosen.Namedriver,
            Placa = driverChosen.Plate,
            Categoria = dto.Category,
            ValorCorrida = totalPrice,
            Origem = dto.Startpoint,
            Destino = dto.Destiny,
            FormaPagamento = dto.PaymentMethod,
            Status = ride.Status.ToString()
        };

        return response;
    }

    public async Task<List<RideResponseDto>> GetAllRiders()
    {
        var rides = await _rideRepository.GetAllRiders();
        List<RideResponseDto> response = new List<RideResponseDto>();

        foreach (var ride in rides)
        {
            RideResponseDto dto = new RideResponseDto()
            {
                Id = ride.Id,
                NomePassageiro = ride.User?.Nameuser ?? "Usuário não encontrado",
                NomeMotorista = ride.Driver?.Namedriver ?? "Motorista não encontrado",
                Placa = ride.Driver?.Plate ?? "Placa não disponível",
                Categoria = ride.Servicetype?.Category.ToString() ?? "Categoria não disponível",
                ValorCorrida = ride.TotalValue,
                Origem = ride.Startpoint,
                Destino = ride.Destiny,
                FormaPagamento = ride.PaymentMethod.ToString(),
                Status = ride.Status.ToString() 
            };

            response.Add(dto);
        }

        return response;
    }

    public List<TbRide> GetRidesByStatus(string status)
    {
        if (!Enum.TryParse<RideStatus>(status, true, out var rideStatus))
            throw new Exception("Status inválido");

        return _rideRepository.GetRideByStatus(rideStatus);
    }

    public async Task<CancelRideResponseDto> CancelRide(Guid rideId)
    {
        // 1 - Buscar a corrida pelo ID 
        var ride = await _rideRepository.GetById(rideId); 
        if (ride == null)
        {
            throw new Exception($"Corrida com ID {rideId} não encontrada.");
        }

        // 2 - Verificar se a corrida pode ser cancelada (só pode cancelar se estiver em andamento)
        if (ride.Status != RideStatus.in_progress)
        {
            throw new Exception($"Não é possível cancelar uma corrida com status '{ride.Status}'.");
        }

        // 3 - Calcular valor do cancelamento 
        decimal valorOriginal = ride.TotalValue;
        decimal valorCancelamento = valorOriginal * RATE_CANCEL;
        decimal valorAPagar = valorCancelamento;; 

        // 4 - Atualizar status da corrida para 'canceled'
        ride.Status = RideStatus.canceled;
        
        // 5 - Salvar alteração no banco 
        await _rideRepository.Update(ride); 

        string mensagem = $"Sua corrida foi cancelada com sucesso. " +
                     $"Valor original: R$ {valorOriginal:F2}. " +
                     $"Taxa de cancelamento (30%): R$ {valorCancelamento:F2}. " +
                     $"Valor a pagar: R$ {valorAPagar:F2}.";

        // 6 - Retornar resposta com as informações
        return new CancelRideResponseDto
        {
            Id = ride.Id,
            Status = ride.Status.ToString(),
            ValorOriginal = valorOriginal.ToString("F2"),
            ValorCancelamento = valorCancelamento.ToString("F2"),
            ValorAPagar = valorAPagar.ToString("F2"),
            Mensagem = mensagem
        };
    }

    public async Task<FinishedRideResponseDto> FinishRide(Guid rideId)
    {
        var ride = await _rideRepository.GetById(rideId);
        if (ride == null)
        {
            throw new Exception($"Corrida com ID {rideId} não encontrada.");
        }

        if (ride.Status != RideStatus.in_progress)
        {
            throw new Exception($"Não é possível finalizar uma corrida com status '{ride.Status}'.");
        }

       var user = await _userRepository.GetByIdAsync(ride.UserId);
       var driver = await _driverRepository.GetByIdAsync(ride.DriverId);
       var serviceType = await _serviceTypeRepository.GetByIdAsync(ride.ServicetypeId);

       decimal valorAPagar = ride.TotalValue;

        ride.Status = RideStatus.finished;

        await _rideRepository.Update(ride);

        string mensagem = $"Corrida finalizada com sucesso! " +
                     $"Valor total: R$ {valorAPagar:F2}. " +
                     $"Forma de pagamento: {ride.PaymentMethod}. " +
                     $"Obrigado por viajar conosco!";

        return new FinishedRideResponseDto
        {
            Id = ride.Id,
            NomePassageiro = user?.Nameuser ?? "Usuário não encontrado",
            NomeMotorista = driver?.Namedriver ?? "Motorista não encontrado",
            Categoria = serviceType?.Category.ToString() ?? "Categoria não disponível",
            Status = ride.Status.ToString(),
            FormaPagamento = ride.PaymentMethod.ToString(),
            ValorAPagar = valorAPagar.ToString("F2"),
            Mensagem = mensagem
        };

    }


}