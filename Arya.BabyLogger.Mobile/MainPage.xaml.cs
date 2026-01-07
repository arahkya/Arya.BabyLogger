using Arya.BabyLogger.Mobile.ViewModels;
using Arya.BabyLogger.Mobile.Views;

namespace Arya.BabyLogger.Mobile;

public partial class MainPage : ContentPage
{
	private readonly HttpClient httpClient;

	public MainPage(MainPageViewModel viewModel, HttpClient httpClient)
	{
		InitializeComponent();

		BindingContext = viewModel;
		this.httpClient = httpClient;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		((MainPageViewModel)BindingContext).LoadEvents();
	}

	private async void OnFeedingButtonClicked(object sender, EventArgs e)
	{
		await Shell.Current.Navigation.PushModalAsync(new FeedEntryView(new FeedEntryViewModel(httpClient)));
	}
}
