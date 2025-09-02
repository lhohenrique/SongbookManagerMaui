using CommunityToolkit.Mvvm.ComponentModel;
using SongbookManagerMaui.Helpers;
using SongbookManagerMaui.Models;
using SongbookManagerMaui.Resx;
using SongbookManagerMaui.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        #endregion

        public MusicPageViewModel(IMusicService musicService)
        {
            _musicService = musicService;

            MusicList = new ObservableCollection<Music>();
        }

        #region Actions
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
