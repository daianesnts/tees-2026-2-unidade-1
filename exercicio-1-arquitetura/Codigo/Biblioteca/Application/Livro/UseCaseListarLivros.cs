using Application.Livro.DTOs;
using Domain.Livro;

namespace Application.Livro;

public class UseCaseListarLivros
{
    private readonly ILivroRepository _repository;

    public UseCaseListarLivros(ILivroRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<LivroDTO> Execute()
    {
        var livros = _repository.GetAll();

        return livros.Select(l => new LivroDTO
        {
            Id = l.Id,
            Titulo = l.Titulo,
            Subtitulo = l.Subtitulo,
            Isbn = l.Isbn,
            EditoraId = l.EditoraId,
            DataPublicacao = l.DataPublicacao,
            Resumo = l.Resumo,
            FotoCapa = l.FotoCapa
        });
    }
}