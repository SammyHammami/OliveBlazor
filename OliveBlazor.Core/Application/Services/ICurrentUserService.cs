namespace OliveBlazor.Core.Application.Services;

public interface ICurrentUserService
{
    string UserId { get; }
    bool IsAuthenticated { get; }
    string UserName { get; }
    //SH maybe this is not the right service for it 
    //string CurrentPath { get;  }
}