using RideFlow.Models;

public class UserService
{
    private readonly UserRepository _repository;
    public UserService(UserRepository repository)
    {
        _repository = repository;
    }

    public void CreateUser(CreateUserDto dto)
    {
        var user = new TbUser
        {
            Nameuser = dto.Nameuser,
            Cpf = dto.Cpf,
            Phoneuser = dto.Phoneuser

        };

        _repository.Add(user);
    }

    public List<UserResponseDto> GetUsers()
    {
       
       List<TbUser> tbUsers = _repository.GetAllUsers();
       List<UserResponseDto> usersDto = new List<UserResponseDto>();


        foreach(TbUser tbUser in tbUsers)
        {
            UserResponseDto dto = new UserResponseDto();
            dto.Id = tbUser.Id;
            dto.Nome = tbUser.Nameuser;
            dto.Cpf = tbUser.Cpf;
            dto.Telefone = tbUser.Phoneuser;
            dto.DataDeCriacao = tbUser.CreatedAt;

            usersDto.Add(dto);
        }

        return usersDto;
        
    }

    public UserResponseDto? UpdateUser(Guid Id, UpdateUserDto dto)
    {
        var user = _repository.GetById(Id);
        
        if (user == null)
        return null;

        Console.WriteLine($"Nome recebido: {dto.Nameuser}");
        Console.WriteLine($"Cpf recebido: {dto.Cpf}");
        Console.WriteLine($"Telefone recebido: {dto.Phoneuser}");   

        if (dto.Nameuser != null)
            user.Nameuser = dto.Nameuser;

        if (dto.Cpf != null)
            user.Cpf = dto.Cpf;

        if (dto.Phoneuser != null)
            user.Phoneuser = dto.Phoneuser;

        _repository.UpdateUsers(user);

        return new UserResponseDto
        {
            Nome = user.Nameuser,
            Cpf = user.Cpf,
            Telefone = user.Phoneuser
        };
    }

    public void  DeleteUser(Guid Id)
    {
        var deleted = _repository.DeleteUsers(Id);

      if (deleted == null)
        throw new Exception("Usuário não encontrado");
    }


}