using Domain.Autor;

namespace Application.Autor;

public class UseCaseObterAutorPorId
{
    private readonly IAutorRepository _autorRepository;

    public UseCaseObterAutorPorId(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }

    public AutorEntity? Execute(uint id)
    {
        return _autorRepository.GetById(id);
    }
}