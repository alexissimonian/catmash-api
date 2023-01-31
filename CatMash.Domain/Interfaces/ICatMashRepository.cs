using CatMash.Domain.Models;

namespace CatMash.Domain.Interfaces;

public interface ICatMashRepository
{
    public Task<List<Cat>> GetAllCatsAsync();
    public Task<Dictionary<Cat, int>> GetAllCatsScoreAsync();
    public Task SaveCatScoresAsync(Dictionary<Cat, int> catScores);
}