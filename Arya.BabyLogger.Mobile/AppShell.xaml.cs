using Arya.BabyLogger.Mobile.Views.BreastPump;
using Arya.BabyLogger.Mobile.Views.Landing;

namespace Arya.BabyLogger.Mobile;

public partial class AppShell
{
	public AppShell()
	{
		InitializeComponent();
		
		Routing.RegisterRoute(nameof(BreastPumpListPage), typeof(BreastPumpListPage));
		Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
	}
}
