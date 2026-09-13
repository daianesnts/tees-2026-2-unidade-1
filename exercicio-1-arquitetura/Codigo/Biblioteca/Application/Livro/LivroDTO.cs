namespace Application.Livro.DTOs;

public class LivroDTO
{
    public uint Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Subtitulo { get; set; }
    public string? Isbn { get; set; } = string.Empty;
    public uint? EditoraId { get; set; }
    public DateTime? DataPublicacao { get; set; }
    public string? Resumo { get; set; }
    public byte[]? FotoCapa { get; set; }
}