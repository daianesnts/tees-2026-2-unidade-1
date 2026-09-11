using Domain.ItemAcervo;

namespace Application.ItemAcervo;

public class CreateItemAcervoUseCase
{
    private readonly IItemAcervoRepository _itemAcervoRepository;

    public CreateItemAcervoUseCase(IItemAcervoRepository itemAcervoRepository)
    {
        _itemAcervoRepository = itemAcervoRepository;
    }

    public void Execute(uint item)
    {
        var item = _itemAcervoRepository.GetById(id);
        if (item == null)
        {
            throw new Exception("Item não encontrado");
        }
        
        _itemAcervoRepository.Delete(id)
    }
}

