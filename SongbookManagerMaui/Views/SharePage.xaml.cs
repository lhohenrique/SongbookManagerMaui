using SongbookManagerMaui.ViewModels;

namespace SongbookManagerMaui.Views;

public partial class SharePage : ContentPage
{
	public SharePage(SharePageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

    protected override async void OnAppearing()
    {
        var viewModel = (SharePageViewModel)BindingContext;
        await viewModel.HandlePageState();
    }
}