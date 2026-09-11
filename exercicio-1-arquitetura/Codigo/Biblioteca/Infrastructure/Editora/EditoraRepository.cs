using Domain.Editora;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class EditoraRepository : IEditoraRepository
{
    private readonly Context context;

    public EditoraRepository(Context context)
    {
        this.context = context;
    }

    public uint Add(EditoraEntity editora)
    {
        context.Editoras.Add(editora);
        context.SaveChanges();
        return editora.Id;
    }

    public void Update(EditoraEntity editora)
    {
        context.Editoras.Update(editora);
        context.SaveChanges();
    }

    public void Delete(uint id)
    {
        var editora = context.Editoras.Find(id);
        if (editora != null)
        {
            context.Editoras.Remove(editora);
            context.SaveChanges();
        }
    }

    public EditoraEntity? GetById(uint id)
    {
        return context.Editoras.Find(id);
    }

    public IEnumerable<EditoraEntity> GetAll()
    {
        return context.Editoras.AsNoTracking().ToList();
    }

    public IEnumerable<EditoraEntity> GetByNome(string nome)
    {
        return context.Editoras
            .AsNoTracking()
            .Where(e => e.Nome.StartsWith(nome))
            .OrderBy(e => e.Nome)
            .ToList();
    }
}
