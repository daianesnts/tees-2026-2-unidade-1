using Domain.ItemAcervo;

namespace Application.ItemAcervo;

public class UseCaseCriarItemAcervo
{
    private readonly IItemAcervoRepository _itemAcervoRepository;

    public UseCaseCriarItemAcervo(IItemAcervoRepository itemAcervoRepository)
    {
        _itemAcervoRepository = itemAcervoRepository;
    }

    public uint Execute(ItemAcervoDTO dto)
    {
        var entity = new ItemAcervoEntity
        {
            IdLivro = dto.IdLivro,
            IdSituacaoItemAcervo = dto.IdSituacaoItemAcervo,
            IdDoacao = dto.IdDoacao,
            DataAquisicao = dto.DataAquisicao
        };
        _itemAcervoRepository.Create(entity);
        return entity.Id;
    }
}

