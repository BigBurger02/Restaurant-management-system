﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

using Restaurant_management_system.Infrastructure.Data.Authorization;

namespace Restaurant_management_system.Infrastructure.Data.Authorization;

public class CooksAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, object>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                    OperationAuthorizationRequirement requirement,
                                                    object resource)// resourse is gag
    {
        if (context.User == null)
        {
            return Task.CompletedTask;
        }

        if (context.User.IsInRole(RolesEnumeration.Cook.ToString()))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}