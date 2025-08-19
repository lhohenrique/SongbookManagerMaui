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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SongbookManagerMaui.ViewModels
{
    public class RegisterPageViewModel : ObservableObject
    {
        #region Fields
        private IUserService _userService;
        #endregion

        #region Properties
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

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

        private string _confirmPassword = string.Empty;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        private bool _result = false;
        public bool Result
        {
            get => _isBusy;
            set
            {
                this._result = value;
                SetProperty(ref _result, value);
            }
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get => _result;
            set
            {
                this._result = value;
                SetProperty(ref _isBusy, value);
            }
        }

        private string _nameErrorMessage = string.Empty;
        public string NameErrorMessage
        {
            get => _nameErrorMessage;
            set => SetProperty(ref _nameErrorMessage, value);
        }

        private string _emailErrorMessage = string.Empty;
        public string EmailErrorMessage
        {
            get => _emailErrorMessage;
            set => SetProperty(ref _emailErrorMessage, value);
        }

        private string _passwordErrorMessage = string.Empty;
        public string PasswordErrorMessage
        {
            get => _passwordErrorMessage;
            set => SetProperty(ref _passwordErrorMessage, value);
        }
        #endregion

        #region Commands
        public AsyncRelayCommand RegisterCommand { get; set; }
        #endregion

        public RegisterPageViewModel(IUserService userService)
        {
            _userService = userService;

            RegisterCommand = new AsyncRelayCommand(RegisterActionAsync);
        }

        #region Action
        private async Task RegisterActionAsync()
        {
            bool infoValid = false;

            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                infoValid = CheckInformations(Name, Email, Password, ConfirmPassword);

                if (infoValid)
                {
                    // Old DataBase structure
                    //User newUser = new User()
                    //{
                    //    Name = this.Name,
                    //    Email = this.Email,
                    //    Password = this.Password
                    //};

                    //await App.Database.RegisterUser(newUser);

                    Result = await _userService.RegisterUSer(Name, Email, Password);

                    if (Result)
                    {
                        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.UserAlreadyRegistered, AppResources.Ok);
                    }
                }
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.UnableRegisterUser, AppResources.Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion

        #region Private Methods
        private bool CheckInformations(string nameToValidate, string emailToValidate, string passwordToValidate, string confirmPasswordToValidate)
        {
            var nameValid = ValidateName(nameToValidate);
            var emailValid = ValidateEmail(emailToValidate);
            var passwordValid = ValidatePassword(passwordToValidate, confirmPasswordToValidate);

            return nameValid && emailValid && passwordValid;
        }

        private bool ValidateName(string nameToValidate)
        {
            bool isValid = false;

            if (String.IsNullOrWhiteSpace(nameToValidate))
            {
                NameErrorMessage = AppResources.GiveAName;
            }
            else
            {
                NameErrorMessage = string.Empty;
                isValid = true;
            }

            return isValid;
        }

        private bool ValidateEmail(string emailToValidate)
        {
            bool isValid = false;

            var emailPattern = "^([\\w\\.\\-]+)@([\\w\\-]+)((\\.(\\w){2,3})+)$";

            if (String.IsNullOrWhiteSpace(emailToValidate) || !(Regex.IsMatch(emailToValidate, emailPattern)))
            {
                EmailErrorMessage = AppResources.InvalidEmail;
            }
            else
            {
                EmailErrorMessage = string.Empty;
                isValid = true;
            }

            return isValid;
        }

        private bool ValidatePassword(string passwordToValidade, string confirmPasswordToValidate)
        {
            bool isValid = false;

            if (String.IsNullOrWhiteSpace(passwordToValidade) || passwordToValidade.Length < 6)
            {
                PasswordErrorMessage = AppResources.MinimumCharacterMessage;
            }
            else if (!passwordToValidade.Equals(confirmPasswordToValidate))
            {
                PasswordErrorMessage = AppResources.PasswordsNotMatch;
            }
            else
            {
                PasswordErrorMessage = string.Empty;
                isValid = true;
            }

            return isValid;
        }
        #endregion
    }
}
