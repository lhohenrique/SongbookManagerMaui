using Android.SE.Omapi;
using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class DataPageViewModel : ObservableObject
    {
        #region Fields
        private IRepertoireService _repertoireService;
        private IUserService _userService;
        List<User> _singerUserList = new List<User>();
        private bool _pageLoaded = false;
        #endregion

        #region Properties
        [ObservableProperty]
        private ObservableCollection<string> singers = new ObservableCollection<string>();

        [ObservableProperty]
        private ObservableCollection<string> periodsList = new ObservableCollection<string>();

        [ObservableProperty]
        private ObservableCollection<MusicData> musicDataList = new ObservableCollection<MusicData>();
        
        [ObservableProperty]
        private string selectedSinger;
        
        [ObservableProperty]
        private string selectedPeriod;

        [ObservableProperty]
        private DateTime startDate;
        
        [ObservableProperty]
        private DateTime endDate = DateTime.Now;
        
        [ObservableProperty]
        private bool isCustomPeriod = false;
        #endregion

        public DataPageViewModel(IRepertoireService repertoireService, IUserService userService)
        {
            _repertoireService = repertoireService;
            _userService = userService;
        }

        #region Methods
        protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(SelectedSinger) || e.PropertyName == nameof(SelectedPeriod))
            {
                await UpdateData();
            }
        }

        public async Task LoadPageAsync()
        {
            await LoadSingersAsync();
            PopulatePeriods();

            SelectedSinger = AppResources.All;
            SelectedPeriod = AppResources.LastMonth;

            _pageLoaded = true;

            await UpdateData();
        }

        public async Task UpdateData()
        {
            try
            {
                if (_pageLoaded)
                {
                    List<Repertoire> repertoireList = new List<Repertoire>();

                    // Get period
                    DateTime start;
                    DateTime end;
                    if (SelectedPeriod.Equals(AppResources.LastMonth))
                    {
                        end = DateTime.Now;
                        start = end.Subtract(TimeSpan.FromDays(30));
                    }
                    else if (SelectedPeriod.Equals(AppResources.Last6Months))
                    {
                        end = DateTime.Now;
                        start = end.Subtract(TimeSpan.FromDays(180));
                    }
                    else if (SelectedPeriod.Equals(AppResources.LastYear))
                    {
                        end = DateTime.Now;
                        start = end.Subtract(TimeSpan.FromDays(365));
                    }
                    else // Custom period
                    {
                        start = StartDate;
                        end = EndDate;
                    }

                    if (SelectedSinger.Equals(AppResources.All))
                    {
                        await LoggedUserHelper.UpdateLoggedUserAsync();

                        var owner = LoggedUserHelper.GetEmail();
                        repertoireList = await _repertoireService.GetRepertoiresByPeriod(owner, start, end);
                    }
                    else
                    {
                        string singerEmail = GetSingerEmail();
                        repertoireList = await _repertoireService.GetRepertoiresBySingerAndPeriod(singerEmail, start, end);
                    }

                    List<MusicData> musicsOrdered = GetMostPlayedSongs(repertoireList);

                    MusicDataList.Clear();

                    musicsOrdered.ForEach(i => MusicDataList.Add(i));
                }
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.CouldNotLoadSongData, AppResources.Ok);
            }
        }

        private List<MusicData> GetMostPlayedSongs(List<Repertoire> repertoireList)
        {
            List<MusicData> musicsOrdered = new List<MusicData>();
            List<MusicData> musicDataList = new List<MusicData>();
            List<MusicRep> musics = new List<MusicRep>();

            repertoireList.ForEach(r => musics.AddRange(r.Musics));

            foreach (MusicRep music in musics)
            {
                int index = musicDataList.FindIndex(m => m.Name.Equals(music.Name));
                if (index == -1)
                {
                    musicDataList.Add(new MusicData { Name = music.Name, Count = 1 });
                }
                else
                {
                    musicDataList[index].Count++;
                }
            }

            musicsOrdered = musicDataList.OrderByDescending(m => m.Count).ToList();

            return musicsOrdered;
        }

        private async Task LoadSingersAsync()
        {
            try
            {
                var owner = LoggedUserHelper.GetEmail();
                _singerUserList = await _userService.GetSingers(owner);

                Singers.Clear();

                Singers.Add(AppResources.All);
                _singerUserList.ForEach(i => Singers.Add(i.Name));
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.CouldNotLoadSingers, AppResources.Ok);
            }
        }

        private void PopulatePeriods()
        {
            PeriodsList.Add(AppResources.LastMonth);
            PeriodsList.Add(AppResources.Last6Months);
            PeriodsList.Add(AppResources.LastYear);
            //PeriodsList.Add(AppResources.Custom);
        }

        private string GetSingerEmail()
        {
            string singerEmail = string.Empty;

            if (!SelectedSinger.Equals(AppResources.All))
            {
                var user = _singerUserList.FirstOrDefault(s => s.Name.Equals(SelectedSinger));
                if (user != null)
                {
                    singerEmail = user.Email;
                }
            }

            return singerEmail;
        }
        #endregion
    }
}
