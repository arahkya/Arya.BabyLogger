using Arya.BabyLogger.Mobile.Views;
namespace Arya.BabyLogger.Mobile;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(Feeding), typeof(Feeding));
	}
}
