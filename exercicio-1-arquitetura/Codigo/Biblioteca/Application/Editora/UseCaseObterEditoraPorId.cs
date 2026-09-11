using Domain.Editora;

namespace Application.Editora;

public class UseCaseObterEditoraPorId
{
    private readonly IEditoraRepository _editoraRepository;

    public UseCaseObterEditoraPorId(IEditoraRepository editoraRepository)
    {
        _editoraRepository = editoraRepository;
    }

    public EditoraDTO? Execute(uint id)
    {
        var editora = _editoraRepository.GetById(id);

        if (editora == null)
        {
            return null;
        }

        return new EditoraDTO
        {
            Id = editora.Id,
            Nome = editora.Nome,
            Rua = editora.Rua,
            Bairro = editora.Bairro,
            Numero = editora.Numero,
            Cep = editora.Cep,
            Cidade = editora.Cidade,
            Estado = editora.Estado
        };
    }
}
