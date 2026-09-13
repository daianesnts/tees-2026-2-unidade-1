namespace Application.Livro.DTOs;

public class CriarLivroDTO
{
    public string Titulo {get; set;} = null!;
    public string? Subtitulo {get; set;}
    public uint? EditoraId {get; set;}
    public string Isbn {get; set;} = null!;
}