using Domain.Autor;

namespace Application.Autor;

public class GetAutorByIdUseCase
{
    private readonly IAutorRepository _autorRepository;

    public GetAutorByIdUseCase(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }

    public AutorEntity? Execute(uint id)
    {
        return _autorRepository.GetById(id);
    }
}