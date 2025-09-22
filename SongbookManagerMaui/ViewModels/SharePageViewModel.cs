using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
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
    public partial class SharePageViewModel : ObservableObject
    {
        #region Fields
        private IUserService _userService;
        private IKeyService _keyService;
        #endregion

        #region Properties
        [ObservableProperty]
        private ObservableCollection<User> userList = new ObservableCollection<User>();
        
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string sharedName;
        
        [ObservableProperty]
        private string sharedEmail;

        [ObservableProperty]
        private bool isShareEnabled = false;

        [ObservableProperty]
        private bool isUpdating = false;

        [ObservableProperty]
        private bool isSharedList = false;
        #endregion

        public SharePageViewModel(IUserService userService, IKeyService keyService)
        {
            _userService = userService;
            _keyService = keyService;
        }

        #region Methods
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(Email))
            {
                HandleShareButton();
            }
        }

        [RelayCommand]
        private async Task Share()
        {
            try
            {
                var userToShare = await _userService.GetUser(Email);

                if (userToShare == null)
                {
                    await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.UserNotFound, AppResources.Ok);
                }
                else if (!string.IsNullOrEmpty(userToShare.SharedList))
                {
                    await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.UserAlreadyAccessingSharedSongList, AppResources.Ok);
                }
                else
                {
                    userToShare.SharedList = LoggedUserHelper.GetEmail();
                    await _userService.UpdateUser(userToShare);

                    Email = string.Empty;

                    await UpdateUserList();
                }
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.UnableShareSongList, AppResources.Ok);
            }
        }

        [RelayCommand]
        private async Task UpdateUserList()
        {
            try
            {
                if (IsUpdating)
                {
                    return;
                }

                IsUpdating = true;

                var userEmail = LoggedUserHelper.GetEmail();
                List<User> userListUpdated = await _userService.GetSharedUsers(userEmail);

                UserList.Clear();

                userListUpdated.ForEach(i => UserList.Add(i));
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.CouldNotUpdateSharedUserList, AppResources.Ok);
            }
            finally
            {
                IsUpdating = false;
            }
        }

        [RelayCommand]
        private async Task RemoveShare(User user)
        {
            try
            {
                var result = await App.Current.MainPage.DisplayAlert(AppResources.AreYouShure, AppResources.ThisUserWillBeRemovedFromYourShareList, AppResources.Yes, AppResources.No);

                if (result)
                {
                    var userToRemove = await _userService.GetUser(user.Email);
                    userToRemove.SharedList = string.Empty;

                    await _userService.UpdateUser(userToRemove);

                    await UpdateUserList();

                    // Remove all keys from this user
                    await _keyService.ClearUserKeys(user.Email);
                }
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.UnableRemoveShare, AppResources.Ok);
            }
        }

        [RelayCommand]
        private async Task Unshare()
        {
            try
            {
                var result = await App.Current.MainPage.DisplayAlert(AppResources.AreYouShure, AppResources.YouWillNoLongerHaveAccessToThisSongList, AppResources.Yes, AppResources.No);

                if (result)
                {
                    LoggedUserHelper.LoggedUser.SharedList = string.Empty;
                    await _userService.UpdateUser(LoggedUserHelper.LoggedUser);

                    await HandlePageState();

                    // Remove all keys from this user
                    await _keyService.ClearUserKeys(LoggedUserHelper.LoggedUser.Email);
                }
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.UnableToUnshare, AppResources.Ok);
            }
        }

        [RelayCommand]
        private async Task Tutorial()
        {
            string message = string.Empty;
            message += "- " + AppResources.SharePageTutorial;
            message += "\n\n- " + AppResources.SharePermissionTutorial;
            message += "\n\n- " + AppResources.SharePageNotShareTutorial;
            message += "\n\n- " + AppResources.SharePageSharedListTutorial;
            message += "\n\n- " + AppResources.SharePagePersonalListTutorial;
            await App.Current.MainPage.DisplayAlert(AppResources.Tutorial, message, AppResources.Ok);
        }

        public void HandleShareButton()
        {
            IsShareEnabled = !string.IsNullOrEmpty(Email);
        }

        public async Task HandlePageState()
        {
            var sharedEmail = LoggedUserHelper.LoggedUser.SharedList;

            if (string.IsNullOrEmpty(sharedEmail))
            {
                await UpdateUserList();

                HandleShareButton();

                IsSharedList = false;
            }
            else
            {
                var sharedUser = await _userService.GetUser(sharedEmail);
                if (sharedUser != null)
                {
                    SharedName = sharedUser.Name;
                    SharedEmail = sharedUser.Email;
                }

                IsSharedList = true;
            }
        }
        #endregion
    }
}
