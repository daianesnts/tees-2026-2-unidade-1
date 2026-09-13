using Domain.Autor;

namespace Application.Autor;

public class UseCaseObterAutorPorId : IObterAutorPorId
{
    private readonly IAutorRepository _autorRepository;

    public UseCaseObterAutorPorId(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }

    public AutorDTO? Execute(uint id)
    {
        var entity = _autorRepository.GetById(id);
        if (entity == null)
            return null;
        return new AutorDTO
        {
            Id = entity.Id,
            Nome = entity.Nome,
            DataNascimento = entity.DataNascimento
        };
    }
}