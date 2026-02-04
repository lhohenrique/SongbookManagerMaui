using CommunityToolkit.Maui.Core.Extensions;
using Firebase.Database;
using Firebase.Database.Query;
using SongbookManagerMaui.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private ObservableCollection<Music> musics = new();
        public ObservableCollection<Music> Musics
        {
            get { return musics; }
            set { musics = value;  }
        }

        #endregion

        public MusicService()
        {
            client = new FirebaseClient("https://songbookmanagerlite-default-rtdb.firebaseio.com/");
        }

        /// <summary>
        /// Get all musics. Should be used just for Admin methods
        /// </summary>
        /// <returns></returns>
        public async Task<List<Music>> GetMusics()
        {
            var musics = (await client.Child("Musics").OnceAsync<Music>())
                .Select(item => new Music
                {
                    Id = item.Key,
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

        public async Task<ObservableCollection<Music>> GetMusicsByUser(string userEmail)
        {
            if (!Musics.Any())
            {
                Musics = (await client.Child("Musics").OnceAsync<Music>()).Select(item => new Music
                {
                    Id = item.Key,
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
                }).Where(m => m.Owner.Equals(userEmail)).ToObservableCollection();
            }
            
            return Musics;
        }

        public async Task<ObservableCollection<Music>> GetMusicsByUserDescending(string userEmail)
        {
            if (!Musics.Any())
            {
                Musics = (await client.Child("Musics").OnceAsync<Music>()).Select(item => new Music
                {
                    Id = item.Key,
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
                }).Where(m => m.Owner.Equals(userEmail)).OrderByDescending(m => m.CreationDate).ToObservableCollection();
            }

            return Musics;
        }

        public async Task<Music> GetMusicByNameAndAuthor(string name, string author, string owner)
        {
            var music = (await client.Child("Musics").OnceAsync<Music>()).Select(item => new Music
            {
                Id = item.Key,
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

        public async Task InsertMusic(Music music)
        {
            var musicAdded = await client.Child("Musics").PostAsync(music);
            
            if(musicAdded != null)
            {
                music.Id = musicAdded.Key;
                Musics.Insert(0, music);
            }
        }

        public async Task UpdateMusic(Music music, string oldName)
        {
            await client.Child($"Musics/{music.Id}").PutAsync(music);

            // Update service list
            int index = Musics.IndexOf(music);
            if(index != -1)
            {
                Musics.RemoveAt(index);
                Musics.Insert(index, music);
            }
        }

        public async Task DeleteMusic(Music music)
        {
            await client.Child($"Musics/{music.Id}").DeleteAsync();

            // Delete from service list
            Musics.Remove(music);
        }

        public async Task<ObservableCollection<Music>> SearchMusic(string searchText, string userEmail)
        {
            return Musics.Where(m => m.Owner.Equals(userEmail) && m.Name.ToUpper().Contains(searchText.ToUpper())).OrderByDescending(m => m.CreationDate).ToObservableCollection();
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
