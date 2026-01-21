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
    public partial class RepertoirePageViewModel : ObservableObject
    {
        #region Fields
        IRepertoireService _repertoireService;
        IKeyService _keyService;
        private bool _pageLoaded = false;
        private bool _isUpdating = false;
        #endregion

        #region Properties
        [ObservableProperty]
        private Repertoire selectedRepertoire;

        [ObservableProperty]
        private string date;

        [ObservableProperty]
        private string time;

        [ObservableProperty]
        private ObservableCollection<Repertoire> repertoireList = new ObservableCollection<Repertoire>();

        [ObservableProperty]
        private string searchText = string.Empty;

        [ObservableProperty]
        private DateTime currentDate = DateTime.Today;
        #endregion

        public RepertoirePageViewModel(IRepertoireService repertoireService, IKeyService keyService)
        {
            _repertoireService = repertoireService;
            _keyService = keyService;
        }

        #region Methods
        protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(SelectedRepertoire))
            {
                await SelectedRepertoireAsync();
            }
            else if (e.PropertyName == nameof(CurrentDate))
            {
                await LoadRepertoire();
            }
        }

        private async Task LoadRepertoire()
        {
            try
            {
                if (_isUpdating)
                {
                    return;
                }

                _isUpdating = true;

                var owner = LoggedUserHelper.GetEmail();
                List<Repertoire> repertoireListUpdated = await _repertoireService.GetRepertoiresByDate(owner, CurrentDate);

                if (repertoireListUpdated.Any())
                {
                    foreach (Repertoire repertoire in repertoireListUpdated)
                    {
                        if (repertoire.Musics != null)
                        {
                            foreach (MusicRep music in repertoire.Musics)
                            {
                                if (!string.IsNullOrEmpty(repertoire.SingerEmail))
                                {
                                    var key = await _keyService.GetKeyByUser(repertoire.SingerEmail, music.Name);
                                    if (key != null)
                                    {
                                        music.SingerKey = key.Key;
                                    }
                                }
                            }
                        }
                    }
                }

                RepertoireList.Clear();

                repertoireListUpdated.ForEach(i => RepertoireList.Add(i));
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.CouldNotUpdateRepertoireList, AppResources.Ok);
            }
            finally
            {
                _isUpdating = false;
            }
            
        }

        [RelayCommand]
        private async Task Search()
        {
            try
            {
                if (!_pageLoaded)
                {
                    return;
                }

                var userEmail = LoggedUserHelper.GetEmail();
                List<Repertoire> repertoireListUpdated = await _repertoireService.SearchRepertoire(SearchText, userEmail);

                RepertoireList.Clear();

                repertoireListUpdated.ForEach(i => RepertoireList.Add(i));
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.UnablePerformSearch, AppResources.Ok);
            }
        }

        [RelayCommand]
        private async Task SelectedRepertoireAsync()
        {
            if (SelectedRepertoire != null)
            {
                _repertoireService.SetRepertoire(SelectedRepertoire);
                await Shell.Current.GoToAsync($"{nameof(PlayRepertoirePage)}");
            }
        }

        [RelayCommand]
        private async Task NewRepertoire()
        {
            _repertoireService.SetRepertoire(null);
            await Shell.Current.GoToAsync($"{nameof(AddEditRepertoirePage)}");
        }

        [RelayCommand]
        private void MostRecentOrder()
        {
            var orderedList = RepertoireList.OrderByDescending(r => r.Date).ToList();

            RepertoireList.Clear();
            orderedList.ForEach(i => RepertoireList.Add(i));
        }

        [RelayCommand]
        private void OldestOrder()
        {
            var orderedList = RepertoireList.OrderBy(r => r.Date).ToList();

            RepertoireList.Clear();
            orderedList.ForEach(i => RepertoireList.Add(i));
        }

        [RelayCommand]
        private async Task Data()
        {
            await Shell.Current.GoToAsync($"{nameof(DataPage)}");
        }

        [RelayCommand]
        private async Task Tutorial()
        {
            string message = string.Empty;
            message += "- " + AppResources.RepertoiresPageTutorial;
            message += "\n\n- " + AppResources.RepertoiresAddRepertoireTutorial;
            message += "\n\n- " + AppResources.RepertoiresPreviewRepertoireTutorial;
            message += "\n\n- " + AppResources.RepertoiresSearchTutorial;

            await App.Current.MainPage.DisplayAlert(AppResources.Tutorial, message, AppResources.Ok);
        }

        private async Task UpdateRepertoireList()
        {
            try
            {
                if (_isUpdating)
                {
                    return;
                }

                _isUpdating = true;

                await LoggedUserHelper.UpdateLoggedUserAsync();

                var owner = LoggedUserHelper.GetEmail();
                List<Repertoire> repertoireListUpdated = await _repertoireService.GetRepertoiresByUser(owner);

                RepertoireList.Clear();

                repertoireListUpdated.ForEach(i => RepertoireList.Add(i));
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.CouldNotUpdateRepertoireList, AppResources.Ok);
            }
            finally
            {
                _isUpdating = false;
            }
        }

        public async Task LoadingPage()
        {
            await LoadRepertoire();
            _pageLoaded = true;
        }
        #endregion
    }
}
