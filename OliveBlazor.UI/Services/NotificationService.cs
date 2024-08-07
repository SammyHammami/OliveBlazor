using Blazored.Toast.Configuration;
using Blazored.Toast.Services;
using Microsoft.JSInterop;
using OliveBlazor.Core.Application.Services;
using OliveBlazor.Core.Domain.Common.Communication;

namespace OliveBlazor.UI.Services;

public class NotificationService:INotificationService
{

    private readonly IJSRuntime _jsRunTime;
    private readonly IToastService _toastService;

    public NotificationService(IJSRuntime jsRunTime, IToastService toastService)
    {
        _jsRunTime = jsRunTime;
        _toastService = toastService;
    }


    public void ShowSuccessful(Message message)
    {
        _toastService.ShowSuccess(message.Subject,Settings );

        void Settings(ToastSettings obj)
        {
            obj.Timeout = 4;
            obj.ShowProgressBar=true;
        }

        
    }

    public void ShowWarning(Message message)
    {
        _toastService.ShowInfo(message.Subject, Settings);

        void Settings(ToastSettings obj)
        {
            obj.Timeout = 10;
            obj.ShowProgressBar = true;
        }
    }


    public void ShowError(Message message)
    {
        _toastService.ShowError(message.Subject, Settings);

        void Settings(ToastSettings obj)
        {
           obj.DisableTimeout=true;
           obj.ShowCloseButton=true;
        }
    }
}