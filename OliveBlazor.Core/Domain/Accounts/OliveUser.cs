namespace OliveBlazor.Core.Domain.Accounts;

public class OliveUser :BaseEntity
{
    public OliveUser()
    {
        Id = Guid.NewGuid();
    }

    
    public string FirstName { get; set; }
    public string LastName { get; set; }

    //public string UserId { get; set; }
    public string UserIdentityId { get; set; }

    public DateTime DateOfBirth { get; set; }



}