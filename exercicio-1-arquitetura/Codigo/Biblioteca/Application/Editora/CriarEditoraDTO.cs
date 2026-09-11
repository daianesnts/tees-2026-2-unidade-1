namespace Application.Editora;

public class CriarEditoraDTO
{
    public string Nome { get; set; } = null!;

    public string? Rua { get; set; }

    public string? Bairro { get; set; }

    public string? Numero { get; set; }

    public string? Cep { get; set; }

    public string? Cidade { get; set; }

    public string? Estado { get; set; }
}
