using Domain.ItemAcervo;

namespace Application.ItemAcervo;

public class GetAllItemAcervo
{
    private readonly IItemAcervoRepository _itemAcervoRepository;

    public UpdateItemAcervoCase(IItemAcervoRepository itemAcervoRepository)
    {
        _itemAcervoRepository = itemAcervoRepository;
    }

    public bool Execute(ItemAcervoEntity id)
    {
        var itemExistente = _itemAcervoRepository.GetById(item.id);

        if(itemExistente == null)
        {
            return false;
        }

        _itemAcervoRepository.Update(item);
        return true;        
    }
}
