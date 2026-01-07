using Arya.BabyLogger.Mobile.ViewModels;

namespace Arya.BabyLogger.Mobile.Views;

public partial class FeedEntryView : ContentPage
{
	public Guid? EventId { get; set; }

	public FeedEntryView(FeedEntryViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (!EventId.HasValue)
		{
			return;
		}

		await ((ViewModels.FeedEntryViewModel)BindingContext).LoadFeedEventByIdAsync(EventId.Value);
	}
}