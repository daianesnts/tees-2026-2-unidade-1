using Domain.ItemAcervo;

namespace Application.ItemAcervo;

public class CreateItemAcervoUseCase
{
    private readonly IItemAcervoRepository _itemAcervoRepository;

    public CreateItemAcervoUseCase(IItemAcervoRepository itemAcervoRepository)
    {
        _itemAcervoRepository = itemAcervoRepository;
    }

    public uint Execute(ItemAcervoEntity item)
    {
        _itemAcervoRepository.Create(item);
        return item.Id;
    }
}

