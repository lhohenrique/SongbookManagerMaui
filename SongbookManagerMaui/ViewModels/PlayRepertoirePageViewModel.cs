using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SongbookManagerMaui.Helpers;
using SongbookManagerMaui.Models;
using SongbookManagerMaui.Resx;
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
    public partial class PlayRepertoirePageViewModel : ObservableObject
    {
        #region Fields
        private bool pageLoaded = false;

        private IMusicService _musicService;
        private IRepertoireService _repertoireService;
        private Repertoire _repertoire;
        private List<Music> _musicList;
        private int _musicNumber = 0;
        private int _musicCount;
        #endregion

        #region Properties
        [ObservableProperty]
        private bool isLyricsChecked = true;

        [ObservableProperty]
        private bool isChordsChecked = false;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string author;

        [ObservableProperty]
        private string lyrics;

        [ObservableProperty]
        private string chords;

        [ObservableProperty]
        private ObservableCollection<string> keyList = new ObservableCollection<string>() { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        [ObservableProperty]
        private string selectedKey;

        //SetChords();
        #endregion

        public PlayRepertoirePageViewModel(IMusicService musicService, IRepertoireService repertoireService)
        {
            _musicService = musicService;
            _repertoireService = repertoireService;

            _repertoire = _repertoireService.Repertoire;
        }

        #region Methods
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(SelectedKey))
            {
                SetChords();
            }
        }

        [RelayCommand]
        public void NextMusic()
        {
            int nextMusic = _musicNumber + 1;

            if (_musicList != null && nextMusic <= _musicCount - 1)
            {
                Music music = _musicList[nextMusic];
                MusicRep musicRep = _repertoire.Musics[nextMusic];

                _musicNumber = nextMusic;

                if (music != null && musicRep != null)
                {
                    SetMusic(music, musicRep);
                }
            }
        }

        [RelayCommand]
        public void PreviousMusic()
        {
            int previousMusic = _musicNumber - 1;

            if (_musicList != null && previousMusic >= 0)
            {
                Music music = _musicList[previousMusic];
                MusicRep musicRep = _repertoire.Musics[previousMusic];

                _musicNumber = previousMusic;

                if (music != null && musicRep != null)
                {
                    SetMusic(music, musicRep);
                }
            }
        }

        public async Task LoadPageAsync()
        {
            try
            {
                if (_repertoire is not null && _repertoire.Musics is not null)
                {
                    _musicCount = _repertoire.Musics.Count;

                    //Load musics from repertoire
                    _musicList = new List<Music>();
                    foreach (MusicRep item in _repertoire.Musics)
                    {
                        Music musicLoaded = await _musicService.GetMusicByNameAndAuthor(item.Name, item.Author, item.Owner);
                        _musicList.Add(musicLoaded);
                    }

                    Music music = _musicList.FirstOrDefault();
                    MusicRep musicRep = _repertoire.Musics.FirstOrDefault();

                    if (music != null && musicRep != null)
                    {
                        SetMusic(music, musicRep);
                    }
                }

                pageLoaded = true;
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.CouldNotLoadSong, AppResources.Ok);
            }
        }

        private void SetMusic(Music music, MusicRep musicRep)
        {
            Name = music.Name;
            Author = music.Author;
            Lyrics = music.Lyrics;

            if (!string.IsNullOrEmpty(musicRep.SingerKey))
            {
                SelectedKey = musicRep.SingerKey;
            }
            else
            {
                SelectedKey = music.Key;
            }
        }

        private void SetChords()
        {
            Music music = _musicList[_musicNumber];

            if (string.IsNullOrEmpty(SelectedKey))
            {
                Chords = music.Chords;
            }
            else
            {
                if (SelectedKey.Equals(music.Key))
                {
                    Chords = music.Chords;
                }
                else
                {
                    Chords = Utils.GetChordsAccordingKey(music.Key, music.Chords, SelectedKey);
                }
            }
        }
        #endregion
    }
}
