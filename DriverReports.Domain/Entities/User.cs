namespace DriverReports.Domain.Entities
{
    [Flags]
    public enum UserRole
    {
        None = 0,
        Driver = 1,
        Admin = 2
    }

    public class User
    {
        public User(Guid id, string name, string email, string passwordHash, UserRole roles)
        {
            Id = id;
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Roles = roles;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public UserRole Roles { get; private set; }

        public User(string name, string email, string passwordHash, UserRole roles)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Roles = roles;
        }

        public void AddRole(UserRole role)
        {
            Roles |= role;
        }

        public void RemoveRole(UserRole role)
        {
            Roles &= ~role;
        }

        public bool HasRole(UserRole role)
        {
            return (Roles & role) == role;
        }
    }
}