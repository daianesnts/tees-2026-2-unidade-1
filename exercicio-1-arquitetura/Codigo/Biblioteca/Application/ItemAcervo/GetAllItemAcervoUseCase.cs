using Domain.ItemAcervo;

namespace Application.ItemAcervo;

public class GetAllItemAcervo
{
    private readonly IItemAcervoRepository _itemAcervoRepository;

    public GetAllItemAcervo(IItemAcervoRepository itemAcervoRepository)
    {
        _itemAcervoRepository = itemAcervoRepository;
    }

    public IEnumerable<ItemAcervoEntity> Execute()
    {
        return _itemAcervoRepository.GetAll();
    }
}
