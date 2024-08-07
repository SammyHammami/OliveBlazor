using Microsoft.AspNetCore.Identity;
using OliveBlazor.Core.Domain.Accounts;

namespace OliveBlazor.Infrastructure.Indentity;

public class RoleIdentity : IdentityRole
{
    // Navigation property
    public OliveRole OliveRole { get; set; }
}
