using MediatR;
using OliveBlazor.Core.Application.Common.Interfaces;

namespace OliveBlazor.Core.Application.Common.Behaviours;

public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    public TransactionBehaviour(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _unitOfWork.BeginTransaction();
        try
        {
            var response = await next();
            _unitOfWork.Commit();
            return response;
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
}
