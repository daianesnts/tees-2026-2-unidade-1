using Domain.Comum;

namespace Domain.Autor;

public interface IAutorRepository
{
    void Create(AutorEntity autor);

    void Update(AutorEntity autor);

    void Delete(uint id);

    AutorEntity? GetById(uint id);

    IEnumerable<AutorEntity> GetAll();

    PagedResult<AutorEntity> GetPage(PageRequest request);
}