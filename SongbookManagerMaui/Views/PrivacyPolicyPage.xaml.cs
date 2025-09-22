using SongbookManagerMaui.Helpers;

namespace SongbookManagerMaui.Views;

public partial class PrivacyPolicyPage : ContentPage
{
	public PrivacyPolicyPage()
	{
		InitializeComponent();

        var htmlSource = new HtmlWebViewSource();
        htmlSource.Html = GlobalVariables.PrivacyPolicy;
        PrivacyPolicyWebView.Source = htmlSource;
    }
}