namespace Domain.ItemAcervo;

public interface IItemAcervoRepository
{
    void Create(ItemAcervoEntity item);

    void Update(ItemAcervoEntity item);

    void Delete(uint id);

    ItemAcervoEntity? GetById(uint id);

    IEnumerable<ItemAcervoEntity> GetAll();
}