namespace OliveBlazor.Core.Application.Common.Interfaces;

public interface IUnitOfWork
{
    void BeginTransaction();
    void Commit();
    void Rollback();
}
