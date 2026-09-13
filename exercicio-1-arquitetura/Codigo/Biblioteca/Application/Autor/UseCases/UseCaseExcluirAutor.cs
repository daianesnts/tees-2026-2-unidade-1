using Domain.Autor;

namespace Application.Autor;

public class UseCaseExcluirAutor : IExcluirAutor
{
    private readonly IAutorRepository _autorRepository;

    public UseCaseExcluirAutor(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }

    public void Execute(uint id)
    {
        var autor = _autorRepository.GetById(id);

        if (autor == null)
        {
            throw new Exception("Autor não encontrado.");
        }

        _autorRepository.Delete(id);
    }
}