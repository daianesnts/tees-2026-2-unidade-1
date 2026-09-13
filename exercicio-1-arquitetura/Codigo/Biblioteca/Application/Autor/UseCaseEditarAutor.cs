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

    public bool Execute(AutorEntity autor)
    {
        var autorExistente =
            _autorRepository.GetById(autor.Id);

        if (autorExistente == null)
        {
            return false;
        }

        ValidateAutor(autor);

        _autorRepository.Update(autor);

        return true;
    }

    private static void ValidateAutor(
        AutorEntity autor
    )
    {
        if (autor.DataNascimento.Year < 1000)
        {
            throw new Exception(
                "O ano de nascimento do autor deve ser maior do que 1000."
            );
        }
    }
}