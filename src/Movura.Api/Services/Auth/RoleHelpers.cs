using System.Security.Claims;
using Movura.Api.Constants;

namespace Movura.Api.Services.Auth;

public static class RoleHelpers
{
    public static bool CanManageUsers(this ClaimsPrincipal user)
        => user.HasClaim(c => c.Type == Permissions.ManageUsers && c.Value == "true");

    public static bool CanManageParkings(this ClaimsPrincipal user)
        => user.HasClaim(c => c.Type == Permissions.ManageParkings && c.Value == "true");

    public static bool CanManageComercios(this ClaimsPrincipal user)
        => user.HasClaim(c => c.Type == Permissions.ManageComercios && c.Value == "true");

    public static bool CanViewReports(this ClaimsPrincipal user)
        => user.HasClaim(c => c.Type == Permissions.ViewReports && c.Value == "true");

    public static bool CanProcessManualPayments(this ClaimsPrincipal user)
        => user.HasClaim(c => c.Type == Permissions.ManualPayment && c.Value == "true");

    public static bool IsAdmin(this ClaimsPrincipal user)
        => user.IsInRole(UserRoles.Admin);

    public static bool IsComercio(this ClaimsPrincipal user)
        => user.IsInRole(UserRoles.Comercio);

    public static bool IsCliente(this ClaimsPrincipal user)
        => user.IsInRole(UserRoles.Cliente);
}