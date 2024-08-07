namespace OliveBlazor.Core.Application.Services;

public interface IErrorService
{
    string ErrorMessage { get; set; }
    Task TransferToUnauthorizedAccessPage(string message);
    Task NotifyUser();
}