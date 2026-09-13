using Domain.ItemAcervo;

namespace Application.ItemAcervo;

public class UseCaseObterItemAcervoPorId
{
    private readonly IItemAcervoRepository _itemAcervoRepository;

    public UseCaseObterItemAcervoPorId(IItemAcervoRepository itemAcervoRepository)
    {
        _itemAcervoRepository = itemAcervoRepository;
    }

    public ItemAcervoDTO? Execute(uint id)
    {
        var entity = _itemAcervoRepository.GetById(id);
        if (entity == null)
            return null;
        return new ItemAcervoDTO
        {
            Id = entity.Id,
            IdLivro = entity.IdLivro,
            IdBiblioteca = entity.IdBiblioteca,
            IdSituacaoItemAcervo = entity.IdSituacaoItemAcervo,
            IdDoacao = entity.IdDoacao,
            DataAquisicao = entity.DataAquisicao
        };
    }
}
