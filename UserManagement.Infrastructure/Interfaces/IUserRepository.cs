using System.Threading.Tasks;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetByEmailAsync(string email);


    }
}
