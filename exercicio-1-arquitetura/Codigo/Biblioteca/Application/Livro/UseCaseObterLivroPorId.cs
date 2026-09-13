using Application.Livro.DTOs;
using Domain.Livro;

namespace Application.Livro;

public class UseCaseObterLivroPorId
{
    private readonly ILivroRepository _repository;

    public UseCaseObterLivroPorId(ILivroRepository repository)
    {
        _repository = repository;
    }

    public LivroDTO? Execute(uint id)
    {
        var entity = _repository.GetById(id);
        if(entity == null)
        {
            return null;
        }

        return new LivroDTO
        {
            Id = entity.Id,
            Titulo = entity.Titulo,
            Subtitulo = entity.Subtitulo,
            Isbn = entity.Isbn,
            EditoraId = entity.EditoraId,
            DataPublicacao = entity.DataPublicacao,
            Resumo = entity.Resumo,
            FotoCapa = entity.FotoCapa
        };
    }
}