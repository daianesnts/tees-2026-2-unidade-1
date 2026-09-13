using Domain.Autor;

namespace Application.Autor;

public class UseCaseListarAutores
{
    private readonly IAutorRepository _autorRepository;

    public UseCaseListarAutores(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }

    public IEnumerable<AutorEntity> Execute()
    {
        return _autorRepository.GetAll();
    }
}