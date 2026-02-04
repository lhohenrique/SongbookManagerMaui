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
    public partial class AddEditMusicPageViewModel : ObservableObject
    {
        #region Fields
        private IMusicService _musicService;
        private IUserService _userService;
        private IKeyService _keyService;
        private string _oldName;

        private const string CATEGORY_CELEBRATION = "Celebration";
        private const string CATEGORY_WORSHIP = "Worship";
        private const string CATEGORY_COMMUNION = "Communion";
        private const string CATEGORY_HOLY_SUPPER = "HolySupper";
        private const string CATEGORY_DIVISOR = ";";
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
        private int id;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string author;
        
        [ObservableProperty]
        private string selectedKey;

        [ObservableProperty]
        private string lyrics;

        [ObservableProperty]
        private string chords;

        [ObservableProperty]
        private string version;
        
        [ObservableProperty]
        private string notes;
        
        [ObservableProperty]
        private ObservableCollection<string> keyList = new ObservableCollection<string>() { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };

        [ObservableProperty]
        private ObservableCollection<UserKey> userList = new ObservableCollection<UserKey>();

        [ObservableProperty]
        private bool hasSingers = false;

        [ObservableProperty]
        private bool isCelebrationChecked;

        [ObservableProperty]
        private bool isWorshipChecked;

        [ObservableProperty]
        private bool isCommunionChecked;

        [ObservableProperty]
        private bool isHolySupperChecked;
        #endregion

        public AddEditMusicPageViewModel(IMusicService musicService, IUserService userService, IKeyService keyService)
        {
            _musicService = musicService;
            _userService = userService;
            _keyService = keyService;

            Music = _musicService.Music;
            _oldName = Music?.Name;
        }

        #region Methods
        [RelayCommand]
        public async Task SaveMusic()
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                {
                    await App.Current.MainPage.DisplayAlert(AppResources.Invalid, AppResources.EnterNameForTheSong, AppResources.Ok);
                }
                else
                {
                    // Edit music
                    if (Music != null)
                    {
                        Music.Name = Name;
                        Music.Author = Author;
                        Music.Version = Version;
                        Music.Key = SelectedKey;
                        Music.Lyrics = Lyrics;
                        Music.Chords = Chords;
                        Music.Notes = Notes;
                        Music.Category = GetCategory();

                        //await App.Database.UpdateMusic(music);
                        await _musicService.UpdateMusic(Music, _oldName);

                        foreach (UserKey userKey in UserList)
                        {
                            if (!string.IsNullOrEmpty(userKey.Key))
                            {
                                var isUserKeyExists = await _keyService.IsUserKeyExists(userKey);
                                if (isUserKeyExists)
                                {
                                    await _keyService.UpdateKey(userKey);
                                }
                                else
                                {
                                    await _keyService.InsertKey(userKey);
                                }
                            }
                        }
                    }
                    else // Save new music
                    {
                        var newMusic = new Music()
                        {
                            Name = Name,
                            Owner = LoggedUserHelper.GetEmail(),
                            Author = Author,
                            Version = Version,
                            Key = SelectedKey,
                            Lyrics = Lyrics,
                            Chords = Chords,
                            Notes = Notes,
                            Category = GetCategory(),
                            CreationDate = DateTime.Now
                        };

                        //await App.Database.InsertMusic(newMusic);
                        await _musicService.InsertMusic(newMusic);

                        foreach (UserKey userKey in UserList)
                        {
                            if (!string.IsNullOrEmpty(userKey.Key))
                            {
                                userKey.MusicName = newMusic.Name;
                                await _keyService.InsertKey(userKey);
                            }
                        }
                    }

                    await Shell.Current.GoToAsync($"//{nameof(MusicPage)}");
                }

            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.CouldNotSaveSong, AppResources.Ok);
            }
        }

        [RelayCommand]
        private async Task Tutorial()
        {
            string message = string.Empty;

            message += "- " + AppResources.AddMusicVersionTutorial;
            message += "\n\n- " + AppResources.AddMusicChordsTutorial;
            message += "\n*" + AppResources.AddMusicChordsLyricsTutorial;
            message += "\n\n- " + AppResources.AddMusicKeysTutorial;
            message += "\n\n- " + AppResources.AddMusicSingersKeysTutorial;
            message += "\n*" + AppResources.AddMusicProfileTutorial;

            await App.Current.MainPage.DisplayAlert(AppResources.Tutorial, message, AppResources.Ok);
        }

        public async Task PopulateMusicFieldsAsync()
        {
            if (Music != null)
            {
                await LoadMusic();
            }
            else
            {
                await HandleMusicFields();
            }

            HasSingers = UserList.Any();
        }

        private async Task LoadMusic()
        {
            try
            {
                Name = Music.Name;
                Author = Music.Author;
                Version = Music.Version;
                SelectedKey = Music.Key;
                Lyrics = Music.Lyrics;
                Chords = Music.Chords;
                Notes = Music.Notes;

                LoadCategory();

                var musicOwner = LoggedUserHelper.GetEmail();
                var sharedUsers = await _userService.GetSingers(musicOwner);
                var usersKeys = await _keyService.GetKeysByOwner(musicOwner, Name);

                foreach (User user in sharedUsers)
                {
                    if (!usersKeys.Exists(u => u.UserEmail.Equals(user.Email)))
                    {
                        usersKeys.Add(new UserKey()
                        {
                            MusicName = Name,
                            UserName = user.Name,
                            UserEmail = user.Email,
                            MusicOwner = musicOwner
                        });
                    }
                }

                usersKeys.ForEach(i => UserList.Add(i));
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.CouldNotLoadSingers, AppResources.Ok);
            }
        }

        private async Task HandleMusicFields()
        {
            try
            {
                Id = 0;
                Name = string.Empty;
                Author = string.Empty;
                Version = string.Empty;
                SelectedKey = string.Empty;
                Lyrics = string.Empty;
                Chords = string.Empty;
                Notes = string.Empty;

                var musicOwner = LoggedUserHelper.GetEmail();
                var sharedUsers = await _userService.GetSingers(musicOwner);
                sharedUsers.ForEach(u => UserList.Add(new UserKey()
                {
                    UserName = u.Name,
                    UserEmail = u.Email,
                    MusicOwner = musicOwner
                }));
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.CouldNotLoadSingers, AppResources.Ok);
            }
        }

        private string GetCategory()
        {
            string category = string.Empty;

            if (IsCelebrationChecked)
            {
                category += CATEGORY_CELEBRATION + CATEGORY_DIVISOR;
            }
            if (IsWorshipChecked)
            {
                category += CATEGORY_WORSHIP + CATEGORY_DIVISOR;
            }
            if (IsCommunionChecked)
            {
                category += CATEGORY_COMMUNION + CATEGORY_DIVISOR;
            }
            if (IsHolySupperChecked)
            {
                category += CATEGORY_HOLY_SUPPER + CATEGORY_DIVISOR;
            }

            return category;
        }

        private void LoadCategory()
        {
            if(Music.Category is not null)
            {
                string[] categories = Music.Category.Split(";");
                
                IsCelebrationChecked = categories.Contains(CATEGORY_CELEBRATION);
                IsWorshipChecked = categories.Contains(CATEGORY_WORSHIP);
                IsCommunionChecked = categories.Contains(CATEGORY_COMMUNION);
                IsHolySupperChecked = categories.Contains(CATEGORY_HOLY_SUPPER);
            }
        }
        #endregion
    }
}
