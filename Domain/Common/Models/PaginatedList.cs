namespace Domain.Common.Models;

public class PaginatedList<T>
{
    public List<T> Items { get; set; }
    public PageData PageData { set; get; }
    
    public PaginatedList(List<T> items, PageData pageData)
    {
        Items = items;
        PageData = pageData;
    }
}