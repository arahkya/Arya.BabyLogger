namespace Arya.BabyLogger.Mobile.Views;

public partial class FeedEntryView : ContentPage
{
	private readonly Guid? _eventId;

	public FeedEntryView(Guid? eventId = null)
	{
		InitializeComponent();

		_eventId = eventId;
		BindingContext = new ViewModels.FeedEntryViewModel();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (!_eventId.HasValue)
		{
			return;
		}

		await ((ViewModels.FeedEntryViewModel)BindingContext).LoadFeedEventByIdAsync(_eventId.Value);
	}
}