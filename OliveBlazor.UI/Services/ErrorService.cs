using Microsoft.AspNetCore.Components;
using OliveBlazor.Core.Application.Services;
using OliveBlazor.Core.Domain.Common.Communication;

namespace OliveBlazor.UI.Services;

public class ErrorService:IErrorService
{
    private readonly NavigationManager _navigationManager;
    private readonly INotificationService _notificationService;

    public string ErrorMessage { get; set; }
    public ErrorService(NavigationManager navigationManager, INotificationService notificationService)
    {
        _navigationManager = navigationManager;
        _notificationService = notificationService;
    }

    public async Task TransferToUnauthorizedAccessPage(string message)
    {
        ErrorMessage = message;
        _navigationManager.NavigateTo("/unauthorized");
    }

    public async Task NotifyUser()
    {
        Message message = new Message()
        {
            Subject = ErrorMessage,
            Title = "Unhandled Exception"
        };
       _notificationService.ShowError(message);
    }
}
