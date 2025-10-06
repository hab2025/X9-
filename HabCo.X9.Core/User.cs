namespace HabCo.X9.Core;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public bool IsActive { get; set; } = true;
    public int RoleId { get; set; }
    public Role Role { get; set; }
}