namespace Domain.ItemAcervo;

public class ItemAcervoEntity
{
    public uint Id { get; set; }

    public uint IdLivro { get; set; }

    public string IdSituacaoItemAcervo { get; set; } = null!;

    public uint? IdDoacao { get; set; }

    public DateTime DataAquisicao { get; set; }

    public uint IdBiblioteca { get; set; }
}