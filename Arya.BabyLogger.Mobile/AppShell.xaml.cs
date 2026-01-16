using Arya.BabyLogger.Mobile.Views;
using Arya.BabyLogger.Mobile.Views.BreastPump;
namespace Arya.BabyLogger.Mobile;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(FeedEntryView), typeof(FeedEntryView));
		Routing.RegisterRoute(nameof(BreastPumpListPage), typeof(BreastPumpListPage));
		Routing.RegisterRoute(nameof(BreastPumpEntryPage), typeof(BreastPumpEntryPage));
	}
}
