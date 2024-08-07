using OliveBlazor.Core.Domain.Security;

namespace OliveBlazor.Core.Domain.Accounts;

public class OliveRole
{
    public OliveRole()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Permissions Permissions { get; set; }  // Using the previously defined Permissions enum
                                                  // ... any other properties you want to include

    // If you wish to maintain a direct many-to-many relationship with OliveUser:

    public string IdentityRoleId { get; set; }
   

   

}
