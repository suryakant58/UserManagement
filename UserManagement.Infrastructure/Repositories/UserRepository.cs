using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Interfaces;
using UserManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace UserManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;

        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

     
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }


        public async Task<User> GetByEmailAsync(string email)
        {
            // Find by the Email value object property instead of using FindAsync (which expects the primary key)
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.Value == email);
        }
    }
}
