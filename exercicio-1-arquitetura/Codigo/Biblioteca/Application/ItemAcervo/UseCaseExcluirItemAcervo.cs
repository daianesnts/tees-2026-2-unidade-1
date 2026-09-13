using Domain.ItemAcervo;

namespace Application.ItemAcervo;

public class UseCaseExcluirItemAcervo
{
    private readonly IItemAcervoRepository _itemAcervoRepository;

    public UseCaseExcluirItemAcervo(IItemAcervoRepository itemAcervoRepository)
    {
        _itemAcervoRepository = itemAcervoRepository;
    }

    public void Execute(uint id)
    {
        var item = _itemAcervoRepository.GetById(id);
        if (item == null)
        {
            throw new Exception("Item não encontrado");
        }
        
        _itemAcervoRepository.Delete(id);
    }
}

