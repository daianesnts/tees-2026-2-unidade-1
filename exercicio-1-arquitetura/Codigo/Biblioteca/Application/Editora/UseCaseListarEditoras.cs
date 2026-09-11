using Domain.Editora;

namespace Application.Editora;

public class UseCaseListarEditoras
{
    private readonly IEditoraRepository _editoraRepository;

    public UseCaseListarEditoras(IEditoraRepository editoraRepository)
    {
        _editoraRepository = editoraRepository;
    }

    public IEnumerable<EditoraDTO> Execute()
    {
        return _editoraRepository.GetAll().Select(editora => new EditoraDTO
        {
            Id = editora.Id,
            Nome = editora.Nome,
            Rua = editora.Rua,
            Bairro = editora.Bairro,
            Numero = editora.Numero,
            Cep = editora.Cep,
            Cidade = editora.Cidade,
            Estado = editora.Estado
        });
    }
}
