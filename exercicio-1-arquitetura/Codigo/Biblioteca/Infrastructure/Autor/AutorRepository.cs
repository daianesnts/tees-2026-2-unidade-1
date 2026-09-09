using Domain.Autor;
using Domain.Comum;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AutorRepository : IAutorRepository
{
    private readonly Context _context;

    public AutorRepository(Context context)
    {
        _context = context;
    }

    public void Create(AutorEntity autor)
    {
        _context.Autores.Add(autor);
        _context.SaveChanges();
    }

    public void Update(AutorEntity autor)
    {
        _context.Autores.Update(autor);
        _context.SaveChanges();
    }

    public void Delete(uint id)
    {
        var autor = _context.Autores.Find(id);

        if (autor != null)
        {
            _context.Autores.Remove(autor);
            _context.SaveChanges();
        }
    }

    public AutorEntity? GetById(uint id)
    {
        return _context.Autores
            .AsNoTracking()
            .FirstOrDefault(autor => autor.Id == id);
    }

    public IEnumerable<AutorEntity> GetAll()
    {
        return _context.Autores
            .AsNoTracking()
            .ToList();
    }

    public PagedResult<AutorEntity> GetPage(PageRequest request)
    {
        var query = _context.Autores
            .AsNoTracking();

        var totalRecords = query.Count();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();

            query = query.Where(autor =>
                autor.Nome.ToLower().Contains(search)
            );
        }

        var totalRecordsFiltered = query.Count();

        var autores = query
            .Skip(request.Start)
            .Take(request.Length)
            .ToList();

        return new PagedResult<AutorEntity>
        {
            Data = autores,
            TotalRecords = totalRecords,
            TotalRecordsFiltered = totalRecordsFiltered
        };
    }
}