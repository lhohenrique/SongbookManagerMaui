using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SongbookManagerMaui.Models;
using SongbookManagerMaui.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongbookManagerMaui.ViewModels
{
    public partial class PreviewMusicPageViewModel : ObservableObject
    {
        #region Fields
        IMusicService _musicService;
        #endregion

        #region Properties
        [ObservableProperty]
        private Music music;

        [ObservableProperty]
        private ObservableCollection<UserKey> userList = new ObservableCollection<UserKey>();

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string author;

        [ObservableProperty]
        private string lyrics;

        [ObservableProperty]
        private string chords;

        [ObservableProperty]
        private bool hasSingers = false;

        [ObservableProperty]
        private bool isPlayMusicVisible = false;

        [ObservableProperty]
        private ObservableCollection<string> keyList = new ObservableCollection<string>() { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        [ObservableProperty]
        private string selectedKey;
        //PropertyChanged: SetChords();
        #endregion

        public PreviewMusicPageViewModel(IMusicService musicService)
        {
            _musicService = musicService;

            Music = _musicService.Music;
        }

        #region Methods
        [RelayCommand]
        private void ShowMusicDetails()
        {
            if(Music is not null)
            {
                Name = Music.Name;
                Author = Music.Author;
                SelectedKey = Music.Key;
                Lyrics = Music.Lyrics;

                IsPlayMusicVisible = !string.IsNullOrEmpty(Music.Version);

                //await GetMusicKeys();
            }

            HasSingers = UserList.Any();
        }
        #endregion
    }
}
