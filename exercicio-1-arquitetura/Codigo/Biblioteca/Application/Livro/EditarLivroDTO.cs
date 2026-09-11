namespace Application.Livro.DTOs;

public class EditarLivroDTO
{
    public uint Id {get; set;}
    public string Isbn {get; set;} = null!;
    public uint IdEditora {get; set;}
    public string Nome {get; set;} = null!;
    public DateTime? DataPublicacao {get; set;}
    public string? Resumo {get; set;}
    public byte[]? FotoCapa {get; set;}
}