using Application.Livro.DTOs;
using Domain.Livro;

namespace Application.Livro;

public class UseCaseEditarLivro
{
    private readonly ILivroRepository _repository;

    public UseCaseEditarLivro(ILivroRepository repository)
    {
        _repository = repository;
    }

    public void Execute(EditarLivroDTO dto)
    {
        var entity = new LivroEntity
        {
            Id = dto.Id,
            Titulo = dto.Titulo,
            Subtitulo = dto.Subtitulo,
            Isbn = dto.Isbn,
            EditoraId = dto.EditoraId
        };

        _repository.Update(entity);
    }
}