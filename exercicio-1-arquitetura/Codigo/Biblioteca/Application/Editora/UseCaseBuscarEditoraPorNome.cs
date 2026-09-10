using Domain.Editora;

namespace Application.Editora;

public class UseCaseBuscarEditoraPorNome
{
    private readonly IEditoraRepository _editoraRepository;

    public UseCaseBuscarEditoraPorNome(IEditoraRepository editoraRepository)
    {
        _editoraRepository = editoraRepository;
    }

    public IEnumerable<EditoraDTO> Execute(string nome)
    {
        return _editoraRepository.GetByNome(nome).Select(editora => new EditoraDTO
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
