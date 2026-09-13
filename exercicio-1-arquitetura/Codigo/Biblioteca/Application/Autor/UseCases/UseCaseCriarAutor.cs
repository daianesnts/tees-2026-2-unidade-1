using Domain.Autor;

namespace Application.Autor;

public class UseCaseCriarAutor : ICriarAutor
{
    private readonly IAutorRepository _autorRepository;

    public UseCaseCriarAutor(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }

    public uint Execute(AutorDTO dto)
    {
        ValidateAutor(dto);
        var autor = new AutorEntity
        {
            Nome = dto.Nome,
            DataNascimento = dto.DataNascimento
        };
        _autorRepository.Create(autor);
        return autor.Id;
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