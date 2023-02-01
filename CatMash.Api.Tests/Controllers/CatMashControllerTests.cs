using System.Net.Http.Headers;
using System.Net.Http.Json;
using CatMash.Api.Models;
using CatMash.Domain.Interfaces;
using CatMash.Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace CatMash.Api.Tests.Controllers;

public class CatMashControllerShould : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Mock<ICatMashService> _service;
    private readonly HttpClient _client;

    public CatMashControllerShould(WebApplicationFactory<Program> factory)
    {
        _service = new Mock<ICatMashService>();
        _client = factory.WithWebHostBuilder(b => b.ConfigureTestServices(s =>
        {
            var descriptor =
                new ServiceDescriptor(typeof(ICatMashService), _service.Object);
            s.Replace(descriptor);
        })).CreateClient();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    [Fact]
    public async Task GetTwoDifferentCatsAsync()
    {
        // Arrange
        var expectedRandomCats = new List<Cat>{ Cat.Create("1", "test/test1"), Cat.Create("2", "test/test2")};
        var expectedCatApiList = new List<CatApi>{ new("1", "test/test1"), new("2", "test/test2")};
        _service.Setup(s => s.GetTwoRandomCatsAsync()).ReturnsAsync(expectedRandomCats);
        
        // Act
        var response = await _client.GetAsync("api/catmash");
        var result = await response.Content.ReadFromJsonAsync<List<CatApi>>();
        
        // Assert
        response.EnsureSuccessStatusCode();
        _service.Verify(s => s.GetTwoRandomCatsAsync(), Times.Exactly(1));
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Equal(expectedCatApiList, result);
    }
    
    [Fact]
    public async Task GetAllCatsScoreAsync()
    {
        // Arrange
        var expectedRandomCats = new Dictionary<Cat, int>()
        {
            {Cat.Create("1", "test/test1"), 2},
            {Cat.Create("2", "test/test2"), 6},
            {Cat.Create("3", "test/test3"), 9},
            {Cat.Create("4", "test/test4"), 0}
        };
        var expectedCatScoreApiList = new List<CatScoreApi>()
        {
            {new(new CatApi("1", "test/test1"), 2)},
            {new(new CatApi("2", "test/test2"), 6)},
            {new(new CatApi("3", "test/test3"), 9)},
            {new(new CatApi("4", "test/test4"), 0)}
        };
        _service.Setup(s => s.GetAllCatsScoreAsync()).ReturnsAsync(expectedRandomCats);
        
        // Act
        var response = await _client.GetAsync("api/catmash/worldwide-scores");
        var result = await response.Content.ReadFromJsonAsync<List<CatScoreApi>>();
        
        // Assert
        response.EnsureSuccessStatusCode();
        _service.Verify(s => s.GetAllCatsScoreAsync(), Times.Exactly(1));
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        Assert.Equal(expectedCatScoreApiList, result);
    }
    
    [Fact]
    public async Task SaveCatScoresAsync()
    {
        // Arrange
        var request = new Dictionary<string, int> { { "1", 2 }, {"2", 1} };
        _service.Setup(s => s.SaveCatScoresAsync(request));
        
        // Act
        var response = await _client.PutAsJsonAsync("api/catmash",
            new CatScoreRequest(request));

        // Assert
        response.EnsureSuccessStatusCode();
        _service.Verify(s => s.SaveCatScoresAsync(request), Times.Exactly(1));
    }
}