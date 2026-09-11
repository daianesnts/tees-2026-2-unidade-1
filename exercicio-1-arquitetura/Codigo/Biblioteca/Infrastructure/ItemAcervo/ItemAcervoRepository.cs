using Domain.ItemAcervo;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ItemAcervorepository : IItemAcervoRepository
{
    private readonly Context _context;

    public ItemAcervorepository(Context _context)
    {
        this._context = _context;
    }

    public void Create(ItemAcervoEntity item)
    {
        _context.ItemAcervo.Add(item);
        _context.SaveChanges();
    }

    public void Update(ItemAcervoEntity item)
    {
        _context.ItemAcervo.Update(item);
        _context.SaveChanges();
    }

    public void Delete(uint id)
    {
        ItemAcervoEntity? item = _context.ItemAcervo.Find(id);

        if (item != null)
        {
            _context.ItemAcervo.Remove(item);
            _context.SaveChanges();
        }
    }

    public ItemAcervoEntity? GetById(uint id)
    {
        return _context.ItemAcervo
            .AsNoTracking()
            .SingleOrDefault(item => item.Id == id);
    }

    public IEnumerable<ItemAcervoEntity> GetAll()
    {
        return _context.ItemAcervo
            .AsNoTracking()
            .ToList();
    }
}