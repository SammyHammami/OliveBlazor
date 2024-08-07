using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace OliveBlazor.Infrastructure.Indentity.Permissions;

public class EnumPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private readonly ConcurrentDictionary<string, AuthorizationPolicy> _policyCache = new ConcurrentDictionary<string, AuthorizationPolicy>();

    public EnumPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
    }

    public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        return Task.FromResult(_policyCache.GetOrAdd(policyName, policyName =>
        {
            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new EnumRequirement(policyName));
            return policy.Build();
        }));
    }
}
