namespace CatMash.Domain.Models;

public class Cat
{
    public string Id { get; init; }
    public string Url { get; init; }

    public static Cat Create(string id, string url)
    {
        return new Cat {Id = id, Url = url};
    }
}