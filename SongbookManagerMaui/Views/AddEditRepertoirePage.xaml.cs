using SongbookManagerMaui.Models;
using SongbookManagerMaui.ViewModels;

namespace SongbookManagerMaui.Views;

public partial class AddEditRepertoirePage : ContentPage
{
	public AddEditRepertoirePage(AddEditRepertoirePageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

    protected async override void OnAppearing()
    {
        var viewModel = (AddEditRepertoirePageViewModel)BindingContext;
        await viewModel.PopulateRepertoireFieldsAsync();
    }

    private void MusicSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        var viewModel = (AddEditRepertoirePageViewModel)BindingContext;
        viewModel.SearchCommand.Execute(null);
    }

    private void MusicListListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var viewModel = (AddEditRepertoirePageViewModel)BindingContext;

        MusicRep musicTapped = (MusicRep)e.Item;
        int musicTappedIndex = e.ItemIndex;

        viewModel.SelectionChanged(musicTapped, musicTappedIndex);
    }
}