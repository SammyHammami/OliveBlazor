using MediatR;
using Microsoft.Extensions.Logging;
using OliveBlazor.Core.Application.Common.Interfaces;
using OliveBlazor.Core.Application.Services;
using OliveBlazor.Core.Domain.Common.Communication;

namespace OliveBlazor.Core.Application.Common.Behaviours;

public class SuccessNotificationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<TRequest> _logger;
    private readonly INotificationService _notificationService;

    public SuccessNotificationBehaviour(ILogger<TRequest> logger, INotificationService notificationService)
    {
        _logger = logger;
        _notificationService = notificationService;
    }



    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            
            var response = await next();


            if  (request is IAnnounceCompletion )
            {
                var requestName = typeof(TRequest).Name;
                // Show success notification
                _notificationService.ShowSuccessful(new Message()
                {
                    Title = $"Successful Operation : {requestName}",
                    Subject = "Record Saved Successfully "
                });

            }

            return response;
        }
        catch (Exception ex)
        {

            var requestName = typeof(TRequest).Name;
            var message = $"Request {requestName} failed: {ex.Message}";
            _logger.LogError(ex, message);

            throw;
        }
    }
}
