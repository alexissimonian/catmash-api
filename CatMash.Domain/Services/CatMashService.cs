using CatMash.Domain.Interfaces;
using CatMash.Domain.Models;

namespace CatMash.Domain.Services;

public class CatMashService: ICatMashService
{
    private readonly ICatMashRepository _repository;
    private List<Cat> _allCats = new List<Cat>();

    public CatMashService(ICatMashRepository repository)
    {   
        _repository = repository;
    }

    public async Task<IEnumerable<Cat>> GetTwoRandomCatsAsync()
    {
        if (!_allCats.Any())
        {
            _allCats = await _repository.GetAllCatsAsync();   
        }

        return GetTwoDifferentRandomCats();
    }

    public async Task<Dictionary<Cat, int>> GetAllCatsScoreAsync()
    {
        return await _repository.GetAllCatsScoreAsync();
    }

    public async Task SaveCatScoresAsync(Dictionary<Cat, int> catScores)
    {
        await _repository.SaveCatScoresAsync(catScores);
    }

    private IEnumerable<Cat> GetTwoDifferentRandomCats()
    {
        var random = new Random();
        var firstRandomIndex = random.Next(_allCats.Count);
        var secondRandomIndex = random.Next(_allCats.Count);
        while (secondRandomIndex == firstRandomIndex)
        {
            secondRandomIndex = random.Next(_allCats.Count);
        }

        return new Cat[] { _allCats[firstRandomIndex], _allCats[secondRandomIndex] };
    }
}