using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SongbookManagerMaui.Helpers;
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
    public partial class ProfilePageViewModel : ObservableObject
    {
        #region Fields
        IUserService _userService;
        #endregion

        #region Properties
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string email;
        
        [ObservableProperty]
        private bool isEditing;
        
        [ObservableProperty]
        private bool isSinger;
        
        [ObservableProperty]
        private ObservableCollection<string> menuList = new ObservableCollection<string>()
        {
            AppResources.ChangePassword,
            AppResources.Feedback,
            //AppResources.RateUs,
            AppResources.PrivacyPolicy,
            AppResources.Logout
        };
        
        [ObservableProperty]
        private string selectedMenu;
        #endregion

        public ProfilePageViewModel(IUserService userService)
        {
            _userService = userService;
        }

        #region Methods
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(SelectedMenu))
            {
                SelectedItemChangedAction();
            }
            else if(e.PropertyName == nameof(IsSinger))
            {
                UpdateUserAsync();
            }
        }

        [RelayCommand]
        public void EditUserName()
        {
            IsEditing = true;
        }

        [RelayCommand]
        public async Task SaveUserName()
        {
            try
            {
                var user = LoggedUserHelper.LoggedUser;
                user.Name = Name;

                await _userService.UpdateUser(user);
            }
            catch (Exception)
            {

            }
            finally
            {
                IsEditing = false;
            }
        }

        [RelayCommand]
        public async Task ChangePassword()
        {
            await Shell.Current.GoToAsync($"{nameof(ChangePasswordPage)}");
        }

        [RelayCommand]
        public void Feedback()
        {
            string emailTo = GlobalVariables.FromEmail;
            string subject = AppResources.FeedbackForSongbook;

            string url = "mailto:" + emailTo + "?subject=" + subject;

            Launcher.OpenAsync(new Uri(url));
        }

        [RelayCommand]
        public void RateApp()
        {
            //var activity = Android.App.Application.Context;
            //var url = $"market://details?id={(activity as Context)?.PackageName}";

            //string url = Device.RuntimePlatform == Device.iOS ? "https://itunes.apple.com/br/app/skype-para-iphone/id304878510?mt=8"
            //   : "https://play.google.com/store/apps/details?id=com.skype.raider&hl=pt_BR";

            //string url = Device.RuntimePlatform == Device.iOS ? "https://itunes.apple.com/br/app/song-folder-lite/id304878510?mt=8"
            //   : "https://play.google.com/store/search?q=Song%20Folder%20Lite&c=apps";

            //Launcher.OpenAsync(new Uri(url));
        }

        [RelayCommand]
        public void Settings()
        {
            // Navigate to settings page
        }

        [RelayCommand]
        public async Task PrivacyPolicy()
        {
            await Shell.Current.GoToAsync($"{nameof(PrivacyPolicyPage)}");
        }

        [RelayCommand]
        public async void Logout()
        {
            var result = await App.Current.MainPage.DisplayAlert(AppResources.AreYouSureYouWantToLogout, string.Empty, AppResources.Yes, AppResources.Cancel);

            if (result)
            {
                Preferences.Clear();
                LoggedUserHelper.ResetLoggedUser();

                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }

        [RelayCommand]
        public async Task NavigateToAdminPage()
        {
            await Shell.Current.GoToAsync($"{nameof(AdminPage)}");
        }

        public void OnAppearing()
        {
            Name = LoggedUserHelper.LoggedUser.Name;
            Email = LoggedUserHelper.LoggedUser.Email;
            IsSinger = LoggedUserHelper.LoggedUser.IsSinger;
            IsEditing = false;

            if (Email.Equals(GlobalVariables.AdminEmail) && !MenuList.Contains(GlobalVariables.AdminLabel))
            {
                MenuList.Add(GlobalVariables.AdminLabel);
            }
        }

        public void SelectedItemChangedAction()
        {
            if (!string.IsNullOrEmpty(SelectedMenu))
            {
                if (SelectedMenu.Equals(AppResources.ChangePassword))
                {
                    ChangePassword();
                }
                else if (SelectedMenu.Equals(AppResources.Feedback))
                {
                    Feedback();
                }
                else if (SelectedMenu.Equals(AppResources.RateUs))
                {
                    RateApp();
                }
                else if (SelectedMenu.Equals(AppResources.PrivacyPolicy))
                {
                    PrivacyPolicy();
                }
                else if (SelectedMenu.Equals(AppResources.Logout))
                {
                    Logout();
                }
                else if (SelectedMenu.Equals(GlobalVariables.AdminLabel))
                {
                    NavigateToAdminPage();
                }

                SelectedMenu = string.Empty;
            }
        }

        private async void UpdateUserAsync()
        {
            try
            {
                var user = LoggedUserHelper.LoggedUser;
                user.IsSinger = IsSinger;

                await _userService.UpdateUser(user);
            }
            catch (Exception)
            {

            }
        }
        #endregion
    }
}
