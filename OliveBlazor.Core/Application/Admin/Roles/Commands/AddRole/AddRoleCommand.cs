using MediatR;
using OliveBlazor.Core.Application.Common.Interfaces;
using OliveBlazor.Core.Application.Services.IdentityManagement;

namespace OliveBlazor.Core.Application.Admin.Roles.Commands.AddRole;

public class AddRoleCommand: IRequest<RoleDto>, IAnnounceCompletion
{
    public string Name {
        get;
        set;
    }
}

public class AddRoleCommandHandler : IRequestHandler<AddRoleCommand, RoleDto>
{
    private readonly IIdentityService _identityService;

    public AddRoleCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<RoleDto> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        var roleDto = new RoleDto(request.Name);
        await _identityService.AddRole(roleDto, cancellationToken);

        return roleDto;
    }
}