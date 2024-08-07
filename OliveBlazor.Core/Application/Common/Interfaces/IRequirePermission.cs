using OliveBlazor.Core.Domain.Security;

namespace OliveBlazor.Core.Application.Common.Interfaces;

public interface IRequirePermission
{
    IEnumerable<Permissions> RequiredPermissions { get; }
}
