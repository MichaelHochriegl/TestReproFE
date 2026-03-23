using Data;
using Data.Entities;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Todos;

public record CreateRequest(string Name, string? Description);
public record CreateResponse(Guid Id);

public class CreateEndpoint(AppDbContext dbContext) : Endpoint<CreateRequest>
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

        var entity = new Todo
        {
            Name = req.Name,
            Description = req.Description
        };
        dbContext.Todos.Add(entity);
        
        await dbContext.SaveChangesAsync(ct);

        await Send.CreatedAtAsync<GetSingle>(new GetSingleRequest(entity.Id), new CreateResponse(entity.Id), cancellation: ct);
    }
}

public class CreateValidator : Validator<CreateRequest>
{
    public CreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
    }
}