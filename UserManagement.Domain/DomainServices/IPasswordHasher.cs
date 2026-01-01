using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Domain.DomainServices
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string passwordHash);
    }
}
