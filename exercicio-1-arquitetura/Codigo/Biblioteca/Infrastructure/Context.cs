using Domain.Autor;
using Domain.Editora;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options)
        : base(options)
    {
    }

    public DbSet<AutorEntity> Autores { get; set; }

    public DbSet<EditoraEntity> Editoras { get; set; }
}