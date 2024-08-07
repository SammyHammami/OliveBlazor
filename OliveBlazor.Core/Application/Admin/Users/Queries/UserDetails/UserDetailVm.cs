namespace OliveBlazor.Core.Application.Admin.Users.Queries.UserDetails;


public class UserDetailsVm
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public List<string> UserRoles { get; set; }=new List<string>();
        public List<string> AllRoles { get; set; }
    }



public class UserRoleVm
{
    public string RoleName { get; set; }
    public bool IsAssigned { get; set; }
}
