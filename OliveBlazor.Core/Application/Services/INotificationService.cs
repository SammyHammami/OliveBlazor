using OliveBlazor.Core.Domain.Common.Communication;

namespace OliveBlazor.Core.Application.Services;


//TODO FOLLOW THAT OF SENTENCING
public interface INotificationService
{
    void ShowSuccessful(Message message);
    void ShowWarning(Message message);
    void ShowError(Message message);
}