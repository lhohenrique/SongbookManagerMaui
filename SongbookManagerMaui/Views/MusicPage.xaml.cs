namespace SongbookManagerMaui.Views;

public partial class MusicPage : ContentPage
{
	public MusicPage()
	{
		InitializeComponent();
	}

    private void MusicSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        //var viewModel = (MusicPageViewModel)BindingContext;
        //viewModel.SearchCommand.Execute(null);
    }
}