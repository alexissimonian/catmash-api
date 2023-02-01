using CatMash.Domain.Exceptions;
using CatMash.Domain.Models;

namespace CatMash.Domain.Interfaces;

public interface ICatMashRepository
{
    public Task InitDataContext();
    public Task<List<Cat>> GetAllCatsAsync();
    public Task<Dictionary<Cat, int>> GetAllCatsScoreAsync();
    public Task<ErrorResponseType> SaveCatScoresAsync(Dictionary<string, int> catScores);
}