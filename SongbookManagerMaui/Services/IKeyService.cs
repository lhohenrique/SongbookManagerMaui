using SongbookManagerMaui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongbookManagerMaui.Services
{
    public interface IKeyService
    {
        Task<bool> IsUserKeyExists(UserKey userKey);

        Task InsertKey(UserKey key);

        Task<UserKey> GetKeyByUser(string userEmail, string musicName);

        Task<List<UserKey>> GetKeysByOwner(string musicOwner, string musicName);

        Task UpdateKey(UserKey key);

        Task RemoveUserKeyByMusic(string musicOwner, string musicName);

        Task ClearUserKeys(string userEmail);
    }
}
