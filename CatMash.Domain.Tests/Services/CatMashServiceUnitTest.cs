using CatMash.Domain.Interfaces;
using CatMash.Domain.Models;
using CatMash.Domain.Services;
using Moq;

namespace CatMash.Domain.Tests.Services;

public class CatMashServiceShould
{
    private readonly Mock<ICatMashRepository> _repository;
    private readonly ICatMashService _sut;

    public CatMashServiceShould()
    {
        _repository = new Mock<ICatMashRepository>();
        _sut = new CatMashService(_repository.Object);
    }

    [Fact]
    public async Task GetTwoDifferentCats()
    {
        // Arrange
        var allCats = new List<Cat>
        {
            Cat.Create("1", "test/test1"), Cat.Create("2", "test/test2"), Cat.Create("3", "test/test3"),
            Cat.Create("4", "test/test4")
        };
        _repository.Setup(r => r.GetAllCatsAsync()).ReturnsAsync(allCats);

        // Act
        var result = await _sut.GetTwoRandomCatsAsync();

        // Assert
        Assert.IsAssignableFrom<IEnumerable<Cat>>(result);
        Assert.Equal(2, result.Count());
        Assert.NotEqual(result.FirstOrDefault(), result.LastOrDefault());
        
        _repository.Verify(r => r.GetAllCatsAsync(), Times.Exactly(1));
        _repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task GetAllCatsWithTheirScore()
    {
        // Arrange
        Dictionary<Cat, int> expectedDictionaryCatScore = new()
        {
            { Cat.Create("1", "test/test1"), 2 },
            { Cat.Create("2", "test/test2"), 8 },
            { Cat.Create("3", "test/test3"), 61 },
            { Cat.Create("4", "test/test4"), 124 }
        };
        _repository.Setup(r => r.GetAllCatsScoreAsync()).ReturnsAsync(expectedDictionaryCatScore);

        // Act
        var result = await _sut.GetAllCatsScoreAsync();

        // Assert
        Assert.Equal(4, result.Count);
        Assert.Equal(expectedDictionaryCatScore, result);
        
        _repository.Verify(r => r.GetAllCatsScoreAsync(), Times.Exactly(1));
        _repository.VerifyNoOtherCalls();
    }
    
    [Fact]
    public async Task CallSaveCatScores()
    {
        // Arrange
        Dictionary<string, int> dictionaryIdScore = new()
        {
            { "1", 2 },
            { "2", 8 },
            { "3", 61 },
            { "4", 124 }
        };
        _repository.Setup(r => r.SaveCatScoresAsync(dictionaryIdScore));

        // Act
        await _sut.SaveCatScoresAsync(dictionaryIdScore);

        // Assert
        _repository.Verify(r => r.SaveCatScoresAsync(dictionaryIdScore), Times.Exactly(1));
        _repository.VerifyNoOtherCalls();
    }
}