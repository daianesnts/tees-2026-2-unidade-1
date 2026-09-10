using Domain.Editora;

namespace Application.Editora;

public class UseCaseCriarEditora
{
    private readonly IEditoraRepository _editoraRepository;

    public UseCaseCriarEditora(IEditoraRepository editoraRepository)
    {
        _editoraRepository = editoraRepository;
    }

    public uint Execute(CriarEditoraDTO dto)
    {
        var editora = new EditoraEntity
        {
            Nome = dto.Nome,
            Rua = dto.Rua,
            Bairro = dto.Bairro,
            Numero = dto.Numero,
            Cep = dto.Cep,
            Cidade = dto.Cidade,
            Estado = dto.Estado
        };

        return _editoraRepository.Add(editora);
    }
}
