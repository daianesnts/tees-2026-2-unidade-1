namespace Domain.Editora;

public class EditoraEntity
{
    public uint Id { get; set; }

    public string Nome { get; set; } = null!;

    public string? Rua { get; set; }

    public string? Bairro { get; set; }

    public string? Numero { get; set; }

    public string? Cep { get; set; }

    public string? Cidade { get; set; }

    public string? Estado { get; set; }

    //a entidade de Livro ainda não existe (depende da issue #6).
}
