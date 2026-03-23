using System.Net;
using Api.Features.Todos;
using Api.Tests.TUnit.Plumbing;
using FastEndpoints;
using System.Threading.Tasks;

namespace Api.Tests.TUnit.Features.Todos;

public class CreateTests : TestBase
{
    [Test]
    public async Task Should_Create_Todo()
    {
        // Arrange
        var client = Factory.CreateClient();
        var todo = new CreateRequest("Test", "Description");

        // Act
        var response = await client.POSTAsync<CreateEndpoint, CreateRequest>(todo);

        // Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.Created);
    }
    
    [Test]
    public async Task Should_Not_Create_Todo_InvalidTitle()
    {
        // Arrange
        var client = Factory.CreateClient();
        var todo = new CreateRequest("", "Description");

        // Act
        var response = await client.POSTAsync<CreateEndpoint, CreateRequest>(todo);

        // Assert
        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);
    }
}