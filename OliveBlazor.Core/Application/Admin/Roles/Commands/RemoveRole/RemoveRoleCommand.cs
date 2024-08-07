using MediatR;
using OliveBlazor.Core.Application.Common.Interfaces;
using OliveBlazor.Core.Application.Services.IdentityManagement;

namespace OliveBlazor.Core.Application.Admin.Roles.Commands.RemoveRole;

public class RemoveRoleCommand:IRequest , IAnnounceCompletion
{
    public Guid Id { get; set; }
}

public class RemoveRoleCommandHandler : IRequestHandler<RemoveRoleCommand>
{
    private readonly IIdentityService _identityService;

    public RemoveRoleCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _identityService.RemoveRole(request.Id, cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            // Handle or log the exception as needed
            Console.WriteLine($"Error removing role: {ex.Message}");
            throw; // Re-throw the exception if necessary
        }

    }
}