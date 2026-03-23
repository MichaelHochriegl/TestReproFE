using Data;
using Data.Entities;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Todos;

public record CreateRequest(string Name, string? Description);
public record CreateResponse(Guid Id);

public class CreateEndpoint(AppDbContext dbContext) : Endpoint<CreateRequest, CreateResponse>
{
    public override void Configure()
    {
        Post("/todos");
    }

    public override async Task HandleAsync(CreateRequest req, CancellationToken ct)
    {
        var existing = await dbContext.Todos.AnyAsync(t => t.Name == req.Name, cancellationToken: ct);

        if (existing)
        {
            ThrowError(x => x.Name, "Todo with this name already exists");
        }
        
        dbContext.Todos.Add(new Todo
        {
            Name = req.Name,
            Description = req.Description
        });
        
        await dbContext.SaveChangesAsync(ct);

        await Send.CreatedAtAsync<GetSingleEndpoint>(cancellation: ct);
    }
}