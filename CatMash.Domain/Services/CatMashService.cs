using CatMash.Domain.Exceptions;
using CatMash.Domain.Interfaces;
using CatMash.Domain.Models;

namespace CatMash.Domain.Services;

public class CatMashService: ICatMashService
{
    private readonly ICatMashRepository _repository;

    public CatMashService(ICatMashRepository repository)
    {   
        _repository = repository;
    }

    public async Task<IEnumerable<Cat>> GetTwoRandomCatsAsync()
    {
        var allCats = await _repository.GetAllCatsAsync();
        return GetTwoDifferentRandomCats(allCats);
    }

    public async Task<Dictionary<Cat, int>> GetAllCatsScoreAsync()
    {
        return await _repository.GetAllCatsScoreAsync();
    }

    public async Task SaveCatScoresAsync(Dictionary<string, int> catScores)
    {
        var response = await _repository.SaveCatScoresAsync(catScores);
        if (response == ErrorResponseType.NotFound)
        {
            throw new CatScoreNotFoundException("One of the cat you submitted cannot be found");
        }
    }

    private IEnumerable<Cat> GetTwoDifferentRandomCats(List<Cat> allCats)
    {
        var random = new Random();
        var firstRandomIndex = random.Next(allCats.Count);
        var secondRandomIndex = random.Next(allCats.Count);
        while (secondRandomIndex == firstRandomIndex)
        {
            secondRandomIndex = random.Next(allCats.Count);
        }

        return new Cat[] { allCats[firstRandomIndex], allCats[secondRandomIndex] };
    }
}