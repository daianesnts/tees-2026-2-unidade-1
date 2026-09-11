using Domain.ItemAcervo;
using Domain.Comum;

namespace Application.ItemAcervo;

public class GetAllItemAcervo
{
    private readonly IItemAcervoRepository _itemAcervoRepository;

    public GetAllItemAcervoUseCase(IItemAcervoRepository itemAcervoRepository)
    {
        _itemAcervoRepository = itemAcervoRepository;
    }

    public PagedResult<ItemAcervoEntity> Execute(PageRequest request)
    {
        return _itemAcervoRepository.GetPage(request);
    }
}
