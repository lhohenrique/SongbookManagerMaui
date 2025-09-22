using SongbookManagerMaui.ViewModels;

namespace SongbookManagerMaui.Views;

public partial class ChangePasswordPage : ContentPage
{
	public ChangePasswordPage(ChangePasswordPageViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}