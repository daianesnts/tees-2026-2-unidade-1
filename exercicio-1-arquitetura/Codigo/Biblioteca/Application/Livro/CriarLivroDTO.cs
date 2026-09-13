namespace Application.Livro.DTOs;

public class CriarLivroDTO
{
    public string Titulo {get; set;} = null!;
    public string? Subtitulo {get; set;}
    public uint? EditoraId {get; set;}
    public string Isbn {get; set;} = null!;
    public DateTime? DataPublicacao { get; set; }
    public string? Resumo { get; set; }
    public byte[]? FotoCapa { get; set; }
}