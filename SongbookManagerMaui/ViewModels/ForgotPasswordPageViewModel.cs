using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SongbookManagerMaui.Helpers;
using SongbookManagerMaui.Models;
using SongbookManagerMaui.Resx;
using SongbookManagerMaui.Services;
using SongbookManagerMaui.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SongbookManagerMaui.ViewModels
{
    public class ForgotPasswordPageViewModel : ObservableObject
    {
        #region Fields
        private IUserService _userService;
        private SmtpClient _smtpServer;
        private string _newPassword;
        private User _user;
        #endregion

        #region Properties
        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        #endregion

        #region Commands
        public AsyncRelayCommand ResetPasswordCommand { get; set; }
        #endregion

        public ForgotPasswordPageViewModel(IUserService userService)
        {
            _userService = userService;

            ResetPasswordCommand = new AsyncRelayCommand(ResetPasswordActionAsync);
        }

        #region Action
        public async Task ResetPasswordActionAsync()
        {
            try
            {
                _user = await _userService.GetUser(Email);

                if (_user != null)
                {
                    // Create a random password with 6 characters
                    Random rd = new Random();
                    int randomNumber = rd.Next(100000, 999999);
                    _newPassword = randomNumber.ToString();

                    // Enviar email com nova senha
                    _smtpServer = new SmtpClient("smtp.gmail.com");
                    _smtpServer.Port = 587;
                    _smtpServer.Host = "smtp.gmail.com";
                    _smtpServer.EnableSsl = true;
                    _smtpServer.UseDefaultCredentials = false;
                    _smtpServer.Credentials = new NetworkCredential(GlobalVariables.FromEmail, GlobalVariables.Password);

                    MailMessage message = new MailMessage();
                    message.From = new MailAddress(GlobalVariables.FromEmail);
                    message.To.Add(Email);
                    message.Subject = GlobalVariables.Subject;
                    message.Body = GlobalVariables.Body.Replace("XXXXXX", _newPassword);
                    message.IsBodyHtml = true;

                    _smtpServer.SendAsync(message, "xyz123d");

                    _smtpServer.SendCompleted += SmtpServer_SendCompleted;
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.UnregisteredUser, AppResources.Ok);
                }
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.ErrorResettingPassword, AppResources.Ok);
            }
        }

        private async void SmtpServer_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.ErrorResettingPassword, AppResources.Ok);
                }
                else if (e.Error != null)
                {
                    await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.ErrorResettingPassword, AppResources.Ok);
                }
                else
                {
                    if (_user != null)
                    {
                        _user.Password = _newPassword;
                        await _userService.UpdateUser(_user);
                    }

                    await App.Current.MainPage.DisplayAlert(AppResources.Sucess, AppResources.EmailWithNewPasswordSent, AppResources.Ok);

                    _smtpServer.SendCompleted -= SmtpServer_SendCompleted;

                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                }
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.ErrorResettingPassword, AppResources.Ok);
            }
        }
        #endregion
    }
}
