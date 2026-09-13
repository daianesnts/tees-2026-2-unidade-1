using Domain.Autor;

namespace Application.Autor;

public class UseCaseObterAutorPorNome : IObterAutorPorNome
{
    private readonly IAutorRepository _autorRepository;

    public UseCaseObterAutorPorNome(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }

    public IEnumerable<AutorDTO> Execute(string nome)
    {
        var autores = _autorRepository.GetByName(nome);

        return autores.Select(autor => new AutorDTO
        {
            Id = autor.Id,
            Nome = autor.Nome,
            DataNascimento = autor.DataNascimento
        });
    }
}
