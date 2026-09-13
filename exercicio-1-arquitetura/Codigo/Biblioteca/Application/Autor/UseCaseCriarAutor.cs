using Domain.Autor;

namespace Application.Autor;

public class UseCaseCriarAutor
{
    private readonly IAutorRepository _autorRepository;

    public UseCaseCriarAutor(IAutorRepository autorRepository)
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