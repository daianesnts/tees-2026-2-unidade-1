using Domain.Autor;

namespace Application.Autor;

public class DeleteAutorUseCase
{
    private readonly IAutorRepository _autorRepository;

    public DeleteAutorUseCase(IAutorRepository autorRepository)
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