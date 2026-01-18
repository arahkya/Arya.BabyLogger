using Arya.BabyLogger.Mobile.ViewModels;
using Arya.BabyLogger.Mobile.ViewModels.Feed;

namespace Arya.BabyLogger.Mobile.Views.Feed;

public partial class FeedListPage : ContentPage
{
	public FeedListPage()
	{
		InitializeComponent();

		BindingContext = App.Services.GetService<FeedListViewModel>();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (BindingContext is FeedListViewModel viewModel)
		{
			await viewModel.LoadEvents();
		}
	}

	private void OnFilterDateSelected(object sender, DateChangedEventArgs e)
	{
		if (BindingContext is FeedListViewModel viewModel)
		{
			viewModel.FilterDateChangedCommand.Execute(e.NewDate);
		}
	}
}
