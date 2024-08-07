using MediatR;
using Microsoft.Extensions.Logging;
using OliveBlazor.Core.Application.Services;

namespace OliveBlazor.Core.Application.Common.Behaviours;


public class UnhandledExceptionBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;
    private readonly IErrorService _errorService;

    public UnhandledExceptionBehaviour(ILogger<TRequest> logger, IErrorService errorService)
    {
        _logger = logger;
        _errorService = errorService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }

        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;
            _errorService.ErrorMessage = $" Unhandled Exception for Request {requestName} {request}: {ex.Message}";
            await _errorService.NotifyUser();
            _logger.LogError(
                ex,
                " Unhandled Exception for Request {Name} {@Request}",
                requestName,
                request);

            throw;
        }


    }
}
