using Domain.ItemAcervo;

namespace Application.ItemAcervo;

public class GetAllItemAcervoByIdUseCase
{
    private readonly IItemAcervoRepository _itemAcervoRepository;

    public GetAllItemAcervoByIdUseCase(IItemAcervoRepository itemAcervoRepository)
    {
        _itemAcervoRepository = itemAcervoRepository;
    }

    public ItemAcervoEntity? Execute(uint id)
    {
        return _itemAcervoRepository.GetById(id);
    }
}
