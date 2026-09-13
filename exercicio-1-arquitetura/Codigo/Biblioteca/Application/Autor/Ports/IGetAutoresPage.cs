using Domain.Autor;
using Domain.Comum;

namespace Application.Autor;

public interface IGetAutoresPage
{
    PagedResult<AutorEntity> Execute(PageRequest request);
}