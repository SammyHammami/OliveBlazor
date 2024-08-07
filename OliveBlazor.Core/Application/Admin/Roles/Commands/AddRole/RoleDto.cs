using OliveBlazor.Core.Domain.Accounts;
using OliveBlazor.Core.Domain.Security;

namespace OliveBlazor.Core.Application.Admin.Roles.Commands.AddRole
{
    public class RoleDto:OliveRole
    {

       
        public RoleDto(string name):base()
        {
            
            Name = name;
           
        }
        public RoleDto(string id, string name)
        {
            Id = new Guid(id);
            Name = name;
        }
        public RoleDto(Guid id, string name, string rIdentityRoleId, Permissions rPermissions)
        {
            Id = id;
            Name = name;
            IdentityRoleId = rIdentityRoleId;
            Permissions = rPermissions;
        }
    }
}