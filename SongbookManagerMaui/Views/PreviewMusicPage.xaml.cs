using SongbookManagerMaui.ViewModels;

namespace SongbookManagerMaui.Views;

public partial class PreviewMusicPage : TabbedPage
{
	public PreviewMusicPage(PreviewMusicPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        var viewModel = (PreviewMusicPageViewModel)BindingContext;
        viewModel.ShowMusicDetailsCommand.Execute(null);
    }
}