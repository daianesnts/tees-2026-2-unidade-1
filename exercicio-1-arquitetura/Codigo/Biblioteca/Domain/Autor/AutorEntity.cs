namespace Domain.Autor;

public class AutorEntity
{
    public uint Id { get; set; }

    public string Nome { get; set; } = null!;

    public DateTime DataNascimento { get; set; }

}