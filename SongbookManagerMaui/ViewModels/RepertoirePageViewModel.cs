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
        private bool _pageLoaded = false;
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
        private bool isUpdating = false;

        [ObservableProperty]
        private string searchText = string.Empty;
        #endregion

        public RepertoirePageViewModel(IRepertoireService repertoireService)
        {
            _repertoireService = repertoireService;
        }

        #region Methods
        protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.PropertyName == nameof(SelectedRepertoire))
            {
                await SelectedRepertoireAsync();
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
                await Shell.Current.GoToAsync($"{nameof(PreviewRepertoirePage)}");
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

        [RelayCommand]
        private async Task UpdateRepertoireList()
        {
            try
            {
                if (IsUpdating)
                {
                    return;
                }

                IsUpdating = true;

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
                IsUpdating = false;
            }
        }

        public async Task LoadingPage()
        {
            await UpdateRepertoireList();
            _pageLoaded = true;
        }
        #endregion
    }
}
