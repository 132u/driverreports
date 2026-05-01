using System.Data;
using System.Xml.Linq;

namespace DriverReports.Domain.Entities
{
    [Flags]
    public enum UserRole
    {
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

        public static (User user, string Error) Create(Guid id, string name, string email, string passwordHash, UserRole role)
        {
            var error = string.Empty;

            if (string.IsNullOrWhiteSpace(name))
                error = "Name is required";

            if (string.IsNullOrWhiteSpace(email))
                error = "Email is required";
            
            if (string.IsNullOrWhiteSpace(passwordHash))
                error = "Password is required";
            
            var user = new User(id, name, email, passwordHash, role);
            return (user, error);
        }
    }
}