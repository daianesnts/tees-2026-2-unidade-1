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
        var entity = _repository.GetById(dto.Id);

        if(entity != null)
        {
            entity.Titulo = dto.Titulo;
            entity.Subtitulo = dto.Subtitulo;
            entity.Isbn = dto.Isbn;
            entity.EditoraId = dto.EditoraId;
            entity.DataPublicacao = dto.DataPublicacao;
            entity.Resumo = dto.Resumo;
            entity.FotoCapa = dto.FotoCapa;

            _repository.Update(entity);
        }
    }
}