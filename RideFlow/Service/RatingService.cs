using RideFlow.Repositories;
using RideFlow.DTOs; 
using RideFlow.Models;

public class RatingService
{
    private readonly RatingRepository _ratingRepository;
    private readonly RideRepository _rideRepository;

    public RatingService(RatingRepository ratingRepository, RideRepository rideRepository)
    {
        _ratingRepository = ratingRepository;
        _rideRepository = rideRepository;
    }

     public async Task<RatingResponseDto> CreateRating(CreateRatingDto dto)
    {
        // 1 - Validar nota (1 a 5)
        if (dto.Rate < 1 || dto.Rate > 5)
        {
            throw new Exception("A nota deve ser entre 1 e 5, sendo 1 a mais insatisfeito e 5 a mais satisfeito.");
        }

        // 2 - Buscar a corrida
        var ride = await _rideRepository.GetById(dto.RideId);
        if (ride == null)
        {
            throw new Exception($"Corrida com ID {dto.RideId} não encontrada.");
        }

        // 3 - Verificar se a corrida está finalizada
        if (ride.Status != RideStatus.finished)
        {
            throw new Exception($"Só é possível avaliar corridas finalizadas. Status atual: {ride.Status}");
        }

        // 4 - Verificar se já existe avaliação para esta corrida
        var exists = await _ratingRepository.ExistsForRide(dto.RideId);
        if (exists)
        {
            throw new Exception("Esta corrida já foi avaliada.");
        }

        // 5 - Criar a avaliação
        var rating = new TbRating
        {
            Id = Guid.NewGuid(),
            RideId = dto.RideId,
            Rate = dto.Rate,
            Comment = dto.Comment
        };

        // 6 - Salvar no banco
        var savedRating = await _ratingRepository.AddAsync(rating);

        // 7 - Buscar a avaliação com os dados completos
        var ratingCompleto = await _ratingRepository.GetByIdAsync(savedRating.Id);

        // 8 - Montar mensagem
        string mensagem = dto.Rate switch
        {
            5 => "Excelente! Muito obrigado pela avaliação! ⭐⭐⭐⭐⭐",
            4 => "Ótimo! Agradecemos sua avaliação! ⭐⭐⭐⭐",
            3 => "Bom! Vamos melhorar ainda mais! ⭐⭐⭐",
            2 => "Que pena! Conte-nos como podemos melhorar. ⭐⭐",
            1 => "Sentimos muito! Entraremos em contato para entender melhor. ⭐",
            _ => "Avaliação registrada com sucesso."
        };

        // 9 - Retornar resposta
        return new RatingResponseDto
        {
            Id = ratingCompleto.Id,
            RideId = ratingCompleto.RideId,
            NomePassageiro = ratingCompleto.Ride?.User?.Nameuser ?? "Não informado",
            NomeMotorista = ratingCompleto.Ride?.Driver?.Namedriver ?? "Não informado",
            Rate = ratingCompleto.Rate,
            Comment = ratingCompleto.Comment,
            Mensagem = mensagem
        };
    }

     public async Task<List<RatingResponseDto>> GetRatingsByDriver(Guid driverId)
    {
        var ratings = await _ratingRepository.GetByDriverId(driverId);
        
        return ratings.Select(r => new RatingResponseDto
        {
            Id = r.Id,
            RideId = r.RideId,
            NomePassageiro = r.Ride?.User?.Nameuser ?? "Não informado",
            NomeMotorista = r.Ride?.Driver?.Namedriver ?? "Não informado",
            Rate = r.Rate,
            Comment = r.Comment,
            Mensagem = $"Avaliação {r.Rate} estrelas"
        }).ToList();
    }

    public async Task<double> GetAverageRatingByDriver(Guid driverId)
    {
        var ratings = await _ratingRepository.GetByDriverId(driverId);
        
        if (!ratings.Any())
            return 0;
            
        return ratings.Average(r => r.Rate);
    }
}