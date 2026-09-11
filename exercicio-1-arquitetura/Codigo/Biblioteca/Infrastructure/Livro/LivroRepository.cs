using Domain.Livro;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class LivroRepository : ILivroRepository
{
    private readonly Context context;

    public LivroRepository(Context context)
    {
        this.context = context;
    }

    public uint Add(LivroEntity livro)
    {
        context.Set<LivroEntity>().Add(livro);
        context.SaveChanges();
        return livro.Id;
    }

    public void Update(LivroEntity livro)
    {
        context.Set<LivroEntity>().Update(livro);
        context.SaveChanges();
    }

    public void Delete(uint id)
    {
        var livro = context.Set<LivroEntity>().Find(id);
        if (livro != null)
        {
            context.Set<LivroEntity>().Remove(livro);
            context.SaveChanges();
        }
    }

    public LivroEntity? GetById(uint id)
    {
        return context.Set<LivroEntity>().Find(id);
    }

    public IEnumerable<LivroEntity> GetAll()
    {
        return context.Set<LivroEntity>().AsNoTracking().ToList();
    }

    public IEnumerable<LivroEntity> GetByTitulo(string titulo)
    {
        return context.Set<LivroEntity>()
            .AsNoTracking()
            .Where(l => l.Titulo.StartsWith(titulo))
            .OrderBy(l => l.Titulo)
            .ToList();
    }
}