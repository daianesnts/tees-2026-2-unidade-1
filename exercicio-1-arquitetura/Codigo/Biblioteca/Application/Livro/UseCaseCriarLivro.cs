using Application.Livro.DTOs;
using Domain.Livro;

namespace Application.Livro;

public class UseCaseCriarLivro
{
    private readonly ILivroRepository _repository;

    public UseCaseCriarLivro(ILivroRepository repository)
    {
        _repository = repository;
    }

    public uint Execute(CriarLivroDTO dto)
    {
        var entity = new LivroEntity
        {
            Titulo = dto.Titulo,
            Subtitulo = dto.Subtitulo,
            Isbn = dto.Isbn,
            EditoraId = dto.EditoraId
        };

        return _repository.Add(entity);
    }
}