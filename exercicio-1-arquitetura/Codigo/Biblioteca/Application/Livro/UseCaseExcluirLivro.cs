using Domain.Livro;

namespace Application.Livro;

public class UseCaseExcluirLivro
{
    private readonly ILivroRepository _repository;

    public UseCaseExcluirLivro(ILivroRepository repository)
    {
        _repository = repository;
    }

    public void Execute(uint id)
    {
        _repository.Delete(id);
    }
}