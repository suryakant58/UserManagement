using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Interfaces;
using UserManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Infrastructure.Repositories
{
    
        public class AuthService : IAuthService
        {
            private readonly UserDbContext _context;

            public AuthService(UserDbContext context)
            {
                _context = context;
            }

            public async Task AddAsync(User user)
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }

            public async Task<User> GetByEmailAsync(string email)
            {
                // Find by the Email value object property instead of using FindAsync (which expects the primary key)
                return await _context.Users.FirstOrDefaultAsync(u => u.Email.Value == email);
            }

        public async Task<User> UpdateAsync(User user)
        {
            
             _context.Update(user);
            await _context.SaveChangesAsync();
            return user;

        }
    }
}
