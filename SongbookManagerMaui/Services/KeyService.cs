using Firebase.Database;
using Firebase.Database.Query;
using SongbookManagerMaui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongbookManagerMaui.Services
{
    public class KeyService : IKeyService
    {
        #region Fields
        FirebaseClient client;
        #endregion

        public KeyService()
        {
            client = new FirebaseClient("https://songbookmanagerlite-default-rtdb.firebaseio.com/");
        }

        public async Task<bool> IsUserKeyExists(UserKey userKey)
        {
            var key = (await client.Child("Keys").OnceAsync<UserKey>()).Where(u => u.Object.UserEmail == userKey.UserEmail && u.Object.MusicId == userKey.MusicId).FirstOrDefault();

            return key != null;
        }

        public async Task InsertKey(UserKey key)
        {
            await client.Child("Keys").PostAsync(key);
        }

        public async Task<UserKey> GetKeyByUser(string userEmail, string musicId)
        {
            var key = (await client.Child("Keys").OnceAsync<UserKey>()).Select(item => new UserKey
            {
                Id = item.Key,
                Key = item.Object.Key,
                UserEmail = item.Object.UserEmail,
                UserName = item.Object.UserName,
                MusicName = item.Object.MusicName,
                MusicOwner = item.Object.MusicOwner,
                MusicId = item.Object.MusicId
            }).Where(k => k.UserEmail.Equals(userEmail) && k.MusicId == musicId).FirstOrDefault();

            return key;
        }

        public async Task<List<UserKey>> GetKeysByOwner(string musicOwner, string musicId)
        {
            var keys = (await client.Child("Keys").OnceAsync<UserKey>()).Select(item => new UserKey
            {
                Id = item.Key,
                Key = item.Object.Key,
                UserEmail = item.Object.UserEmail,
                UserName = item.Object.UserName,
                MusicName = item.Object.MusicName,
                MusicOwner = item.Object.MusicOwner,
                MusicId = item.Object.MusicId
            }).Where(k => k.MusicOwner == musicOwner && k.MusicId == musicId).ToList();

            return keys;
        }

        public async Task UpdateKey(UserKey key)
        {
            await client.Child($"Keys/{key.Id}").PutAsync(key);
        }

        public async Task RemoveUserKey(UserKey key)
        {
            await client.Child($"Keys/{key.Id}").DeleteAsync();
        }

        public async Task ClearMusicUserKey(string musicId)
        {
            var keysToRemove = (await client.Child("Keys").OnceAsync<UserKey>())
                                                .Where(k => k.Object.MusicId == musicId).ToList();

            foreach (var key in keysToRemove)
            {
                await client.Child("Keys").Child(key.Key).DeleteAsync();
            }
        }

        public async Task ClearUserKeys(string userEmail)
        {
            var keysToRemove = (await client.Child("Keys").OnceAsync<UserKey>())
                                                .Where(k => k.Object.UserEmail == userEmail).ToList();

            foreach (var key in keysToRemove)
            {
                await client.Child("Keys").Child(key.Key).DeleteAsync();
            }
        }

        public async Task<List<UserKey>> GetAllKeys()
        {
            var keys = (await client.Child("Keys").OnceAsync<UserKey>()).Select(item => new UserKey
            {
                Id = item.Key,
                Key = item.Object.Key,
                UserEmail = item.Object.UserEmail,
                MusicId = item.Object.MusicId,
                UserName = item.Object.UserName,
                MusicName = item.Object.MusicName,
                MusicOwner = item.Object.MusicOwner
            }).ToList();

            return keys;
        }
    }
}
