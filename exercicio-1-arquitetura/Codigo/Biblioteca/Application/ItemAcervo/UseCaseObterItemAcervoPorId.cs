using Domain.ItemAcervo;

namespace Application.ItemAcervo;

public class UseCaseObterItemAcervoPorId
{
    private readonly IItemAcervoRepository _itemAcervoRepository;

    public UseCaseObterItemAcervoPorId(IItemAcervoRepository itemAcervoRepository)
    {
        _itemAcervoRepository = itemAcervoRepository;
    }

    public ItemAcervoEntity? Execute(uint id)
    {
        return _itemAcervoRepository.GetById(id);
    }
}
