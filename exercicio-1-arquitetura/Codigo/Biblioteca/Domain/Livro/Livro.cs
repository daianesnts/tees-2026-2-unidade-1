namespace Domain.Livro;

public class LivroEntity
{
    public uint Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Subtitulo { get; set; }

    public string? Isbn { get; set; }

    public uint? EditoraId { get; set; }
}