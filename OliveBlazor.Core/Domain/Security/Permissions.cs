namespace OliveBlazor.Core.Domain.Security;

[Flags]
public enum Permissions
{
    None = 0,
    ViewUsers = 1 << 0,       // 1
    ManageUsers = 1 << 1,     // 2
    ViewRoles = 1 << 2,      // 4
    AddRoles = 1 << 3     // 8
    ,ManageSecurity = 1 << 4//16
    ,ManagePermissions= 1 << 5 //32
    ,AssignRoles = 1 << 6 //64
    ,Demo = 1<<7 //128


}

public static class PermissionsExtensions
{
    public static string ToPolicy(this Permissions permission)
    {
        return permission.ToString();
    }
}
public static class EnumExtensions
{
    public static IEnumerable<T> GetIndividualFlags<T>(this T value) where T : Enum
    {
        foreach (T flag in Enum.GetValues(value.GetType()))
        {
            if (value.HasFlag(flag))
            {
                yield return flag;
            }
        }
    }
}

