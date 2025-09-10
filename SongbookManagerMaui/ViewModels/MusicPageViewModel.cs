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
        private bool pageLoaded = false;
        #endregion

        #region Properties
        [ObservableProperty]
        private bool isUpdating;

        [ObservableProperty]
        private ObservableCollection<Music> musicList;

        [ObservableProperty]
        private int totalMusics;

        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        private Music selectedMusic;
        #endregion

        public MusicPageViewModel(IMusicService musicService)
        {
            _musicService = musicService;

            MusicList = new ObservableCollection<Music>();
        }

        #region Methods
        protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(SelectedMusic))
            {
                await SelectedMusicAsync();
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
                List<Music> musicListUpdated = await _musicService.GetMusicsByUserDescending(userEmail);

                MusicList.Clear();

                musicListUpdated.ForEach(i => MusicList.Add(i));

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

        [RelayCommand]
        private async Task Search()
        {
            try
            {
                if (!pageLoaded)
                {
                    return;
                }

                //List<Music> musicListUpdated = await App.Database.SearchMusic(SearchText);
                var userEmail = LoggedUserHelper.GetEmail();
                List<Music> musicListUpdated = await _musicService.SearchMusic(SearchText, userEmail);

                MusicList.Clear();

                musicListUpdated.ForEach(i => MusicList.Add(i));
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.UnablePerformSearch, AppResources.Ok);
            }
        }

        [RelayCommand]
        private async void ShareAsync()
        {
            await Shell.Current.GoToAsync($"{nameof(SharePage)}");
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
            await UpdateMusicList();
            pageLoaded = true;

        }
        #endregion
    }
}
