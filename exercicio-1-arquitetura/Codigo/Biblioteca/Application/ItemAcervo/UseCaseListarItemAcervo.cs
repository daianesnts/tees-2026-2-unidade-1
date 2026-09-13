using Domain.ItemAcervo;

namespace Application.ItemAcervo;

public class UseCaseListarItemAcervo
{
    private readonly IItemAcervoRepository _itemAcervoRepository;

    public UseCaseListarItemAcervo(IItemAcervoRepository itemAcervoRepository)
    {
        _itemAcervoRepository = itemAcervoRepository;
    }

    public IEnumerable<ItemAcervoDTO> Execute()
    {
        var itens = _itemAcervoRepository.GetAll();
        return itens.Select(item => new ItemAcervoDTO
        {
            Id = item.Id,
            IdLivro = item.IdLivro,
            IdSituacaoItemAcervo = item.IdSituacaoItemAcervo,
            IdDoacao = item.IdDoacao,
            DataAquisicao = item.DataAquisicao
        });
    }
}
