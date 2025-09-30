using Microsoft.AspNetCore.Authorization;
using Movura.Api.Constants;

namespace Movura.Api.Services.Auth;

public static class AuthorizationPolicies
{
    public static void ConfigurePolicies(AuthorizationOptions options)
    {
        // Políticas basadas en roles
        options.AddPolicy(Policies.RequireAdminRole, policy =>
            policy.RequireRole(UserRoles.Admin));
            
        options.AddPolicy(Policies.RequireComercioRole, policy =>
            policy.RequireRole(UserRoles.Comercio));

        // Políticas basadas en permisos
        options.AddPolicy(Policies.CanManageUsers, policy =>
            policy.RequireClaim(Permissions.ManageUsers));

        options.AddPolicy(Policies.CanManageParkings, policy =>
            policy.RequireClaim(Permissions.ManageParkings));

        options.AddPolicy(Policies.CanManageComercios, policy =>
            policy.RequireClaim(Permissions.ManageComercios));

        options.AddPolicy(Policies.CanViewReports, policy =>
            policy.RequireClaim(Permissions.ViewReports));

        options.AddPolicy(Policies.CanProcessManualPayments, policy =>
            policy.RequireClaim(Permissions.ManualPayment));
    }
}