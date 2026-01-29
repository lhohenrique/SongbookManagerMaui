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
    public class MusicService : IMusicService
    {
        #region Fields
        FirebaseClient client;
        #endregion

        #region Properties
        private Music music;

        public Music Music
        {
            get { return music; }
            set { music = value; }
        }

        private List<Music> musics = new();
        public List<Music> Musics
        {
            get { return musics; }
            set { musics = value;  }
        }

        #endregion

        public MusicService()
        {
            client = new FirebaseClient("https://songbookmanagerlite-default-rtdb.firebaseio.com/");
        }

        public async Task<List<Music>> GetMusics()
        {
            var musics = (await client.Child("Musics").OnceAsync<Music>())
                .Select(item => new Music
                {
                    Name = item.Object.Name,
                    Author = item.Object.Author,
                    Category = item.Object.Category,
                    Key = item.Object.Key,
                    Lyrics = item.Object.Lyrics,
                    Chords = item.Object.Chords,
                    Owner = item.Object.Owner,
                    Version = item.Object.Version,
                    Notes = item.Object.Notes,
                    CreationDate = item.Object.CreationDate
                }).ToList();

            return musics;
        }

        public async Task<List<Music>> GetMusicsByUser(string userEmail, bool forceUpdate = false)
        {
            if (!Musics.Any() || forceUpdate)
            {
                Musics = (await client.Child("Musics").OnceAsync<Music>()).Select(item => new Music
                {
                    Name = item.Object.Name,
                    Author = item.Object.Author,
                    Category = item.Object.Category,
                    Key = item.Object.Key,
                    Lyrics = item.Object.Lyrics,
                    Chords = item.Object.Chords,
                    Owner = item.Object.Owner,
                    Version = item.Object.Version,
                    Notes = item.Object.Notes,
                    CreationDate = item.Object.CreationDate
                }).Where(m => m.Owner.Equals(userEmail)).ToList();
            }
            
            return Musics;
        }

        public async Task<List<Music>> GetMusicsByUserDescending(string userEmail)
        {
            if (!Musics.Any())
            {
                Musics = (await client.Child("Musics").OnceAsync<Music>()).Select(item => new Music
                {
                    Name = item.Object.Name,
                    Author = item.Object.Author,
                    Category = item.Object.Category,
                    Key = item.Object.Key,
                    Lyrics = item.Object.Lyrics,
                    Chords = item.Object.Chords,
                    Owner = item.Object.Owner,
                    Version = item.Object.Version,
                    Notes = item.Object.Notes,
                    CreationDate = item.Object.CreationDate
                }).Where(m => m.Owner.Equals(userEmail)).OrderByDescending(m => m.CreationDate).ToList();
            }

            return Musics;
        }

        public async Task<Music> GetMusicByNameAndAuthor(string name, string author, string owner)
        {
            var music = (await client.Child("Musics").OnceAsync<Music>()).Select(item => new Music
            {
                Name = item.Object.Name,
                Author = item.Object.Author,
                Category = item.Object.Category,
                Key = item.Object.Key,
                Lyrics = item.Object.Lyrics,
                Chords = item.Object.Chords,
                Owner = item.Object.Owner,
                Version = item.Object.Version,
                Notes = item.Object.Notes,
                CreationDate = item.Object.CreationDate
            }).Where(m => m.Owner.Equals(owner) && m.Name.Equals(name) && m.Author.Equals(author)).FirstOrDefault();

            return music;
        }

        public async Task<bool> InsertMusic(Music music)
        {
            await client.Child("Musics").PostAsync(music);
            Musics.Add(music);

            return true;
        }

        public async Task UpdateMusic(Music music, string oldName)
        {
            var musicToUpdate = (await client.Child("Musics").OnceAsync<Music>())
                                                .Where(m => m.Object.Name.Equals(oldName) && m.Object.Owner.Equals(music.Owner)).FirstOrDefault();

            await client.Child("Musics").Child(musicToUpdate.Key).PutAsync(music);

            GetMusicsByUser(music.Owner, true);
        }

        public async Task DeleteMusic(Music music)
        {
            var musicToDelete = (await client.Child("Musics").OnceAsync<Music>())
                                                .Where(m => m.Object.Name.Equals(music.Name) && m.Object.Owner.Equals(music.Owner)).FirstOrDefault();

            await client.Child("Musics").Child(musicToDelete.Key).DeleteAsync();

            GetMusicsByUser(music.Owner, true);
        }

        public async Task<List<Music>> SearchMusic(string searchText, string userEmail)
        {
            return Musics.Where(m => m.Owner.Equals(userEmail) && m.Name.ToUpper().Contains(searchText.ToUpper())).OrderByDescending(m => m.CreationDate).ToList();
        }

        public async Task DeleteAll()
        {
            await client.Child("Musics").DeleteAsync();
            Musics.Clear();
        }

        public void SetMusic(Music selectedMusic)
        {
            Music = selectedMusic;
        }
    }
}
