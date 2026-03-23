using System.Net;
using Api.Features.Todos;
using Api.Tests.FE.Plumbing;
using AwesomeAssertions;
using FastEndpoints;
using Xunit;

namespace Api.Tests.FE.Features.Todos;

public class CreateTests(ApiFixture app) : IClassFixture<ApiFixture>
{
    
    [Fact]
    public async Task Creates_todo_and_returns_created()
    {

        var response = await app.Client.POSTAsync<CreateEndpoint, CreateRequest>(new CreateRequest("Test", "Description"));

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
    
    
}