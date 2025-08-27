using Microsoft.AspNetCore.Authorization;

namespace ITI.Shipping.APIs.Filters;
public class HasPermissionAttribute:AuthorizeAttribute
{
    public HasPermissionAttribute(params string[] permissions)
    {
        Permissions = permissions;
        Policy = string.Join(",",permissions); // نخزنهم في الـ Policy Name
    }

    public string[] Permissions { get; }
}