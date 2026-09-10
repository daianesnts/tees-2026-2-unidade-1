using Domain.Editora;

namespace Application.Editora;

public class UseCaseExcluirEditora
{
    private readonly IEditoraRepository _editoraRepository;

    public UseCaseExcluirEditora(IEditoraRepository editoraRepository)
    {
        _editoraRepository = editoraRepository;
    }

    public void Execute(uint id)
    {
        var editora = _editoraRepository.GetById(id);

        if (editora == null)
        {
            throw new Exception("Editora não encontrada.");
        }

        _editoraRepository.Delete(id);
    }
}
