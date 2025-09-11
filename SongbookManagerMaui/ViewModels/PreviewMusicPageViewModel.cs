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
    public partial class PreviewMusicPageViewModel : ObservableObject
    {
        #region Fields
        IMusicService _musicService;
        IKeyService _keyService;
        #endregion

        #region Properties
        [ObservableProperty]
        private bool isDetailsChecked = true;

        [ObservableProperty]
        private bool isChordsChecked = false;

        [ObservableProperty]
        private bool isKeysChecked = false;

        [ObservableProperty]
        private bool isNotesChecked = false;

        [ObservableProperty]
        private Music music;

        [ObservableProperty]
        private ObservableCollection<UserKey> userList = new ObservableCollection<UserKey>();

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string author;

        [ObservableProperty]
        private string lyrics;

        [ObservableProperty]
        private string chords;

        [ObservableProperty]
        private string notes;

        [ObservableProperty]
        private bool hasSingers = false;

        [ObservableProperty]
        private bool isPlayMusicVisible = false;

        [ObservableProperty]
        private ObservableCollection<string> keyList = new ObservableCollection<string>() { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        [ObservableProperty]
        private string selectedKey;
        //PropertyChanged: SetChords();
        #endregion

        public PreviewMusicPageViewModel(IMusicService musicService, IKeyService keyService)
        {
            _musicService = musicService;
            _keyService = keyService;

            Music = _musicService.Music;
        }

        #region Methods
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(SelectedKey))
            {
                SetChords();
            }
        }

        [RelayCommand]
        private async Task ShowMusicDetails()
        {
            if(Music is not null)
            {
                Name = Music.Name;
                Author = Music.Author;
                SelectedKey = Music.Key;
                Lyrics = Music.Lyrics;
                Notes = Music.Notes;

                IsPlayMusicVisible = !string.IsNullOrEmpty(Music.Version);

                await GetMusicKeys();
            }

            HasSingers = UserList.Any();
        }

        [RelayCommand]
        private async Task EditMusic()
        {
            if (Music != null)
            {
                await Shell.Current.GoToAsync($"{nameof(AddEditMusicPage)}");
            }
        }

        [RelayCommand]
        private async Task RemoveMusic()
        {
            if (Music is not null)
            {
                try
                {
                    var result = await App.Current.MainPage.DisplayAlert(AppResources.AreYouShure, AppResources.AreYouShureSongRemoved, AppResources.Yes, AppResources.No);

                    if (result)
                    {
                        await _musicService.DeleteMusic(Music);
                    }
                }
                catch (Exception)
                {
                    await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.CouldNotRemoveSong, AppResources.Ok);
                }
            }

            await Shell.Current.GoToAsync($"//{nameof(MusicPage)}");
        }

        [RelayCommand]
        private void PlayMusic()
        {
            string url = Music.Version;

            Launcher.OpenAsync(new Uri(url));
        }

        [RelayCommand]
        private async Task Tutorial()
        {
            string message = string.Empty;
            message += "- " + AppResources.PreviewMusicPlayTutorial;
            message += "\n\n- " + AppResources.PreviewMusicKeyChangeTutorial;
            message += "\n\n- " + AppResources.PreviewMusicSingersKeysTutorial;
            message += "\n\n- " + AppResources.PreviewMusicEditTutorial;
            message += "\n\n- " + AppResources.PreviewMusicRemoveTutorial;
            message += "\n\n- " + AppResources.PreviewMusicSharedListTutorial;

            await App.Current.MainPage.DisplayAlert(AppResources.Tutorial, message, AppResources.Ok);
        }

        private async Task GetMusicKeys()
        {
            try
            {
                var usersToAdd = await _keyService.GetKeysByOwner(LoggedUserHelper.GetEmail(), Name);

                UserList.Clear();
                usersToAdd.ForEach(i => UserList.Add(i));
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.UnableToLoadKeys, AppResources.Ok);
            }
        }

        private void SetChords()
        {
            if (Music is not null)
            {
                if (string.IsNullOrEmpty(SelectedKey))
                {
                    Chords = Music.Chords;
                }
                else
                {
                    if (SelectedKey.Equals(Music.Key))
                    {
                        Chords = Music.Chords;
                    }
                    else
                    {
                        Chords = Helpers.Utils.GetChordsAccordingKey(Music.Key, Music.Chords, SelectedKey);
                    }
                }
            }
        }
        #endregion
    }
}
