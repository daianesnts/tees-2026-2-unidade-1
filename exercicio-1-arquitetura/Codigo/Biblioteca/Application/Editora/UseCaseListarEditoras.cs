using Domain.Editora;

namespace Application.Editora;

public class UseCaseListarEditoras
{
    private readonly IEditoraRepository _editoraRepository;

    public UseCaseListarEditoras(IEditoraRepository editoraRepository)
    {
        _editoraRepository = editoraRepository;
    }

    public IEnumerable<EditoraDTO> ExecuteGetAll()
    {
        return _editoraRepository.GetAll().Select(ToDTO);
    }

    public EditoraDTO? ExecuteGetById(uint id)
    {
        var editora = _editoraRepository.GetById(id);

        if (editora == null)
        {
            return null;
        }

        return ToDTO(editora);
    }

    private static EditoraDTO ToDTO(EditoraEntity editora)
    {
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
