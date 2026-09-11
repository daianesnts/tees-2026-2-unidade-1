using Domain.ItemAcervo;

namespace Application.ItemAcervo;

public class UpdateItemAcervoPage
{
    private readonly IItemAcervoRepository _itemAcervoRepository;

    public UpdateItemAcervoPage(IItemAcervoRepository itemAcervoRepository)
    {
        _itemAcervoRepository = itemAcervoRepository;
    }

    public bool Execute(ItemAcervoEntity item)
    {
        var itemExistente = _itemAcervoRepository.GetById(item.Id);

        if(itemExistente == null)
        {
            return false;
        }

        _itemAcervoRepository.Update(item);
        return true;
    }
}
