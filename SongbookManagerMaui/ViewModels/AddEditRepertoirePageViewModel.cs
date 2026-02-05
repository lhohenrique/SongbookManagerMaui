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
        private MusicRep _draggedItem;
        #endregion

        #region Properties
        [ObservableProperty]
        private User selectedSinger;

        [ObservableProperty]
        private ObservableCollection<User> singers = new ObservableCollection<User>();

        [ObservableProperty]
        private DateTime date = DateTime.Now;

        [ObservableProperty]
        private TimeSpan time = TimeSpan.Zero;

        [ObservableProperty]
        private ObservableCollection<Music> filteredMusicList = new ObservableCollection<Music>();

        [ObservableProperty]
        private Music filteredSelectedMusic;

        [ObservableProperty]
        private ObservableCollection<MusicRep> repertoireMusics = new ObservableCollection<MusicRep>();

        [ObservableProperty]
        private bool isSearchEnabled = false;

        [ObservableProperty]
        private bool isMusicsVisible = false;

        [ObservableProperty]
        private string serviceName = string.Empty;
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
        protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(SelectedSinger))
            {
                IsSearchEnabled = SelectedSinger != null;
            }
            else if (e.PropertyName == nameof(FilteredSelectedMusic))
            {
                AddMusicToRepertoire();
                FilteredMusicList.Clear();
            }
        }

        [RelayCommand]
        public async Task SaveRepertoire()
        {
            try
            {
                foreach(MusicRep music in RepertoireMusics)
                {
                    var key = await _keyService.GetKeyByUser(music.SingerEmail, music.Name);
                    if (key != null)
                    {
                        music.SingerKey = key.Key;
                    }
                }

                if (_repertoire != null)
                {
                    _repertoire.ServiceName = ServiceName;
                    _repertoire.Date = Date;
                    _repertoire.Time = Time;
                    _repertoire.Musics = RepertoireMusics.ToList();

                    await _repertoireService.UpdateRepertoire(_repertoire);
                }
                else
                {
                    Repertoire newRepertoire = new Repertoire()
                    {
                        ServiceName = ServiceName,
                        Date = Date,
                        Time = Time,
                        Musics = RepertoireMusics.ToList(),
                        Owner = LoggedUserHelper.GetEmail()
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

        [RelayCommand]
        private async Task Search(string term)
        {
            try
            {
                if (!_pageLoaded)
                {
                    return;
                }

                var userEmail = LoggedUserHelper.GetEmail();
                FilteredMusicList = await _musicService.SearchMusic(term, userEmail);
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

        [RelayCommand]
        private void RemoveMusic(MusicRep music)
        {
            if(RepertoireMusics != null)
            {
                RepertoireMusics.Remove(music);

                IsMusicsVisible = RepertoireMusics.Any();
            }
        }

        [RelayCommand]
        private void StartDrag(MusicRep item)
        {
            _draggedItem = item;
        }

        [RelayCommand]
        private void Drop(MusicRep targetItem)
        {
            if (_draggedItem == null || targetItem == null || _draggedItem == targetItem)
                return;

            var oldIndex = RepertoireMusics.IndexOf(_draggedItem);
            var newIndex = RepertoireMusics.IndexOf(targetItem);

            if (oldIndex == -1 || newIndex == -1)
                return;

            RepertoireMusics.Move(oldIndex, newIndex);
        }

        private void AddMusicToRepertoire()
        {
            RepertoireMusics.Add(new MusicRep()
            {
                Name = FilteredSelectedMusic.Name,
                Author = FilteredSelectedMusic.Author,
                Owner = FilteredSelectedMusic.Owner,
                SingerName = SelectedSinger.Name,
                SingerEmail = SelectedSinger.Email
            });

            IsMusicsVisible = true;
        }

        public async Task PopulateRepertoireFieldsAsync()
        {
            await LoadSingersAsync();

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
            ServiceName = _repertoire.ServiceName;
            Date = _repertoire.Date;
            Time = _repertoire.Time;

            if (_repertoire.Musics != null)
            {
                RepertoireMusics = new(_repertoire.Musics);

                IsMusicsVisible = RepertoireMusics.Any();
            }
        }

        private async Task LoadSingersAsync()
        {
            try
            {
                var musicOwner = LoggedUserHelper.GetEmail();
                _singerUserList = await _userService.GetSingers(musicOwner);

                Singers.Clear();

                _singerUserList.ForEach(i => Singers.Add(i));
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.CouldNotLoadSingers, AppResources.Ok);
            }
        }

        private void SetLoggedSinger()
        {
            if (LoggedUserHelper.LoggedUser.IsSinger)
            {
                SelectedSinger = LoggedUserHelper.LoggedUser;
            }
        }
        #endregion
    }
}
