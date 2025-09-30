namespace Movura.Api.Constants;

public static class UserRoles
{
    public const string Admin = "Admin";
    public const string ParkingAdmin = "ParkingAdmin";
    public const string Comercio = "Comercio";
    public const string Cliente = "Cliente";
}

public static class Permissions
{
    public const string ManageUsers = "ManageUsers";
    public const string ManageParkings = "ManageParkings";
    public const string ManageComercios = "ManageComercios";
    public const string ViewReports = "ViewReports";
    public const string ManualPayment = "ManualPayment";
}

public static class Policies
{
    public const string RequireAdminRole = "RequireAdminRole";
    public const string RequireComercioRole = "RequireComercioRole";
    public const string CanManageUsers = "CanManageUsers";
    public const string CanManageParkings = "CanManageParkings";
    public const string CanManageComercios = "CanManageComercios";
    public const string CanViewReports = "CanViewReports";
    public const string CanProcessManualPayments = "CanProcessManualPayments";
}