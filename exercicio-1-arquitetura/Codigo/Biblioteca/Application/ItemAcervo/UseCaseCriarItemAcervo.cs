using Domain.ItemAcervo;

namespace Application.ItemAcervo;

public class UseCaseCriarItemAcervo
{
    private readonly IItemAcervoRepository _itemAcervoRepository;

    public UseCaseCriarItemAcervo(IItemAcervoRepository itemAcervoRepository)
    {
        _itemAcervoRepository = itemAcervoRepository;
    }

    public uint Execute(ItemAcervoEntity item)
    {
        _itemAcervoRepository.Create(item);
        return item.Id;
    }
}

