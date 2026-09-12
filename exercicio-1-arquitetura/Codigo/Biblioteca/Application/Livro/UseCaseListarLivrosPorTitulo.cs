using Application.Livro.DTOs;
using Domain.Livro;

namespace Application.Livro;

public class UseCaseListarLivrosPorTitulo
{
    private readonly ILivroRepository _repository;

    public UseCaseListarLivrosPorTitulo(ILivroRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<LivroDTO> ExecutePorTitulo(string titulo)
    {
        var livros = _repository.GetByTitulo(titulo);

        return livros.Select(l => new LivroDTO
        {
            Id = l.Id,
            Titulo = l.Titulo,
            Subtitulo = l.Subtitulo,
            Isbn = l.Isbn,
            EditoraId = l.EditoraId
        });
    }
}