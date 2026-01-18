using Arya.BabyLogger.Mobile.ViewModels;
using Arya.BabyLogger.Mobile.Views;
using Arya.BabyLogger.Mobile.Views.BreastPump;
using Arya.BabyLogger.Mobile.Views.Feed;
namespace Arya.BabyLogger.Mobile;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(FeedEntryView), typeof(FeedEntryView));
		Routing.RegisterRoute(nameof(FeedEntryViewModel), typeof(FeedEntryViewModel));
		Routing.RegisterRoute(nameof(FeedListPage), typeof(FeedListPage));
		Routing.RegisterRoute(nameof(BreastPumpListPage), typeof(BreastPumpListPage));
		Routing.RegisterRoute(nameof(BreastPumpEntryPage), typeof(BreastPumpEntryPage));
	}
}
