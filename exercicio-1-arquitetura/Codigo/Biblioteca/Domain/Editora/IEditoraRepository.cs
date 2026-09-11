namespace Domain.Editora;


public interface IEditoraRepository
{
    uint Add(EditoraEntity editora);
    void Update(EditoraEntity editora);
    void Delete(uint id);
    EditoraEntity? GetById(uint id);
    IEnumerable<EditoraEntity> GetAll();
    IEnumerable<EditoraEntity> GetByNome(string nome);
}
