namespace Domain.Livro;

public class LivroEntity
{
    public uint Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Subtitulo { get; set; }

    public string? Isbn { get; set; }

    public uint? EditoraId { get; set; }

    public DateTime? DataPublicacao { get; set; }

    public string? Resumo { get; set; }

    public byte[]? FotoCapa { get; set; }
}