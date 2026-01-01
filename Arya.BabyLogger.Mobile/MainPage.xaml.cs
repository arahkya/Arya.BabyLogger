namespace Arya.BabyLogger.Mobile;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

		BindingContext = new ViewModels.MainPageViewModel();
	}

}
