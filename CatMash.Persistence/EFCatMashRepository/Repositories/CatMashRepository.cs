using CatMash.Domain.Exceptions;
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
        using (StreamReader r = new StreamReader(@"InitSample\SampleData.Json"))
        {
            string json = await r.ReadToEndAsync();
            catScore = JsonConvert.DeserializeObject<List<CatScore>>(json);
        }

        await _context.AddRangeAsync(catScore);
        await _context.SaveChangesAsync();
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

    public async Task<ErrorResponseType> SaveCatScoresAsync(Dictionary<string, int> dictionaryCatScore)
    {
        foreach (var kvp in dictionaryCatScore)
        {
            var scoreToEdit = await _context.CatScores.FindAsync(kvp.Key);
            if (scoreToEdit is null)
            {
                return ErrorResponseType.NotFound;
            }
            
            scoreToEdit.Score += kvp.Value;
        }
        await _context.SaveChangesAsync();
        return ErrorResponseType.Ok;
    }

    private Cat ConvertCatScoreToCat(CatScore catScore)
    {
        return Cat.Create(catScore.Id, catScore.Url);
    }
    
    private Dictionary<Cat, int> ConvertCatScoreToDictionary(IEnumerable<CatScore> catScores)
    {
        return catScores.ToDictionary(ConvertCatScoreToCat, catScore => catScore.Score);
    }
}