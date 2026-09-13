namespace Application.ItemAcervo;

public class ItemAcervoDTO
{
    public uint Id { get; set; }

    public uint IdLivro { get; set; }

    public string IdSituacaoItemAcervo { get; set; } = string.Empty;

    public uint? IdDoacao { get; set; }
    
    public DateTime DataAquisicao { get; set; }
}