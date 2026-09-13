using Domain.Autor;

namespace Application.Autor;

public class UseCaseEditarAutor
{
    private readonly IAutorRepository _autorRepository;

    public UseCaseEditarAutor(
        IAutorRepository autorRepository
    )
    {
        _autorRepository = autorRepository;
    }

    public bool Execute(AutorDTO dto)
    {
        var autorExistente = _autorRepository.GetById(dto.Id);
        if (autorExistente == null)
        {
            return false;
        }
        ValidateAutor(dto);
        autorExistente.Nome = dto.Nome;
        autorExistente.DataNascimento = dto.DataNascimento;
        _autorRepository.Update(autorExistente);
        return true;
    }
    
    private static void ValidateAutor(AutorDTO dto)
    {
        if (dto.DataNascimento.Year < 1000)
        {
            throw new Exception(
                "O ano de nascimento do autor deve ser maior do que 1000."
            );
        }
    }
}