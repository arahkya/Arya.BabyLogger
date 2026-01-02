namespace Arya.BabyLogger.Mobile.Views;

public partial class FeedEntryView : ContentPage
{
	public FeedEntryView()
	{
		InitializeComponent();

		BindingContext = new ViewModels.FeedEntryViewModel();
	}
}