using Arya.BabyLogger.Mobile.Views;

namespace Arya.BabyLogger.Mobile;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

		BindingContext = new ViewModels.MainPageViewModel();
	}

	private async void OnFeedingButtonClicked(object sender, EventArgs e)
	{
		await Navigation.PushModalAsync(new Feeding());
	}
}
