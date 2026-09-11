using Domain.Autor;
using Domain.ItemAcervo;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options)
        : base(options)
    {
    }

    public DbSet<AutorEntity> Autores { get; set; }
    public DbSet<ItemAcervoEntity> ItemAcervo { get; set; }
}