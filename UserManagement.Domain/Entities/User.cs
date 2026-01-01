using UserManagement.Domain.DomainServices;
using UserManagement.Domain.ValueObject;

namespace UserManagement.Domain.Entities
{
    public class User
    {
        // Primary Key
        public Guid Id { get; private set; }

        // Value Objects

        public Email Email { get; private set; }
        public string PasswordHash { get;  set; }

        // Domain Properties
        public string FullName { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsActive { get; private set; }
        public string Role { get; private set; } // e.g., Student, Instructor, Admin

        // Constructor
        private User() { } // For EF Core

        public User(Email email, string passwordHash, string fullName, string role)
        {
            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            FullName = fullName;
            Role = role;
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        // Domain Behaviors
        public void Deactivate() => IsActive = false;

        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
        }

        public void ChangeRole(string newRole)
        {
            Role = newRole;
        }
    }
}