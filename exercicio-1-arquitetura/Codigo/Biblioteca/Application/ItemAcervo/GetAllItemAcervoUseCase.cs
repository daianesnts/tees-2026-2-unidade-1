using Domain.ItemAcervo;

namespace Application.ItemAcervo;

public class GetAllItemAcervo
{
    private readonly IItemAcervoRepository _itemAcervoRepository;

    public GetAllItemAcervoUseCase(IItemAcervoRepository itemAcervoRepository)
    {
        _itemAcervoRepository = itemAcervoRepository;
    }

    public IEnumerable<ItemAcervoEntity> Execute()
    {
        return _itemAcervoRepository.GetAll();
    }
}
