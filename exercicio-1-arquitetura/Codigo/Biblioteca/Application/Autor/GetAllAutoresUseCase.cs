using Domain.Autor;

namespace Application.Autor;

public class GetAllAutoresUseCase
{
    private readonly IAutorRepository _autorRepository;

    public GetAllAutoresUseCase(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }

    public IEnumerable<AutorEntity> Execute()
    {
        return _autorRepository.GetAll();
    }
}