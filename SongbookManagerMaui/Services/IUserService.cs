using SongbookManagerMaui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongbookManagerMaui.Services
{
    public interface IUserService
    {
        Task<bool> LoginUser(string email, string password);

        Task<bool> RegisterUSer(string name, string email, string password);

        Task<User> GetUser(string email);

        Task<List<User>> GetSharedUsers(string email);

        Task<List<User>> GetSingers(string email);

        Task UpdateUser(User user);

        Task<List<User>> GetUsers();

        Task DeleteUser(User user);
    }
}
