using Microsoft.AspNetCore.Authorization;

namespace ITI.Shipping.APIs.Filters;
public class PermissionAuthorizationHandler:AuthorizationHandler<PermissionRequirements>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,PermissionRequirements requirement)
    {
        if(context.User.Identity is not { IsAuthenticated: true })
            return Task.CompletedTask;

        var userPermissions = context.User.Claims
            .Where(c => c.Type == "permissions")
            .Select(c => c.Value)
            .ToList();

        if(requirement.Permissions.Any(p => userPermissions.Contains(p)))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}