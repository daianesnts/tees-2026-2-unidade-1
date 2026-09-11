using Domain.ItemAcervo;

namespace Application.ItemAcervo;

public class GetItemAcervoByIdUseCase
{
    private readonly IItemAcervoRepository _itemAcervoRepository;

    public GetItemAcervoByIdUseCase(IItemAcervoRepository itemAcervoRepository)
    {
        _itemAcervoRepository = itemAcervoRepository;
    }

    public ItemAcervoEntity? Execute(uint id)
    {
        return _itemAcervoRepository.GetById(id);
    }
}
