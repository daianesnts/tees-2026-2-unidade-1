using Domain.Autor;

namespace Application.Autor;

public class UseCaseListarAutores : IListarAutores
{
    private readonly IAutorRepository _autorRepository;

    public UseCaseListarAutores(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }

    public IEnumerable<AutorDTO> Execute()
    {
        var autores = _autorRepository.GetAll();
        return autores.Select(autor => new AutorDTO
        {
            Id = autor.Id,
            Nome = autor.Nome,
            DataNascimento = autor.DataNascimento
        });
    }
}