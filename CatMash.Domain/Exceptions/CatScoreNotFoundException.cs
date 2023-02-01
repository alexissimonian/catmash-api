namespace CatMash.Domain.Exceptions;

public class CatScoreNotFoundException : Exception
{
    public CatScoreNotFoundException()
    {
    }

    public CatScoreNotFoundException(string message)
        : base(message)
    {
    }
}