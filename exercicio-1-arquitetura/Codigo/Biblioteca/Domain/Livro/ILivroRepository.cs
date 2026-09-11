namespace Domain.Livro;

public interface ILivroRepository
{
    uint Add(LivroEntity livro);
    void Update(LivroEntity livro);
    void Delete(uint id);
    LivroEntity? GetById(uint id);
    IEnumerable<LivroEntity> GetAll();
    IEnumerable<LivroEntity> GetByTitulo(string titulo);
}