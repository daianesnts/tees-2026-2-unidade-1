namespace Domain.Comum;

public class PagedResult<T>
{
    public IEnumerable<T> Data { get; set; } = [];

    public int TotalRecords { get; set; }

    public int TotalRecordsFiltered { get; set; }
}