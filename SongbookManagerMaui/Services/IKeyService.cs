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

        Task<UserKey> GetKeyByUser(string userEmail, string musicId);

        Task<List<UserKey>> GetKeysByOwner(string musicOwner, string musicId);

        Task UpdateKey(UserKey key);

        Task RemoveUserKey(UserKey key);
        
        Task ClearMusicUserKey(string musicId);

        Task ClearUserKeys(string userEmail);

        Task<List<UserKey>> GetAllKeys();
    }
}
