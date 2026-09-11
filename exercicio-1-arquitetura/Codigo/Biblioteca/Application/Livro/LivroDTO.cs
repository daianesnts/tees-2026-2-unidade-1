namespace Application.Livro.DTOs;

public class LivroDTO
{
    public uint Id {get; set;}
    public string Isbn {get; set;} = string.Empty;
    public string Nome {get; set;} = string.Empty;
    public string? NomeEditora {get; set;}
}