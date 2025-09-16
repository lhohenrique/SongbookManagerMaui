using SongbookManagerMaui.Models;
using SongbookManagerMaui.ViewModels;

namespace SongbookManagerMaui.Views;

public partial class PreviewRepertoirePage : ContentPage
{
	public PreviewRepertoirePage(PreviewRepertoirePageViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
	}

    protected async override void OnAppearing()
    {
        var viewModel = (PreviewRepertoirePageViewModel)BindingContext;
        await viewModel.LoadPageAsync();
    }

    private async void RepertoireMusicsListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var viewModel = (PreviewRepertoirePageViewModel)BindingContext;

        MusicRep musicTapped = (MusicRep)e.Item;
        int musicTappedIndex = e.ItemIndex;

        await viewModel.SelectionChangedAction(musicTapped, musicTappedIndex);
    }
}