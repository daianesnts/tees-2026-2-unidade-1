using Domain.Autor;

namespace Application.Autor;

public class CreateAutorUseCase
{
    private readonly IAutorRepository _autorRepository;

    public CreateAutorUseCase(IAutorRepository autorRepository)
    {
        _autorRepository = autorRepository;
    }

    public uint Execute(AutorEntity autor)
    {
        ValidateAutor(autor);

        _autorRepository.Create(autor);

        return autor.Id;
    }

    private static void ValidateAutor(AutorEntity autor)
    {
        if (autor.DataNascimento.Year < 1000)
        {
            throw new Exception(
                "O ano de nascimento do autor deve ser maior do que 1000."
            );
        }
    }
}