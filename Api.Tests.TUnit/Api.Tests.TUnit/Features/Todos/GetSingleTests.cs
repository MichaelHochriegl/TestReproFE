using System.Net;
using Api.Features.Todos;
using Api.Tests.TUnit.Plumbing;
using FastEndpoints;

namespace Api.Tests.TUnit.Features.Todos;

public class GetSingleTests : TestBase
{
    [Test]
    public async Task Should_Get_Single_Todo()
    {
        // Arrange
        var client = Factory.CreateClient();
        var todo = new CreateRequest("Test", "Description");
        var (createResponse, createResult) = await client.POSTAsync<CreateEndpoint, CreateRequest, CreateResponse>(todo);
        createResponse.EnsureSuccessStatusCode();
        var request = new GetSingleRequest(createResult.Id);
        
        var expected = new GetSingleResponse(createResult.Id, todo.Name, todo.Description);
        
        // Act
        var (response, result) = await client.GETAsync<GetSingle, GetSingleRequest, GetSingleResponse>(request);

        // Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
        await Assert.That(result).IsEquivalentTo(expected);
    }
}