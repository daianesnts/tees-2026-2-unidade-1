using Domain.ItemAcervo;

namespace Application.ItemAcervo;

public class UseCaseListarItemAcervo
{
    private readonly IItemAcervoRepository _itemAcervoRepository;

    public UseCaseListarItemAcervo(IItemAcervoRepository itemAcervoRepository)
    {
        _itemAcervoRepository = itemAcervoRepository;
    }

    public IEnumerable<ItemAcervoEntity> Execute()
    {
        return _itemAcervoRepository.GetAll();
    }
}
