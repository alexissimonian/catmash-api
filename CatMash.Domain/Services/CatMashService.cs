using CatMash.Domain.Interfaces;
using CatMash.Domain.Models;

namespace CatMash.Domain.Services;

public class CatMashService: ICatMashService
{
    public async Task<IEnumerable<Cat>> GetTwoRandomCatsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Dictionary<Cat, int>> GetAllCatsScoreAsync()
    {
        throw new NotImplementedException();
    }

    public async Task SaveCatScoresAsync()
    {
        throw new NotImplementedException();
    }
}