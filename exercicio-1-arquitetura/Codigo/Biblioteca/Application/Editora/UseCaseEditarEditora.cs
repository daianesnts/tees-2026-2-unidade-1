using Domain.Editora;

namespace Application.Editora;

public class UseCaseEditarEditora
{
    private readonly IEditoraRepository _editoraRepository;

    public UseCaseEditarEditora(IEditoraRepository editoraRepository)
    {
        _editoraRepository = editoraRepository;
    }

    public bool Execute(AtualizarEditoraDTO dto)
    {
        var editoraExistente = _editoraRepository.GetById(dto.Id);

        if (editoraExistente == null)
        {
            return false;
        }

        var editora = new EditoraEntity
        {
            Id = dto.Id,
            Nome = dto.Nome,
            Rua = dto.Rua,
            Bairro = dto.Bairro,
            Numero = dto.Numero,
            Cep = dto.Cep,
            Cidade = dto.Cidade,
            Estado = dto.Estado
        };

        _editoraRepository.Update(editora);

        return true;
    }
}
