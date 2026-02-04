using CommunityToolkit.Maui.Core.Extensions;
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
    public partial class MusicPageViewModel : ObservableObject
    {
        #region Fields
        private IMusicService _musicService { get; set; }
        private bool _pageLoaded = false;
        private ObservableCollection<Music> _allMusics;
        #endregion

        #region Properties
        [ObservableProperty]
        private bool isUpdating;

        [ObservableProperty]
        private ObservableCollection<Music> musicList;

        [ObservableProperty]
        private int totalMusics;

        [ObservableProperty]
        private Music selectedMusic;

        [ObservableProperty]
        private bool isAllChecked = true;

        [ObservableProperty]
        private bool isCelebrationChecked;

        [ObservableProperty]
        private bool isWorshipChecked;

        [ObservableProperty]
        private bool isCommunionChecked;

        [ObservableProperty]
        private bool isHolySupperChecked;
        #endregion

        public MusicPageViewModel(IMusicService musicService)
        {
            _musicService = musicService;

            MusicList = new ObservableCollection<Music>();
            _musicService.Musics.CollectionChanged += OnMusicsCollectionChanged;
        }

        #region Methods
        private void OnMusicsCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            MusicList = _musicService.Musics;
        }

        protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(SelectedMusic))
            {
                await SelectedMusicAsync();
            }
            else if (e.PropertyName == nameof(IsAllChecked)
                || e.PropertyName == nameof(IsCelebrationChecked)
                || e.PropertyName == nameof(IsWorshipChecked)
                || e.PropertyName == nameof(IsCommunionChecked)
                || e.PropertyName == nameof(IsHolySupperChecked))
            {
                HandleMusicCategories();
            }
        }

        [RelayCommand]
        private async Task SelectedMusicAsync()
        {
            if(SelectedMusic != null)
            {
                _musicService.SetMusic(SelectedMusic);
                await Shell.Current.GoToAsync($"{nameof(PreviewMusicPage)}");
            }
        }

        [RelayCommand]
        private async Task NewMusicAsync()
        {
            _musicService.SetMusic(null);
            await Shell.Current.GoToAsync($"{nameof(AddEditMusicPage)}");
        }

        [RelayCommand]
        private async Task EditMusicAsync()
        {
            await Shell.Current.GoToAsync($"{nameof(AddEditMusicPage)}");
        }

        private async Task UpdateMusicList()
        {
            try
            {
                if (IsUpdating)
                {
                    return;
                }

                IsUpdating = true;

                await LoggedUserHelper.UpdateLoggedUserAsync();

                //List<Music> musicListUpdated = await App.Database.GetAllMusics();
                var userEmail = LoggedUserHelper.GetEmail();
                _allMusics = await _musicService.GetMusicsByUserDescending(userEmail);

                HandleMusicCategories();

                TotalMusics = MusicList.Count;
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.CouldNotUpdateSongList, AppResources.Ok);
            }
            finally
            {
                IsUpdating = false;
            }
        }

        private void HandleMusicCategories()
        {
            MusicList.Clear();
            string category = "all";

            if (IsAllChecked)
            {
                MusicList = _allMusics.ToObservableCollection();
                return;
            }
            
            if (IsCelebrationChecked)
            {
                category = "Celebration";
            }
            else if (IsWorshipChecked)
            {
                category = "Worship";
            }
            else if (IsCommunionChecked)
            {
                category = "Communion";
            }
            else if (IsHolySupperChecked)
            {
                category = "HolySupper";
            }

            MusicList = _allMusics.Where(m => m.Category is not null && m.Category.Contains(category)).ToObservableCollection();
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
                MusicList = await _musicService.SearchMusic(term, userEmail);
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.UnablePerformSearch, AppResources.Ok);
            }
        }

        [RelayCommand]
        private void AlphabeticalOrder()
        {
            var orderedList = MusicList.OrderBy(m => m.Name).ToList();

            MusicList.Clear();
            orderedList.ForEach(i => MusicList.Add(i));
        }

        [RelayCommand]
        private void MostRecentOrder()
        {
            var orderedList = MusicList.OrderByDescending(m => m.CreationDate).ToList();

            MusicList.Clear();
            orderedList.ForEach(i => MusicList.Add(i));
        }

        [RelayCommand]
        private void OldestOrder()
        {
            var orderedList = MusicList.OrderBy(m => m.CreationDate).ToList();

            MusicList.Clear();
            orderedList.ForEach(i => MusicList.Add(i));
        }

        [RelayCommand]
        private async Task Tutorial()
        {
            string message = string.Empty;
            message += "- " + AppResources.MusicsPageTutorial;
            message += "\n\n- " + AppResources.MusicsAddMusicTutorial;
            message += "\n*" + AppResources.MusicsListSharedTutorial;
            message += "\n\n- " + AppResources.MusicsPreviewMusicTutorial;
            message += "\n\n- " + AppResources.MusicsSearchTutorial;
            message += "\n\n- " + AppResources.MusicsShareListTutorial;

            await App.Current.MainPage.DisplayAlert(AppResources.Tutorial, message, AppResources.Ok);
        }
        #endregion

        #region Public Methods
        public async Task LoadingPage()
        {
            SelectedMusic = null;
            await UpdateMusicList();
            _pageLoaded = true;
        }
        #endregion
    }
}
