using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public class LoginPageViewModel : ObservableObject
    {
        #region Fields
        private IUserService _userService;
        #endregion

        #region Properties
        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private bool _result;
        public bool Result
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                SetProperty(ref _result, value);
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _result;
            set
            {
                _result = value;
                SetProperty(ref _isBusy, value);
            }
        }
        #endregion

        #region Commands
        public AsyncRelayCommand LoginCommand { get; set; }
        public AsyncRelayCommand SignUpCommand { get; set; }
        public AsyncRelayCommand ForgotPasswordCommand { get; set; }
        #endregion

        public LoginPageViewModel(IUserService userService)
        {
            _userService = userService;

            LoginCommand = new AsyncRelayCommand(LoginActionAsync);
            SignUpCommand = new AsyncRelayCommand(SignUpAction);
            ForgotPasswordCommand = new AsyncRelayCommand(ForgotPasswordAction);
        }

        #region Actions
        private async Task SignUpAction()
        {
            await Shell.Current.GoToAsync($"{nameof(RegisterPage)}");
        }

        private async Task LoginActionAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                // Old DataBase structure
                //User userLogged = await App.Database.LoginUser(Email, Password);

                Result = await _userService.LoginUser(Email, Password);

                if (Result)
                {
                    Preferences.Set("Email", Email);
                    await Shell.Current.GoToAsync($"//{nameof(MusicPage)}");

                    // Old navigation
                    //await Application.Current.MainPage.Navigation.PushAsync(new MusicPage());
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(AppResources.Invalid, AppResources.InvalidEmailPassword, AppResources.Ok);
                }
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.CouldNotSignIn, AppResources.Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ForgotPasswordAction()
        {
            await Shell.Current.GoToAsync($"{nameof(ForgotPasswordPage)}");
        }
        #endregion

        #region [Public Methods]
        public async void OnAppearingAsync()
        {
            var loggedUserEmail = Preferences.Get("Email", string.Empty);
            if (string.IsNullOrEmpty(loggedUserEmail))
            {
                Email = string.Empty;
                Password = string.Empty;
            }
            else
            {
                await Shell.Current.GoToAsync($"//{nameof(MusicPage)}");
            }
        }
        #endregion
    }
}
