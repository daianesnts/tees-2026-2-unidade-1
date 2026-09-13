using Domain.ItemAcervo;

namespace Application.ItemAcervo;

public class UseCaseEditarItemAcervo
{
    private readonly IItemAcervoRepository _itemAcervoRepository;

    public UseCaseEditarItemAcervo(IItemAcervoRepository itemAcervoRepository)
    {
        _itemAcervoRepository = itemAcervoRepository;
    }

    public bool Execute(ItemAcervoDTO dto)
    {
        var itemExistente = _itemAcervoRepository.GetById(dto.Id);
        if (itemExistente == null)
            return false;
        itemExistente.IdLivro = dto.IdLivro;
        itemExistente.IdSituacaoItemAcervo = dto.IdSituacaoItemAcervo;
        itemExistente.IdDoacao = dto.IdDoacao;
        itemExistente.DataAquisicao = dto.DataAquisicao;
        _itemAcervoRepository.Update(itemExistente);
        return true;
    }
}
