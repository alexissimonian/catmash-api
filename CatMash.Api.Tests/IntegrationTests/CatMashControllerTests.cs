using System.Net.Http.Headers;
using CatMash.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace CatMash.Api.Tests.IntegrationTests;

public class CatMashControllerShould
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
}