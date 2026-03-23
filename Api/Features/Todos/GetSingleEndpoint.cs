using Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Todos;

public record GetSingleRequest(Guid Id);

public record GetSingleResponse(Guid Id, string Name, string? Description);

public class GetSingleEndpoint(AppDbContext dbContext) : Endpoint<GetSingleRequest, GetSingleResponse>
{
    public override void Configure()
    {
        Get("/todos/{id}");
    }

    public override async Task HandleAsync(GetSingleRequest req, CancellationToken ct)
    {
        var todo = await dbContext.Todos.FirstOrDefaultAsync(t => t.Id == req.Id, cancellationToken: ct);

        if (todo is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }
        
        await Send.OkAsync(new GetSingleResponse(todo.Id, todo.Name, todo.Description), ct);
    }
}