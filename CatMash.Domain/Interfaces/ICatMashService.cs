using CatMash.Domain.Models;

namespace CatMash.Domain.Interfaces;

public interface ICatMashService
{
    public Task<IEnumerable<Cat>> GetTwoRandomCatsAsync();
    public Task<Dictionary<Cat, int>> GetAllCatsScoreAsync();
    public Task SaveCatScoresAsync(Dictionary<Cat, int> catScores);
}