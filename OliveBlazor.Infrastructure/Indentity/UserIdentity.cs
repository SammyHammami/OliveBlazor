using Microsoft.AspNetCore.Identity;
using OliveBlazor.Core.Domain.Accounts;

namespace OliveBlazor.Infrastructure.Indentity;

public class UserIdentity : IdentityUser
{
    public OliveUser OliveUser { get; set; }
    
}