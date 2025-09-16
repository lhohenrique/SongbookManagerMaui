using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SongbookManagerMaui.Helpers;
using SongbookManagerMaui.Models;
using SongbookManagerMaui.Resx;
using SongbookManagerMaui.Services;
using SongbookManagerMaui.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongbookManagerMaui.ViewModels
{
    public partial class AddEditRepertoirePageViewModel : ObservableObject
    {
        #region Fields
        private Repertoire _repertoire;
        private DateTime _oldDate;
        private TimeSpan _oldTime;
        private IRepertoireService _repertoireService;
        private IMusicService _musicService;
        private IKeyService _keyService;
        private IUserService _userService;
        private bool _pageLoaded = false;
        private List<User> _singerUserList = new List<User>();
        #endregion

        #region Properties
        [ObservableProperty]
        private string selectedSinger;

        [ObservableProperty]
        private ObservableCollection<string> singers = new ObservableCollection<string>();

        [ObservableProperty]
        private DateTime date = DateTime.Now;

        [ObservableProperty]
        private TimeSpan time = TimeSpan.Zero;

        [ObservableProperty]
        private ObservableCollection<MusicRep> musicList = new ObservableCollection<MusicRep>();

        [ObservableProperty]
        private List<MusicRep> selectedMusics = new List<MusicRep>();

        [ObservableProperty]
        private MusicRep selectedMusic;

        [ObservableProperty]
        private string searchText = string.Empty;
        #endregion

        public AddEditRepertoirePageViewModel(IRepertoireService repertoireService, IMusicService musicService, IKeyService keyService, IUserService userService)
        {
            _repertoireService = repertoireService;
            _musicService = musicService;
            _keyService = keyService;
            _userService = userService;

            _repertoire = _repertoireService.Repertoire;

            if (_repertoire is not null)
            {
                _oldDate = _repertoire.Date;
                _oldTime = _repertoire.Time;
            }
        }

        #region Methods
        [RelayCommand]
        public async Task SaveRepertoire()
        {
            try
            {
                string userEmail = GetSingerEmail();

                if (_repertoire != null)
                {
                    _repertoire.Date = Date;
                    _repertoire.Time = Time;
                    _repertoire.Musics = SelectedMusics.ToList();
                    _repertoire.SingerName = SelectedSinger;
                    _repertoire.SingerEmail = userEmail;

                    await _repertoireService.UpdateRepertoire(_repertoire, _oldDate, _oldTime);
                }
                else
                {
                    Repertoire newRepertoire = new Repertoire()
                    {
                        Date = Date,
                        Time = Time,
                        Musics = SelectedMusics.ToList(),
                        Owner = LoggedUserHelper.GetEmail(),
                        SingerName = SelectedSinger,
                        SingerEmail = userEmail
                    };

                    await _repertoireService.InsertRepertoire(newRepertoire);
                }

                await Shell.Current.GoToAsync($"//{nameof(RepertoirePage)}");
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.CouldNotSaveRepertoire, AppResources.Ok);
            }
        }

        public void SelectionChanged(MusicRep musicTapped, int musicTappedIndex)
        {
            if (musicTapped.IsSelected)
            {
                musicTapped.IsSelected = false;
                
                for (int i = 0; i < SelectedMusics.Count; i++)
                {
                    if (SelectedMusics[i].Name.Equals(musicTapped.Name) && SelectedMusics[i].Author.Equals(musicTapped.Author))
                    {
                        SelectedMusics.RemoveAt(i);
                    }
                }
                
                MusicList.RemoveAt(musicTappedIndex);
                MusicList.Insert(musicTappedIndex, musicTapped);
            }
            else
            {
                musicTapped.IsSelected = true;

                SelectedMusics.Add(musicTapped);

                MusicList.RemoveAt(musicTappedIndex);
                MusicList.Insert(musicTappedIndex, musicTapped);
            }
        }

        [RelayCommand]
        private async Task Search()
        {
            try
            {
                if (!_pageLoaded)
                {
                    return;
                }

                var userEmail = LoggedUserHelper.GetEmail();
                List<Music> musicListUpdated = await _musicService.SearchMusic(SearchText, userEmail);

                MusicList.Clear();

                bool isSelected = false;
                foreach (Music music in musicListUpdated)
                {
                    var selectedMusic = SelectedMusics.FirstOrDefault(m => m.Name.Equals(music.Name) && m.Author.Equals(music.Author));
                    isSelected = selectedMusic != null;

                    MusicList.Add(new MusicRep()
                    {
                        Name = music.Name,
                        Author = music.Author,
                        Owner = music.Owner,
                        IsSelected = isSelected
                    });
                }
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.UnablePerformSearch, AppResources.Ok);
            }
        }

        [RelayCommand]
        private async Task Tutorial()
        {
            string message = string.Empty;
            message += "- " + AppResources.AddRepertoirePageTutorial;

            await App.Current.MainPage.DisplayAlert(AppResources.Tutorial, message, AppResources.Ok);
        }

        public async Task PopulateRepertoireFieldsAsync()
        {
            await LoadSingersAsync();
            await LoadMusicsAsync();

            if (_repertoire != null)
            {
                LoadRepertoire();
            }
            else
            {
                SetLoggedSinger();
            }

            _pageLoaded = true;
        }

        private void LoadRepertoire()
        {
            SelectedSinger = _repertoire.SingerName;
            Date = _repertoire.Date;
            Time = _repertoire.Time;

            if (_repertoire.Musics != null)
            {
                SelectedMusics = _repertoire.Musics.ToList();

                foreach (MusicRep musicSelected in _repertoire.Musics)
                {
                    MusicRep item = MusicList.FirstOrDefault(m => m.Name.Equals(musicSelected.Name) && m.Author.Equals(musicSelected.Author));
                    int index = MusicList.IndexOf(item);

                    if (index != -1)
                    {
                        MusicList.RemoveAt(index);
                        MusicList.Insert(index, musicSelected);
                    }
                }
            }
        }

        private async Task LoadSingersAsync()
        {
            try
            {
                var musicOwner = LoggedUserHelper.GetEmail();
                _singerUserList = await _userService.GetSingers(musicOwner);

                Singers.Clear();

                _singerUserList.ForEach(i => Singers.Add(i.Name));
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.CouldNotLoadSingers, AppResources.Ok);
            }
        }

        private async Task LoadMusicsAsync()
        {
            try
            {
                await LoggedUserHelper.UpdateLoggedUserAsync();

                var userEmail = LoggedUserHelper.GetEmail();
                List<Music> musicListUpdated = await _musicService.GetMusicsByUserDescending(userEmail);

                MusicList.Clear();

                foreach (Music music in musicListUpdated)
                {
                    MusicList.Add(new MusicRep()
                    {
                        Name = music.Name,
                        Author = music.Author,
                        Owner = music.Owner,
                        IsSelected = false
                    });
                }
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.CouldNotUpdateSongList, AppResources.Ok);
            }
        }

        private void SetLoggedSinger()
        {
            if (LoggedUserHelper.LoggedUser.IsSinger)
            {
                SelectedSinger = LoggedUserHelper.LoggedUser.Name;
            }
        }

        private string GetSingerEmail()
        {
            string singerEmail = string.Empty;

            if (!string.IsNullOrEmpty(SelectedSinger))
            {
                if (SelectedSinger.Equals(LoggedUserHelper.LoggedUser.Name))
                {
                    singerEmail = LoggedUserHelper.LoggedUser.Email;
                }
                else
                {
                    var user = _singerUserList.FirstOrDefault(s => s.Name.Equals(SelectedSinger));
                    if (user != null)
                    {
                        singerEmail = user.Email;
                    }
                }
            }

            return singerEmail;
        }
        #endregion
    }
}
