using Microsoft.AspNetCore.Authorization;

namespace ITI.Shipping.APIs.Filters;
public class PermissionRequirements:IAuthorizationRequirement
{
    public PermissionRequirements(params string[] permissions)
    {
        Permissions = permissions;
    }

    public string[] Permissions { get; }
}