using Domain.ItemAcervo;

namespace Application.ItemAcervo;

public class GetAllItemAcervoById
{
    private readonly IItemAcervoRepository _itemAcervoRepository;

    public GetAllItemAcervoById(IItemAcervoRepository itemAcervoRepository)
    {
        _itemAcervoRepository = itemAcervoRepository;
    }

    public ItemAcervoEntity? Execute(uint id)
    {
        return _itemAcervoRepository.GetById(id);
    }
}
