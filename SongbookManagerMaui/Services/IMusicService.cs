using SongbookManagerMaui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongbookManagerMaui.Services
{
    public interface IMusicService
    {
        Task<List<Music>> GetMusics();

        Task<List<Music>> GetMusicsByUser(string userEmail);

        Task<List<Music>> GetMusicsByUserDescending(string userEmail);

        Task<Music> GetMusicByNameAndAuthor(string name, string author, string owner);

        Task<bool> InsertMusic(Music music);

        Task UpdateMusic(Music music, string oldName);

        Task DeleteMusic(Music music);

        Task<List<Music>> SearchMusic(string searchText, string userEmail);

        Task DeleteAll();
    }
}
