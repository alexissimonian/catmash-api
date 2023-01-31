using System.Reflection;
using CatMash.Domain.Interfaces;
using CatMash.Domain.Models;
using CatMash.Persistence.EFCatMashRepository.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CatMash.Persistence.EFCatMashRepository.Repositories;

public class CatMashRepository: ICatMashRepository
{
    private readonly CatContext _context;

    public CatMashRepository(CatContext context)
    {
        _context = context;
    }
    
    public async Task InitDataContext()
    {
        List<CatScore> catScore = new();
        using (StreamReader r = new StreamReader(@"Data\SampleData.Json"))
        {
            string json = await r.ReadToEndAsync();
            catScore = JsonConvert.DeserializeObject<List<CatScore>>(json);
        }

        await _context.AddRangeAsync(catScore);
    }
    
    public async Task<List<Cat>> GetAllCatsAsync()
    {
        var cats = await _context.CatScores.ToListAsync();
        return cats.Select(ConvertCatScoreToCat).ToList();
    }

    public async Task<Dictionary<Cat, int>> GetAllCatsScoreAsync()
    {
        var cats = await _context.CatScores.ToListAsync();
        return ConvertCatScoreToDictionary(cats);
    }

    public async Task SaveCatScoresAsync(Dictionary<Cat, int> dictionaryCatScore)
    {
        IEnumerable<CatScore> catScores = ConvertDictionaryToCatScore(dictionaryCatScore);
        await _context.CatScores.AddRangeAsync(catScores);
    }

    private Cat ConvertCatScoreToCat(CatScore catScore)
    {
        return Cat.Create(catScore.Id, catScore.Url);
    }
    
    private Dictionary<Cat, int> ConvertCatScoreToDictionary(IEnumerable<CatScore> catScores)
    {
        return catScores.ToDictionary(ConvertCatScoreToCat, catScore => catScore.Score);
    }

    private IEnumerable<CatScore> ConvertDictionaryToCatScore(Dictionary<Cat, int> dictionaryCatScore)
    {
        return dictionaryCatScore.Select(d => new CatScore() { Id = d.Key.Id, Url = d.Key.Url, Score = d.Value });
    }
}