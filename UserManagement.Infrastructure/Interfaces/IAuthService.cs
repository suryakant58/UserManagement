using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Interfaces
{
    public interface IAuthService
    {
        Task AddAsync(User user);
        Task<User> GetByEmailAsync(string email);

        Task<User> UpdateAsync(User user);
    }
}
