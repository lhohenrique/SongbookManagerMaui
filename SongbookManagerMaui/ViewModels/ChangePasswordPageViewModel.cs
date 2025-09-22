using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SongbookManagerMaui.Helpers;
using SongbookManagerMaui.Resx;
using SongbookManagerMaui.Services;
using SongbookManagerMaui.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongbookManagerMaui.ViewModels
{
    public partial class ChangePasswordPageViewModel : ObservableObject
    {
        #region Fields
        private IUserService _userService;
        #endregion

        #region Properties
        [ObservableProperty]
        private string newPassword;

        [ObservableProperty]
        private string confirmNewPassword;

        [ObservableProperty]
        private string passwordErrorMessage = string.Empty;
        #endregion

        public ChangePasswordPageViewModel(IUserService userService)
        {
            _userService = userService;
        }

        #region Methods
        [RelayCommand]
        public async Task ChangePassword()
        {
            if (ValidatePassword(NewPassword, ConfirmNewPassword))
            {
                try
                {
                    var user = LoggedUserHelper.LoggedUser;
                    user.Password = NewPassword;
                    await _userService.UpdateUser(user);

                    await App.Current.MainPage.DisplayAlert(AppResources.PasswordChanged, string.Empty, AppResources.Ok);

                    await Shell.Current.GoToAsync($"//{nameof(ProfilePage)}");
                }
                catch (Exception)
                {
                    await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.CouldNotChangePassword, AppResources.Ok);
                }
            }
        }

        private bool ValidatePassword(string passwordToValidade, string confirmPasswordToValidate)
        {
            bool isValid = false;

            if (!String.IsNullOrWhiteSpace(passwordToValidade) && passwordToValidade.Equals(confirmPasswordToValidate))
            {
                PasswordErrorMessage = string.Empty;
                isValid = true;
            }
            else
            {
                PasswordErrorMessage = AppResources.PasswordsNotMatch;
            }

            return isValid;
        }
        #endregion
    }
}
