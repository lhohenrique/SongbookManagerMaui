using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class AdminPageViewModel : ObservableObject
    {
        #region Fields
        private IUserService _userService;
        private IMusicService _musicService;
        #endregion

        #region Properties
        [ObservableProperty]
        private bool isUsersChecked = true;

        [ObservableProperty]
        private bool isMusicsChecked = false;

        [ObservableProperty]
        private ObservableCollection<Music> musicList = new ObservableCollection<Music>();

        [ObservableProperty]
        private ObservableCollection<User> userList = new ObservableCollection<User>();

        [ObservableProperty]
        private int totalUsers;

        [ObservableProperty]
        private int totalMusics;
        #endregion

        public AdminPageViewModel(IUserService userService, IMusicService musicService)
        {
            _userService = userService;
            _musicService = musicService;
        }

        #region Methods
        public async Task LoadingPageAsync()
        {
            await GetUsersAsync();
            await GetMusicsAsync();

            TotalUsers = UserList.Count;
            TotalMusics = MusicList.Count;
        }

        public async Task RemoveMusicAction(Music music)
        {
            try
            {
                await _musicService.DeleteMusic(music);

                await GetMusicsAsync();
                TotalMusics = MusicList.Count;
            }
            catch (Exception)
            {

            }
        }

        public async Task RemoveUserAction(User user)
        {
            try
            {
                await _userService.DeleteUser(user);

                await GetUsersAsync();
                TotalUsers = UserList.Count;
            }
            catch (Exception)
            {

            }
        }

        public async Task GetUsersAsync()
        {
            try
            {
                List<User> users = await _userService.GetUsers();

                UserList.Clear();
                users.ForEach(u => UserList.Add(u));
            }
            catch (Exception)
            {

            }
        }

        public async Task GetMusicsAsync()
        {
            try
            {
                List<Music> musics = await _musicService.GetMusics();

                MusicList.Clear();
                musics.ForEach(m => MusicList.Add(m));
            }
            catch (Exception)
            {

            }
        }
        #endregion
    }
}
