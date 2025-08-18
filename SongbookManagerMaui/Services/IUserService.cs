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
    }
}
