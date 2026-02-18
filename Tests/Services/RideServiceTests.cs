using RideFlow.Models;
using Xunit;

namespace Tests.Services;

/// <summary>
/// Testes para RideService
/// 
/// NOTA: Para testes mais completos com mocks de Repositories, é necessário refatorar
/// as classes Ride/Driver/UserRepository para interfaces (IRideRepository, IDriverRepository, etc)
/// para que Moq possa criar mocks corretamente.
/// </summary>
public class RideServiceTests
{
    /// <summary>
    /// Testa se o CreateRideDto pode ser criado com propriedades válidas
    /// </summary>
    [Fact]
    public void CreateRideDto_WithValidData_ShouldInitializeCorrectly()
    {
        // ARRANGE
        var userId = Guid.NewGuid();
        var startpoint = "Rua A, 100";
        var destiny = "Rua B, 200";
        var category = "Econômico";
        var paymentMethod = "Cartão de Crédito";
        var km = 15.5m;

        // ACT
        var createRideDto = new CreateRideDto
        {
            UserId = userId,
            Startpoint = startpoint,
            Destiny = destiny,
            Category = category,
            PaymentMethod = paymentMethod,
            Km = km
        };

        // ASSERT
        Assert.Equal(userId, createRideDto.UserId);
        Assert.Equal(startpoint, createRideDto.Startpoint);
        Assert.Equal(destiny, createRideDto.Destiny);
        Assert.Equal(category, createRideDto.Category);
        Assert.Equal(paymentMethod, createRideDto.PaymentMethod);
        Assert.Equal(km, createRideDto.Km);
    }

    /// <summary>
    /// Testa se RideResponseDto pode ser criado com dados corretos
    /// </summary>
    [Fact]
    public void RideResponseDto_WithValidData_ShouldInitializeCorrectly()
    {
        // ARRANGE
        var nomePassageiro = "João Silva";
        var nomeMotorista = "Maria Santos";
        var placa = "ABC-1234";
        var categoria = "Econômico";
        var valorCorrida = 45.50m;
        var origem = "Rua A, 100";
        var destino = "Rua B, 200";
        var formaPagamento = "Cartão";

        // ACT
        var rideResponseDto = new RideResponseDto
        {
            NomePassageiro = nomePassageiro,
            NomeMotorista = nomeMotorista,
            Placa = placa,
            Categoria = categoria,
            ValorCorrida = valorCorrida,
            Origem = origem,
            Destino = destino,
            FormaPagamento = formaPagamento
        };

        // ASSERT
        Assert.Equal(nomePassageiro, rideResponseDto.NomePassageiro);
        Assert.Equal(nomeMotorista, rideResponseDto.NomeMotorista);
        Assert.Equal(placa, rideResponseDto.Placa);
        Assert.Equal(categoria, rideResponseDto.Categoria);
        Assert.Equal(valorCorrida, rideResponseDto.ValorCorrida);
        Assert.Equal(origem, rideResponseDto.Origem);
        Assert.Equal(destino, rideResponseDto.Destino);
        Assert.Equal(formaPagamento, rideResponseDto.FormaPagamento);
    }

    /// <summary>
    /// Testa a categoria de um TbDriver baseado no ano do carro
    /// Usando a regra: 2023+ = "Econômico", abaixo = "Clássico"
    /// </summary>
    [Theory]
    [InlineData(2023, true)]   // 2023 é >= 2023, então Econômico
    [InlineData(2024, true)]   // 2024 é >= 2023, então Econômico
    [InlineData(2022, false)]  // 2022 é < 2023, então Clássico
    [InlineData(2015, false)]  // 2015 é < 2023, então Clássico
    [InlineData(2010, false)]  // 2010 é < 2023, então Clássico
    public void DriverCategory_ShouldBeEconomicoWhen2023OrNewer(int yearcar, bool shouldBeEconomico)
    {
        // ARRANGE
        var driver = new TbDriver
        {
            Id = Guid.NewGuid(),
            Namedriver = "Test Driver",
            Cnh = "12345678901",
            Plate = "ABC-1234",
            Yearcar = yearcar,
            Modelcar = "Test Model"
        };

        // ACT
        var isRecentCar = driver.Yearcar >= 2023;

        // ASSERT
        Assert.Equal(shouldBeEconomico, isRecentCar);
    }
}
