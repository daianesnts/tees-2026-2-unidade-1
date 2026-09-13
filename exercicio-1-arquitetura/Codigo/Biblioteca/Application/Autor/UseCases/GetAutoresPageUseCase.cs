using Domain.Autor;
using Domain.Comum;

namespace Application.Autor;

public class GetAutoresPageUseCase : IGetAutoresPage
{
    private readonly IAutorRepository _autorRepository;

    public GetAutoresPageUseCase(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }

    public PagedResult<AutorEntity> Execute(PageRequest request)
    {
        return _autorRepository.GetPage(request);
    }
}