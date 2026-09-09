namespace Domain.Comum;

public class PageRequest
{
    public int Start { get; set; }

    public int Length { get; set; }

    public string? Search { get; set; }
}