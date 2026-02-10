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
    public partial class PreviewRepertoirePageViewModel : ObservableObject
    {
        #region Fields
        IRepertoireService _repertoireService;
        IKeyService _keyService;
        
        private bool _isReordering = false;
        private MusicRep _musicToReorder;
        private int _musicIndexToReorder = -1;
        #endregion

        #region Properties
        [ObservableProperty]
        private bool isPlayRepertoireEnabled = true;

        [ObservableProperty]
        private Repertoire repertoire;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string date;

        [ObservableProperty]
        private string time;

        [ObservableProperty]
        private ObservableCollection<MusicRep> musics = new ObservableCollection<MusicRep>();
        #endregion

        public PreviewRepertoirePageViewModel(IRepertoireService repertoireService, IKeyService keyService)
        {
            _repertoireService = repertoireService;
            _keyService = keyService;

            Repertoire = _repertoireService.Repertoire;
        }

        #region Methods
        [RelayCommand]
        public async Task PlayRepertoire()
        {
            if (Repertoire != null)
            {
                _repertoireService.SetRepertoire(Repertoire);
                await Shell.Current.GoToAsync($"{nameof(PlayRepertoirePage)}");
            }
        }

        [RelayCommand]
        public async Task SendRepertoire()
        {
            await Utils.SendRepertoire(Repertoire);
        }

        [RelayCommand]
        public async Task EditRepertoire()
        {
            if (Repertoire != null)
            {
                _repertoireService.SetRepertoire(Repertoire);
                await Shell.Current.GoToAsync($"{nameof(AddEditRepertoirePage)}");
            }
        }

        [RelayCommand]
        public async Task RemoveRepertoire()
        {
            if (Repertoire != null)
            {
                try
                {
                    var result = await App.Current.MainPage.DisplayAlert(AppResources.AreYouShure, AppResources.AreYouShureRepertoireRemoved, AppResources.Yes, AppResources.No);

                    if (result)
                    {
                        await _repertoireService.DeleteRepertoire(Repertoire);
                    }
                }
                catch (Exception)
                {
                    await App.Current.MainPage.DisplayAlert(AppResources.Error, AppResources.CouldNotRemoveRepertoire, AppResources.Ok);
                }
            }

            await Shell.Current.GoToAsync($"//{nameof(RepertoirePage)}");
        }

        [RelayCommand]
        private async Task TutorialAsync()
        {
            string message = string.Empty;
            message += "- " + AppResources.PreviewRepertoirePageTutorial;
            message += "\n\n- " + AppResources.PreviewRepertoireReorderTutorial;
            message += "\n\n- " + AppResources.PreviewRepertoirePlayTutorial;
            message += "\n\n- " + AppResources.PreviewRepertoireSendTutorial;
            message += "\n\n- " + AppResources.PreviewRepertoireEditRemoveTutorial;

            await App.Current.MainPage.DisplayAlert(AppResources.Tutorial, message, AppResources.Ok);
        }

        public async Task LoadPageAsync()
        {
            try
            {
                if (Repertoire != null)
                {
                    Date = Repertoire.DateFormated;
                    Time = Repertoire.TimeFormated;

                    Musics.Clear();

                    if (Repertoire.Musics != null)
                    {
                        foreach (MusicRep music in Repertoire.Musics)
                        {
                            //if (!string.IsNullOrEmpty(Repertoire.SingerEmail))
                            //{
                            //    var key = await _keyService.GetKeyByUser(Repertoire.SingerEmail, music.Name);
                            //    if (key != null)
                            //    {
                            //        music.SingerKey = key.Key;
                            //    }
                            //}

                            Musics.Add(music);
                        }

                        if (!Musics.Any())
                        {
                            IsPlayRepertoireEnabled = false;
                        }
                    }
                    else
                    {
                        IsPlayRepertoireEnabled = false;
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion
    }
}
