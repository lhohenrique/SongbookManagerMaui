using SongbookManagerMaui.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongbookManagerMaui.Services
{
    public interface IMusicService
    {
        #region Properties
        Music Music { get; set; }
        ObservableCollection<Music> Musics { get; set; }
        #endregion

        Task<List<Music>> GetMusics();

        Task<ObservableCollection<Music>> GetMusicsByUser(string userEmail);

        Task<ObservableCollection<Music>> GetMusicsByUserDescending(string userEmail);

        Task<Music> GetMusicByNameAndAuthor(string name, string author, string owner);

        Task InsertMusic(Music music);

        Task UpdateMusic(Music music, string oldName);

        Task DeleteMusic(Music music);

        Task<ObservableCollection<Music>> SearchMusic(string searchText, string userEmail);

        Task DeleteAll();

        void SetMusic(Music selectedMusic);
    }
}
