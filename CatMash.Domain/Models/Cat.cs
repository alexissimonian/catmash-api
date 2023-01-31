namespace CatMash.Domain.Models;

public class Cat
{
    private Cat(string id, string url)
    {
        Id = id;
        Url = url;
    }
    
    public string Id { get; }
    public string Url { get; }

    public static Cat Create(string id, string url)
    {
        return new Cat(id, url);
    }
}